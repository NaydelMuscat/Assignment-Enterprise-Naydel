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

                log.Log("File was created successfully", HttpContext.Connection.RemoteIpAddress.ToString(), User.Identity.Name);
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
                    string error = "Updated successfully";
                    ViewBag.Error = error;
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

        //public IActionResult Share(Acl acl)

        //{           
        //        fileService.ShareFile(acl.FileIdFk, acl.UserName, acl);
        //        var getFile = fileService.GetFiles().SingleOrDefault(x => x.Id == acl.FileIdFk);


        //    return View(getFile);
        //}

        public IActionResult ListAcl()
        {
            var listAcl = fileService.GetUsers();
            return View(listAcl);
        }

        //fileService.ShareFile(acl.FileIdFk, acl.UserName, acl);
        //ViewBag.Message = "File was shared successfully";
        //var listFiles = fileService.GetFile();
        //return View("ListFiles", listFiles);



        //var file = fileService.GetFile(id);
        //if (file == null)
        //{
        //    ViewBag.Error = "File doesn't exist, Cannot share file";
        //    var listfile = fileService.GetFiles();
        //    return View("ListFiles", listfile);
        //}
        //else return View(file);
    }
}