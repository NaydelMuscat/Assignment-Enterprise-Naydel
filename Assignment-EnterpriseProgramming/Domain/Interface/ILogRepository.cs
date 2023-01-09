using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interface
{
    public interface ILogRepository
    {
        void Log(string msg, string ipaddress, string user);
        void Log(Exception exception, string ipaddress, string user);
    }
}
