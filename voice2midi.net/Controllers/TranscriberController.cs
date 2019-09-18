using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using voice2midiAPI.Models;
using voice2midiAPI.net.Managers;
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
        [HttpGet("generate/midi/{id}")]
        public async Task<IActionResult> GenerateMidiFile(long id)// only .wav to .mid
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

        // GET: api/transcriber/generate/{id}
        [HttpGet("generate/mp3/{id}")]
        public async Task<IActionResult> GenerateMp3File(long id)// only .wav to .mid
        {
            string filePathIn = await FileTools.ExtractToTmpFile(_context, id, ".mid");

            if (filePathIn == null)
            {
                return BadRequest();
            }

            string filePathOut = filePathIn + ".mp3";

            Console.WriteLine(filePathIn);

            var mp3Converter = new Mp3ConverterManager(true);
            await mp3Converter.run(filePathIn, filePathOut);

            var fileOutId = await FileTools.SaveToDB(_context, filePathOut, id);

            return Ok(new { filePathIn, filePathOut, fileOutId });
        }
    }
}
