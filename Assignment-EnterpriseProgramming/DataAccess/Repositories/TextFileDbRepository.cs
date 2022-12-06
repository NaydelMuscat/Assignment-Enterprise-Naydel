﻿using DataAccess.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class TextFileDbRepository
    {
        private FileSharingContext fileSharingContext;

        public TextFileDbRepository(FileSharingContext _fileSharingContext)
        {
            _fileSharingContext = fileSharingContext;
        }

        public void CreateFile(TextFile textFile)
        {
            fileSharingContext.TextFiles.Add(textFile);
            fileSharingContext.SaveChanges();
        }

        public IQueryable<TextFile> GetFiles()
        {
            return fileSharingContext.TextFiles;
        }

        public TextFile GetFile(int id)
        {
            return GetFiles().SingleOrDefault(x => x.Id == id);
        }



        public TextFile ShareFile(int fileId, string Recipient)
        {
            var recipient = fileSharingContext.Acls.SingleOrDefault(x => x.UserName.Equals(Recipient));

            var file = GetFiles().SingleOrDefault(x => x.Id == fileId);
            recipient.FileName = file.FileName;
            fileSharingContext.SaveChanges();
            return file;
        }

        //public void EditFile(int fileId, string changes, string editor)
        //{
        //    var existingFile = (from file in fileSharingContext.TextFiles
        //                        where file.Id == fileId
        //                        select file).FirstOrDefault();

        //    existingFile.Data = changes;

        //    existingFile.LastEditedBy = editor;
        //    fileSharingContext.SaveChanges();
        //}

        public IQueryable<Acl> GetPermissions()
        {
            return fileSharingContext.Acls;
        }



        public void EditFile(int fileId,string changes, TextFile updatedFile)
        {
            var originalFile = GetFile(updatedFile.Id);

            originalFile.Id = fileId;
            originalFile.FileName = updatedFile.FileName;
            originalFile.UploadedOn = updatedFile.UploadedOn;
            originalFile.Data = updatedFile.Data;
            originalFile.LastUpdated = updatedFile.LastUpdated;
            originalFile.LastEditedBy = updatedFile.LastEditedBy;
            fileSharingContext.SaveChanges();
        }

        //public void EditItem(Item updatedItem)
        //{
        //    var originalItem = GetItem(updatedItem.Id);

        //    originalItem.Name = updatedItem.Name;
        //    originalItem.Description = updatedItem.Description;
        //    originalItem.CategoryId = updatedItem.CategoryId;
        //    originalItem.ImagePath = updatedItem.ImagePath;
        //    originalItem.Price = updatedItem.Price;
        //    originalItem.Stock = updatedItem.Stock;

        //    context.SaveChanges();
        //}

    }
}

