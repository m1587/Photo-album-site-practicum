using Bl.InterfaceServices;
using Bl.Services;
using Dl.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            this._fileService = fileService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileById(int id)
        {
            try
            {
                var file = await _fileService.GetFileByIdAsync(id);
                return file != null ? Ok(file) : NotFound(new { Message = "File not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetFiles()
        {
            try
            {
                var files = await _fileService.GetFilesAsync();
                return Ok(files);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddFile([FromBody] File1 file)
        {
            try
            {
                await _fileService.AddFileAsync(file);
                return CreatedAtAction(nameof(GetFileById), new { id = file.Id }, file);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFile(int id, [FromBody] File1 file)
        {
            try
            {
                var updatedFile = await _fileService.UpdateFileAsync(id, file);
                return updatedFile != null ? Ok(new { Message = "File updated successfully", File = updatedFile }) : NotFound(new { Message = "File not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFile(int id)
        {
            try
            {
                var deletedFile = await _fileService.RemoveFileAsync(id);
                return deletedFile != null ? Ok(new { Message = "File deleted successfully", File = deletedFile }) : NotFound(new { Message = "File not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }

    }
}
