using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Interface
{
    public interface ITextFileRepository
    {
        void CreateFile(TextFile textfile);
        void EditFile(Guid filename, string changes);
        void ShareFile(Guid filename, string recipient,Acl acl);
    }
}
