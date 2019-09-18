using System;
using Microsoft.EntityFrameworkCore;

namespace voice2midiAPI.Models
{
    public class FileContext : DbContext //Entity framework
    {
        public FileContext(DbContextOptions<FileContext> options)
            : base(options)
        {   

        }

        public DbSet<FileModel> Files { get; set; }
    }
}
