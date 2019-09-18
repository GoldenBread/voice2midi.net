﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using voice2midiAPI.Models;
using voice2midiAPI.net.Models;

namespace voice2midiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]// Attribut qui indique que le controlleur répond aux requêtes de l'api
    public class FilesController : ControllerBase
    {
        private readonly FileContext _context;

        //        [HttpPost]
        //        public upload

        public FilesController(FileContext context)// injection de dépendance pour le contexte de DB
        {
            _context = context;
            /*
             * Garbage
             * 
            if (context.Files.Count() == 0)
            {
                // Créer une nouvelle collection File si il n'y en a pas.
                // Sans la condition, créer un "File" supplémentaire à chaque nouvelle requête.

                _context.Files.Add(new FileModel { Author = "Moi" });
                _context.SaveChanges();
            }*/
        }

        // GET: api/files
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileModel>>> GetFiles() // ActionResult serialize en JSON
        {
            return await _context.Files.ToListAsync();// méthode Entity
        }

        // GET: api/files/{id}
        [HttpGet("{id}")]// préciser le parametre
        public async Task<ActionResult<FileModel>> GetFile(long Id)
        {
            var file = await _context.Files.FindAsync(Id);
            if (file == null)
            {
                return NotFound();// code d'erreur 404
            }
            return file;// code de retour 200
        }

        // POST: api/files
        [HttpPost]
        public async Task<ActionResult<FileModel>> PostFile(FileModel file)
        {
            file.CreationDate = DateTime.UtcNow;
            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFile), new { id = file.Id }, file);// GetFile est nécessaire pour pouvoir renseigner le champ "Location" de l'en-tête,
                                                                                // nameof(GetFile) == "GetFile" (Evite de hardcode le string du nom de la methode)
            // code de retour 201: created
        }

        // PUT: api/files/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFile(long id, FileModel file)// Met à jour l'entité complète d'un File
        {
            if (id != file.Id)
            {
                return BadRequest();// code d'erreur 200
            }

            _context.Entry(file).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();// code de retour 204
        }

        // DELETE: api/files/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(long id)
        {
            var file = await _context.Files.FindAsync(id);

            if (file == null)
            {
                return NotFound();
            }

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // POST: api/files/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            long size = file.Length;
            string filePath = null;
            long fileId = -1;

            if (file.Length > 0)
            {
                filePath = await FileTools.SaveToTmpFile(file);
                fileId = await FileTools.SaveToDB(_context, file);
            }

            return Ok(new { size, filePath, fileId });//count
        }


        // GET: api/files/{id}/download
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadFile(long id)
        {
            var file = await _context.Files.FindAsync(id);

            if (file == null)
            {
                return NotFound();
            }

            return File(file.Data, System.Net.Mime.MediaTypeNames.Application.Octet, file.Filename);
        }

        [HttpGet("{id}/list")]
        public async Task<ActionResult<IEnumerable<FileModelShort>>> FileList(long id)
        {
            return await _context.Files
                .Where(files => files.SourceId == id)
                .Select(x => new FileModelShort
                {
                    Id = x.Id,
                    Filename = x.Filename,
                    CreationDate = x.CreationDate,
                    Author = x.Author,
                    FileExtension = x.FileExtension,
                    SourceId = x.SourceId
                }).ToListAsync();
        }

        /*
        [HttpGet]
        public ActionResult<IEnumerable<string>> uploadTestGet()
        {
            return new string[] { "value1", "valueOui" };
        }*/

    }
}
