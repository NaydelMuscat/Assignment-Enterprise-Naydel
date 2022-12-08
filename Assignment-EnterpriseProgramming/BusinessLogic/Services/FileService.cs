using DataAccess.Repositories;
using Domain.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;

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
        public void EditFile(int fileId, string changes, int userId)
        {

            //Permissions
            if (textFileDbRepository.GetPermissions().Where(x => x.FileIdFk == fileId && x.Id == userId && x.UserAccess == true).Any())
            {

                textFileDbRepository.EditFile(fileId, changes, new Domain.Models.TextFile()
                {
                    Id = fileId,
                    LastUpdated = DateTime.Now,
                    Data = changes

                });
                //Digital Signature
                var signature = DigitalSign(changes);
                
            }
            else
            {
                throw new Exception("user does not have access to edit file");
            }
        }
        public IQueryable<Acl> GetPermissions( )
        {
            var permissions = from access in textFileDbRepository.GetPermissions()
                              select new Acl()
                              {                                 
                                  FileIdFk = access.FileIdFk,
                                  Id = access.Id,
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

        private byte[] DigitalSign(string changes)
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
