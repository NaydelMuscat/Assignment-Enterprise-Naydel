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
        public void EditFile(int fileId, string changes, bool access)
        {

            //Permissions
            if (textFileDbRepository.GetPermissions().Where(x => x.UserAccess == true).SingleOrDefault() != null)
            {

                textFileDbRepository.EditFile(fileId, changes, new Domain.Models.TextFile()
                {
                    Id = fileId,
                    LastUpdated = DateTime.Now,
                    Data = changes

                });
                //Digital Signature
                using SHA256 alg = SHA256.Create();
                byte[] data = Encoding.ASCII.GetBytes(changes);
                byte[] hash = alg.ComputeHash(data);

                RSAParameters SharedParameters;
                byte[] signedHash;

                //Generate Signature
                using (RSA rsa = RSA.Create())
                {
                    SharedParameters = rsa.ExportParameters(false);

                    RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                    rsaFormatter.SetHashAlgorithm(nameof(SHA256));

                    signedHash = rsaFormatter.CreateSignature(hash);

                }
            }
            else if (textFileDbRepository.GetPermissions().Where(x => x.UserAccess == false).SingleOrDefault() == null)
            {
                
               
                textFileDbRepository.EditFile(fileId,changes, new Domain.Models.TextFile()
                {
                    Id = fileId,
                    LastUpdated = DateTime.Now,
                    Data = changes
                });
            }
        }
        public IQueryable<Acl> GetPermissions()
        {
            var permissions = from access in textFileDbRepository.GetPermissions()
                              select new Acl()
                              {
                                  UserAccess = access.UserAccess

                              };
            return permissions;

        }

        public void ShareFile(int fileId, string Recipient, Acl file)
        {
            textFileDbRepository.ShareFile(fileId, Recipient);
            {
                fileId = file.FileIdFk;
                Recipient = file.UserName;
            }

        }

    }
}
