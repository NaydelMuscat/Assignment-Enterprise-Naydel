using DataAccess.context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess.Repositories
{
    public class TextFileInFileRepository
    {
        private FileInfo Context;

        public TextFileInFileRepository(FileInfo _Context)
        {
            Context = _Context;
        }

        public void CreateFile(TextFile textFile)
        {
            using (StreamWriter sw = File.AppendText("C:\\Users\\nayde\\source\\repos\\Assignment-Enterprise-Naydel\\Assignment-EnterpriseProgramming\\WebApplication\\Inventory\\Inventory.txt"))
            {
                sw.WriteLine(textFile);
            }
        }
    }
}
