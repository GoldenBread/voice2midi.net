﻿using System;
namespace voice2midiAPI.Models
{
    public class FileModel
    {
        public FileModel()
        {
        }

        public FileModel(
            string filename,
            string author,
            string fileExtension)// generated
        {
            Filename = filename;
            CreationDate = DateTime.UtcNow;
            Author = author;
            Data = null;
            FileExtension = fileExtension;
            SourceId = -1;
        }

        public long Id { get; set; }
        public string Filename { get; set; }
        public DateTime CreationDate { get; set; }
        public string Author { get; set; }
        public byte[] Data { get; set; }
        public string FileExtension { get; set; }
        public long SourceId { get; set; }

    }
}
