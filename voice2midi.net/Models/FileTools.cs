using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace voice2midiAPI.Models
{
    public class FileTools// Méthodes de manipulations des datas des fichiers avec la DB
    {
        public static async Task<long> SaveToDB(FileContext _context, IFormFile file)
        {
            var fileModel = new FileModel();
            fileModel.CreationDate = DateTime.UtcNow;
            fileModel.FileExtension = Path.GetExtension(file.FileName);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileModel.Data = memoryStream.ToArray();
            }

            _context.Files.Add(fileModel);
            await _context.SaveChangesAsync();

            return fileModel.Id;
        }

        public static async Task<long> SaveToDB(FileContext _context, string filePath)
        {
            var fileModel = new FileModel();

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.ReadAsync(fileModel.Data);
            }

            _context.Files.Add(fileModel);
            await _context.SaveChangesAsync();

            return fileModel.Id;
        }

        public static async Task<string> SaveToTmpFile(IFormFile file)
        {
            // full path to file in temp location
            var fileExtension = Path.GetExtension(file.FileName);
            var filePath = GetTempFileNameWithExtension(fileExtension);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public static async Task<string> ExtractToTmpFile(FileContext context, long Id)
        {
            var file = await context.Files.FindAsync(Id);

            var filePath = GetTempFileNameWithExtension(file.FileExtension);

            if (file == null)
            {
                return null;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await stream.WriteAsync(file.Data);
            }

            return filePath;
        }

        public static string GetTempFileNameWithExtension(string extension)// Pas d'extension method possible (Path = static)
        {
            var pathStr = Path.GetTempPath();
            var filename = Guid.NewGuid().ToString() + extension;
            return Path.Combine(pathStr, filename);
        }

        /*
        public static async Task<File> ExtractFileFromDB(FileContext context, long id)
        {
            return new File
        }*/
    }
}
