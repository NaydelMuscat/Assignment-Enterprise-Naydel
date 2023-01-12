using BusinessLogic.Services;
using BusinessLogic.ViewModels;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Linq;



namespace WebApplication.Controllers
{
    public class FileController : Controller
    {
        private FileService fileService;
        private TextFileDbRepository textFileDbRepository;
        private IWebHostEnvironment webHostEnvironment;
        private LogService log;
        public FileController(FileService _fileService, TextFileDbRepository textFileDbRepository,
                             IWebHostEnvironment _webHostEnvironment, LogService _log)
        {
            fileService = _fileService;
            this.textFileDbRepository = textFileDbRepository;
            this.webHostEnvironment = _webHostEnvironment;
           
            this.log = _log;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(TextFile file, IFormFile filepath)
        {
            try
            {
                //Upload of file
                if (filepath != null)
                {//C:\Users\User\Desktop\Enterprise\Assignment-EnterpriseProgramming\WebApplication\Data\
                    string uniqueFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(filepath.FileName);
                    string absolutePath = webHostEnvironment.ContentRootPath + @"\Data\" + uniqueFileName;
                    //string absolutePath = webHostEnvironment.WebRootPath + @"\Data\" + uniqueFileName;
                    using (var destinationFile = System.IO.File.Create(absolutePath))
                    {
                        filepath.CopyTo(destinationFile);
                    }
                    file.FilePath = "/Data/" + uniqueFileName;
                    string[] lines = System.IO.File.ReadAllLines(absolutePath);
                    file.Data = string.Join("", lines);
                }
                var createFile = fileService.CreateFile(Guid.NewGuid(), file.Data, file.Author, file.FilePath);
               
                fileService.CreatePermissions(createFile, file.Author, true);

                log.Log("File was created successfully", HttpContext.Connection.RemoteIpAddress.ToString(), file.Author);
                string msg = "File was created successfully";
                ViewBag.Message = msg;
            }
            catch (Exception e)
            {
                log.Log(e, HttpContext.Connection.RemoteIpAddress.ToString(), file.Author);
                string error = "File was not created successfully";
                ViewBag.Error = error;
            }
            return View();
        }
        public IActionResult ListFiles()
        {
            var listFiles = fileService.GetFiles();
            return View(listFiles);
        }

        [HttpGet]
        public IActionResult editFile(Guid id)
        {
            var currentFile = fileService.GetFile(id);
            ListFileViewModels myModel = new ListFileViewModels()
            {
                FileName = currentFile.FileName,
                Data = currentFile.Data
            };
            return View(myModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult editFile(Guid filename, string changes, ListFileViewModels file)
        {
            try
            {
                if (textFileDbRepository.GetPermissions().Where(x => x.FileName == filename && x.UserName == User.Identity.Name 
                                                                && x.UserAccess == true).FirstOrDefault() != null)
                {
                    file.FileName = filename;
                    file.LastEditedBy = User.Identity.Name;
                    changes = file.Data;
                    file.DigitalSignature = Convert.ToBase64String(fileService.DigitalSign(changes));
                    fileService.EditFile(filename, changes, file);
                    
                    log.Log("File was updated successfully", HttpContext.Connection.RemoteIpAddress.ToString(), User.Identity.Name);
                    string msg = "Updated successfully";
                    ViewBag.Message = msg;
                }
                else
                {
                    throw new Exception("user does not have access to edit file");

                }
            }

            catch (Exception ex)
            {
                string error = "Was Not Updated Successfully!";
                ViewBag.Error = error;
                log.Log(ex, HttpContext.Connection.RemoteIpAddress.ToString(), file.Author);
            }
            return RedirectToAction("ListFiles");
        }

        [HttpGet]
        public IActionResult ShareFile(Guid fileId)

        {
            var file = fileService.GetFile(fileId);
            var acl = new Acl { FileName = file.FileName, UserName = User.Identity.Name };

            return View(acl);
        }

        [HttpPost]
        public IActionResult ShareFile(Guid fileid, string user)
        {
            try
            {
                var file = fileService.GetFile(fileid);
                if(file != null)
                {
                    fileService.ShareFile(fileid, user);
                    string msg = "File has been shared successfully";
                    ViewBag.Message = msg;
                }
            }
            catch(Exception ex)
            {
                string msg = "File was not shared successfully";
                ViewBag.Message = msg;
            }
            return RedirectToAction("ListFiles");
        }

        public IActionResult ListAcl()
        {
            var listAcl = fileService.GetUsers();
            return View(listAcl);
        }
       
       



       
}
}