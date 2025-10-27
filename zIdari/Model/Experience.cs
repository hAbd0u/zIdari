using System;

// Model/Experience.cs
namespace zIdari.Model
{
    public sealed class Experience
    {
        public int? ExperienceId { get; set; }  // nullable for new records
        public int FolderNum { get; set; }
        public int FolderNumYear { get; set; }
        public string CertRef { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}