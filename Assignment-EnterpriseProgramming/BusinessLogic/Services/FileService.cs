using BusinessLogic.ViewModels;
using DataAccess.Repositories;
using Domain.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
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
        public Guid CreateFile(Guid fileName, DateTime uploadedOn, string data, string author, string filepath = "")
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
                Author = author,
                FilePath = filepath
            });
            return fileName;
        }



        public void CreatePermissions(Guid fileName, string user, bool useraccess)
        {



            var file = GetFiles().SingleOrDefault(x => x.FileName == fileName);
            textFileDbRepository.CreatePermissions(new Domain.Models.Acl()
            {



                FileName = fileName,
                UserAccess = useraccess,
                UserName = user
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
                             UploadedOn = file.UploadedOn,
                             Data = file.Data,
                             LastEditedBy = file.LastEditedBy,
                             LastUpdated = file.LastUpdated,



                         };
            return result;
        }



        public IQueryable<Acl> GetUsers()
        {
            var result = from acl in textFileDbRepository.GetUsers()
                         select new Acl()
                         {
                             Id = acl.Id,
                             FileName = acl.FileName,
                             UserName = acl.UserName,
                             UserAccess = acl.UserAccess
                         };
            return result;
        }



        //GetFile
        public TextFile GetFile(Guid id)
        {
            return GetFiles().SingleOrDefault(f => f.FileName == id);
        }




        public void EditFile(Guid fileId, string changes, ListFileViewModels file)
        {



            textFileDbRepository.EditFile(fileId, changes, new Domain.Models.TextFile()
            {

                FileName = fileId,
                LastUpdated = file.LastUpdated,
                LastEditedBy = file.LastEditedBy,
                Data = changes,
                DigitalSignature = file.DigitalSignature
            });
        }




        public IQueryable<Acl> GetPermissions()
        {
            var permissions = from access in textFileDbRepository.GetPermissions()
                              select new Acl()
                              {
                                  Id = access.Id,
                              };
            return permissions;



        }



        public byte[] DigitalSign(string changes)
        {



            using SHA256 alg = SHA256.Create();
            byte[] data = Encoding.ASCII.GetBytes(changes);
            byte[] hash = alg.ComputeHash(data);



            RSAParameters SharedParameters;




            //Generate Signature
            using (RSA rsa = RSA.Create())
            {
                SharedParameters = rsa.ExportParameters(false);



                RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm(nameof(SHA256));



                return rsaFormatter.CreateSignature(hash);



            }
        }





    }
}