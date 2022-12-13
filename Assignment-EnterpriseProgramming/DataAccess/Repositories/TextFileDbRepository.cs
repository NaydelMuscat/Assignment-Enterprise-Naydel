﻿using DataAccess.context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static System.Net.WebRequestMethods;

namespace DataAccess.Repositories
{
    public class TextFileDbRepository 
    {
        private FileSharingContext fileSharingContext;

        public TextFileDbRepository(FileSharingContext _fileSharingContext)
        {
            fileSharingContext = _fileSharingContext;
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

        public IQueryable<Acl> GetUser()
        {
            return fileSharingContext.Acls;
        }

        public void ShareFile(int fileId, string Recipient)
        {
            var recipient = GetUser().SingleOrDefault(x => x.UserName.Equals(Recipient));
                //fileSharingContext.Acls.SingleOrDefault(x => x.UserName.Equals(Recipient));
            var fileid = GetFiles().SingleOrDefault(x => x.Id == fileId);
            
            recipient.FileIdFk = fileId;
            fileSharingContext.SaveChanges();
           
        }

        public IQueryable<Acl> GetPermissions()
        {
            return fileSharingContext.Acls;
        }

        public void EditFile(int fileId,string changes, TextFile updatedFile)
        {
            var originalFile = GetFile(updatedFile.Id);

            originalFile.Id = fileId;
            originalFile.FileName = updatedFile.FileName;
            originalFile.UploadedOn = DateTime.Now;
            originalFile.Data = changes;
            originalFile.LastUpdated = updatedFile.LastUpdated;
            originalFile.LastEditedBy = updatedFile.LastEditedBy;
            fileSharingContext.SaveChanges();
        }

       
    }
}

