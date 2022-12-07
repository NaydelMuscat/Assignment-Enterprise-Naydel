using BusinessLogic.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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
                fileService.CreateFile(file.FileName, file.UploadedOn, file.Data, file.Author);
                ViewBag.Message = msg;
            }
            catch(Exception)
            {
                string error = "File was not created successfully";
                ViewBag.Error = error;
            }
            var listOfFiles = fileService.GetFiles();
            TextFile textFile = new TextFile();
            

            return View(textFile);
        }
    }
}
