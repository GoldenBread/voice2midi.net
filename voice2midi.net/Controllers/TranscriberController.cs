using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using voice2midiAPI.Models;
using voice2midiAPI.net.Tools;

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
            string filePathIn = await FileTools.ExtractToTmpFile(_context, id, ".wav");

            if (filePathIn == null)
            {
                return BadRequest();
            }

            string filePathOut = filePathIn + ".mid";

            Console.WriteLine(filePathIn);

            var melodia = new MelodiaManager(true);
            await melodia.run(filePathIn, filePathOut);

            var fileOutId = await FileTools.SaveToDB(_context, filePathOut, id);
            
            return Ok(new { filePathIn, filePathOut, fileOutId });
        }
    }
}
