using DataAccess.context;
using Domain.Models;
using System;
using System.Linq;



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
        public TextFile GetFile(Guid id)
        {
            return GetFiles().SingleOrDefault(x => x.FileName == id);
        }
        public IQueryable<Acl> GetUsers()
        {
            return fileSharingContext.Acls;
        }

        //public void ShareFile(Guid fileId, string Recipient)
        //{
        //    var recipient = fileSharingContext.Acls.SingleOrDefault(x => x.UserName.Equals(Recipient));
        //    var fileid = GetFile(fileId).Id;
        //    recipient.FileName = fileid;
        //    fileSharingContext.SaveChanges();
        //}
        public void CreatePermissions(Acl acl)
        {
            fileSharingContext.Acls.Add(acl);
            fileSharingContext.SaveChanges();
        }

        public IQueryable<Acl> GetPermissions()
        {
            return fileSharingContext.Acls;
        }

        public void EditFile(Guid filename, string changes, TextFile updatedFile)
        {
            var originalFile = GetFile(updatedFile.FileName);

            originalFile.FileName = filename;
            originalFile.UploadedOn = DateTime.Now;
            originalFile.Data = changes;
            originalFile.LastUpdated = updatedFile.LastUpdated;
            originalFile.LastEditedBy = updatedFile.LastEditedBy;
            fileSharingContext.SaveChanges();
        }
    }
}