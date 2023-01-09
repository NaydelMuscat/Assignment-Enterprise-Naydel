using DataAccess.context;
using Domain.Interface;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class LogViaDBRepository : ILogRepository
    {

        private FileSharingContext filesharingcontext;

        public LogViaDBRepository(FileSharingContext _filesharingcontext)
        {
            this.filesharingcontext = _filesharingcontext;
        }

        public void Log(string msg, string ipaddress, string user)
        {
            var log = new Log()
            {
                Id = Guid.NewGuid(),
                Msg = msg,
                Ipaddress = ipaddress,
                User = user,
                TimeStamp = DateTime.Now
            };

            filesharingcontext.Logs.Add(log);
            filesharingcontext.SaveChanges();
        }

        public void Log(Exception exception, string ipaddress, string user)
        {
            var log = new Log()
            {
                Id = Guid.NewGuid(),
                Msg = exception.Message,
                Ipaddress = ipaddress,
                User = user,
                TimeStamp = DateTime.Now
            };

            filesharingcontext.Logs.Add(log);
            filesharingcontext.SaveChanges();
        }
    }
}
