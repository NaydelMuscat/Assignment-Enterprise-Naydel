using System;

namespace BusinessLogic.ViewModels
{
    public class ListFileViewModels
    {
            public int Id { get; set; }
            public Guid FileName { get; set; }
            public DateTime UploadedOn { get; set; }
            public string Data { get; set; }
            public string Author { get; set; }
            public string LastEditedBy { get; set; }
            public Nullable<DateTime> LastUpdated { get; set; }
            public string DigitalSignature { get; set; }
    }
}