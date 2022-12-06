using DataAccess.Context;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Services
{
    public class FileService
    {
        private TextFileDbRepository textFileDbRepository { get; set; }

        public FileService(TextFileDbRepository _textFileDbRepository)
        {
            textFileDbRepository = _textFileDbRepository;
        }

        //Create File
        public void CreateFile(Guid fileName, DateTime uploadedOn, string data, string author)
        {
            if (textFileDbRepository.GetFiles().Where(file => file.FileName == fileName).Count() > 0)
            {
                throw new Exception("File already exists, Please use a different name");
            }

            textFileDbRepository.CreateFile(new Domain.Models.TextFile()
            {
                FileName = fileName,
                UploadedOn = uploadedOn,
                Data = data,
                Author = author
            });
        }

        //GetFiles
        public IQueryable<TextFile> GetFiles()
        {
            var result = from file in textFileDbRepository.GetFiles()
                         select new TextFile()
                         {
                             FileName = file.FileName,
                             Author = file.Author,
                             Id = file.Id,
                             UploadedOn = file.UploadedOn,
                             Data = file.Data,
                             LastEditedBy = file.LastEditedBy,
                             LastUpdated = file.LastUpdated,

                         };
            return result;
        }

        //GetFile
        public TextFile GetFile(int fileId)
        {
            return GetFiles().SingleOrDefault(f => f.Id == fileId);
        }
        public void EditFile(int fileId, string changes,  TextFile file)
        {
            


          textFileDbRepository.EditFile(fileId ,changes,new Domain.Models.TextFile()
            {
                Id = fileId,
                LastUpdated = DateTime.Now,
                Data = changes,
                Author = file.Author,

                
            });
        }

        //public void EditItem(int id, CreateItemViewModel model)
        //{
        //    itemRepository.EditItem(
        //         new Domain.Models.Item()
        //         {
        //             Id = id,
        //             Name = model.Name,
        //             Description = model.Description,
        //             CategoryId = model.CategoryId,
        //             ImagePath = model.ImagePath,
        //             Price = model.Price,
        //             Stock = model.Stock
        //         }
        //        );

        //}
    }
}
