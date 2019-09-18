using System;
namespace voice2midiAPI.Models
{
    public class FileModel
    {
        public long Id { get; set; }
        public string Filename { get; set; }
        public DateTime CreationDate { get; set; }
        public string Author { get; set; }
        public byte[] Data { get; set; }
        public string FileExtension { get; set; }
    }
}
