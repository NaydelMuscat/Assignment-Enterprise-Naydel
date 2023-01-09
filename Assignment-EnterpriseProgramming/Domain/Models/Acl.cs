using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class Acl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        [ForeignKey("TextFileModel")]
        public Guid FileName { get; set; }
        public virtual TextFile TextFileModel { get; set; }
        public bool UserAccess { get; set; }
    }
}
