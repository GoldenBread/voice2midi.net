using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace voice2midiAPI.Models
{
    public class FileTools// Méthodes de manipulations des datas des fichiers avec la DB
    {
        public static async Task<long> SaveToDB(FileContext _context, IFormFile file, long sourceId = -1)
        {
            var fileModel = new FileModel(file.FileName, null, Path.GetExtension(file.FileName));
            fileModel.CreationDate = DateTime.UtcNow;
            fileModel.FileExtension = Path.GetExtension(file.FileName);
            fileModel.SourceId = sourceId;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileModel.Data = memoryStream.ToArray();
            }

            _context.Files.Add(fileModel);
            await _context.SaveChangesAsync();

            // Get the newly created Id and set it as source
            fileModel.SourceId = fileModel.Id;
            await _context.SaveChangesAsync();

            return fileModel.Id;
        }

        public static async Task<long> SaveToDB(FileContext _context, string filePath, long sourceId = -1)
        {
            var fileModel = new FileModel(Path.GetFileName(filePath), null, Path.GetExtension(filePath));
            fileModel.SourceId = sourceId;

            int bufferSize = 512;
            byte[] buffer = new byte[bufferSize];
            int readSize = 0;
            int totalReadSize = 0;
            byte[] fullBuffer = new byte[0];// Increase of size over iterations

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                Console.WriteLine("=>fullBuffer.Length" + fullBuffer.Length);
                do
                {
                    readSize = await stream.ReadAsync(buffer, 0, bufferSize);
                    Console.WriteLine("=>READSIZE " + readSize);
                    if (readSize > 0)
                    {
                        Array.Resize(ref fullBuffer, totalReadSize + readSize);
                        Buffer.BlockCopy(buffer, 0, fullBuffer, totalReadSize, readSize);
                        totalReadSize += readSize;
                    }
                }
                while (readSize > 0);
                Console.WriteLine("=>fullBuffer.Length" + fullBuffer.Length);
            }

            Console.WriteLine($"Total Bytes read : {totalReadSize} bytes");

            fileModel.Data = fullBuffer;

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

        public static async Task<string> ExtractToTmpFile(FileContext context, long Id, string checkExtension = null)
        {
            var file = await context.Files.FindAsync(Id);

            if (checkExtension != null && file.FileExtension != checkExtension)// Usually to avoid melodia not .wav file as input
            {
                return null;
            }

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
            var filename = GetRndFilename(extension);
            return Path.Combine(pathStr, filename);
        }

        public static string GetRndFilename(string extension)
        {
            return Guid.NewGuid().ToString() + extension;
        }

    }
}
