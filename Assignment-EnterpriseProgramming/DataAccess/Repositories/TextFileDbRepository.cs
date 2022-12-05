using DataAccess.Context;
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

        public void EditFile(int fileId, string changes, string editor)
        {
            var existingFile = (from file in fileSharingContext.TextFiles
                                where file.Id == fileId
                                select file).FirstOrDefault();

            existingFile.Data = changes;

            existingFile.LastEditedBy = editor;
            fileSharingContext.SaveChanges();
        }
    }
}

