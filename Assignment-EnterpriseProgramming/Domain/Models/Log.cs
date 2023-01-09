using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public Guid Id { get; set; }
         public string Msg { get; set; }
         public string Ipaddress { get; set; }
         public string User { get; set; }
         public DateTime TimeStamp { get; set; }
         public string Changes { get; set; }

    }
}
