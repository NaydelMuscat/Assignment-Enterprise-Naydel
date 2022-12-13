using BusinessLogic.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace WebApplication.Controllers
{
    public class FileController : Controller
    {
        private FileService fileService;
        private IWebHostEnvironment hostService;
        public FileController(FileService _fileService, IWebHostEnvironment _hostService)
        {
            fileService = _fileService;
            hostService = _hostService;
        }

       
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(TextFile file, IFormFile filepath)
        {
            
            try
            {
                //Upload of file
                if (filepath !=null)
                    //absolute path
                {//C:\Users\User\Source\Repos\Assignment-Enterprise-Naydel\Assignment-EnterpriseProgramming\WebApplication\Data\

                    string uniqueFileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(filepath.FileName);

                    string absolutePath = hostService.ContentRootPath + @"\Data\" + uniqueFileName;

                    using (var destinationFile = System.IO.File.Create(absolutePath))
                    {
                        filepath.CopyTo(destinationFile);
                    }

                    file.FilePath = "/Data/" + uniqueFileName;

                }

                string msg = "File was created successfully";
                fileService.CreateFile(Guid.NewGuid(), file.UploadedOn, file.Data, file.Author, file.FilePath);
                ViewBag.Message = msg;
            }
            catch(Exception e)
            {
                string error = "File was not created successfully";
                ViewBag.Error = error;
            }
            //var listOfFiles = fileService.GetFiles();
            //TextFile textFile = new TextFile();
            

            return View();
        }


        public IActionResult ListFiles()
        {
            var listFiles = fileService.GetFiles();
                return View(listFiles);
        }
        public IActionResult Share(Acl acl)
        {
           fileService.ShareFile(acl.FileIdFk, acl.UserName, acl);
           var getFile = fileService.GetFile(acl.FileIdFk);
            return View(getFile);
        }

        [HttpGet]
        public IActionResult editFile(int id)
        {
            var currentFile = fileService.GetFile(id);

            TextFile myModel = new TextFile()
            {
                Id = currentFile.Id,
                FileName = currentFile.FileName,
                UploadedOn = DateTime.Now,
                Data = currentFile.Data,
                Author = currentFile.Author,
                LastEditedBy = currentFile.LastEditedBy,
                LastUpdated = currentFile.LastUpdated
            };
            return View(myModel);
        }


        [HttpPost]
        public IActionResult Edit(int id, TextFile file)
        {
           


            return View();
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

