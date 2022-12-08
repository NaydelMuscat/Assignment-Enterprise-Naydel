using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class TextFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid FileName { get; set; }

        [Required]
        public DateTime UploadedOn { get; set; }

        [Required]
        public string Data { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        public string LastEditedBy { get; set; } 
        public Nullable<DateTime> LastUpdated { get; set; }




    }
}

