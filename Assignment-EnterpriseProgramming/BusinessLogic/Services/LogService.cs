using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Services
{
    public class LogService
    {
        private ILogRepository repository { get; set; }

        public LogService(ILogRepository _repository)
        {
            this.repository = _repository;
        }

        public void Log(string msg, string ipaddress, string user)
        {
            repository.Log(msg, ipaddress, user);
        }

        public void Log(Exception exeption, string ipaddress, string user)
        {
            repository.Log(exeption, ipaddress, user);
        }
    }
}
