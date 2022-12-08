using BusinessLogic.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    public class FileController : Controller
    {
        private FileService fileService;

        public FileController(FileService _fileService)
        {
            fileService = _fileService;
        }

       
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(TextFile file)
        {
            
            try
            {
                string msg = "File was created successfully";
                fileService.CreateFile(Guid.NewGuid(), file.UploadedOn, file.Data, file.Author);
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

