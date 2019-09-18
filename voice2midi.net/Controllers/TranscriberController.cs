using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using voice2midiAPI.Models;

namespace voice2midiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranscriberController : ControllerBase
    {
        private readonly FileContext _context;

        public TranscriberController(FileContext context)
        {
            _context = context;
        }

        // GET: api/transcriber/generate/{id}
        [HttpGet("generate/{id}")]
        public async Task<IActionResult> GenerateFile(long id)
        {
            string filePathIn = await FileTools.ExtractToTmpFile(_context, id);
            string filePathOut = filePathIn + ".mid";

            Console.WriteLine(filePathIn);

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"/Users/thierry/Projects/voice2midi_microservice_simplified/audio_to_midi_melodia.py";
            start.Arguments = filePathIn + " " + filePathOut + " 60";


            // Redirect output
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;

            using (Process process = Process.Start(start))
            {
                // Read output
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = await reader.ReadToEndAsync();
                    Console.WriteLine(result);
                }
            }

            var fileOutId = await FileTools.SaveToDB(_context, filePathOut);

            return Ok(new { filePathIn, filePathOut, fileOutId });
        }

        [HttpGet]
        public async Task<IActionResult> GenerateTestAsync()
        {
            var fileOutId = await FileTools.SaveToDB(_context, @"/var/folders/9c/z6130hm95tj318dnwm3n4yhr0000gn/T/a61a81de-6608-4d61-b664-84aff13b9026.wav.mid");

            return Ok(new { fileOutId });
        }
    }
}
