using System;
namespace voice2midiAPI.net.Models
{
    public class FileModelShort
    {
        public long Id { get; set; }
        public string Filename { get; set; }
        public DateTime CreationDate { get; set; }
        public string Author { get; set; }
        public string FileExtension { get; set; }
        public long SourceId { get; set; }
    }
}
