using Bl.InterfaceServices;
using Bl.validation;
using Dl;
using Dl.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Services
{
    public class FileService : IFileService
    {
        private readonly IDataContext _dataContext;
        public FileService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<File1> GetFileByIdAsync(int id)
        {
            FileValidation.ValidateFileId(id);
            var file = await _dataContext.Files.FirstOrDefaultAsync(f => f.Id == id);
            return file ?? throw new KeyNotFoundException("File not found");
        }
        public async Task<List<File1>> GetFilesAsync()
        {
            return await _dataContext.Files.ToListAsync();
        }
        public async Task AddFileAsync(File1 file)
        {
            FileValidation.ValidateFileName(file.Name);
            await _dataContext.Files.AddAsync(file);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<File1> UpdateFileAsync(int id, File1 file)
        {
            FileValidation.ValidateFileId(id);
            FileValidation.ValidateFileName(file.Name);

            var existingFile = await _dataContext.Files.FirstOrDefaultAsync(f => f.Id == id);
            if (existingFile == null)
                throw new KeyNotFoundException("File not found");

            existingFile.Name = file.Name;
            existingFile.Type = file.Type;
            existingFile.Size = file.Size;
            existingFile.FolderId = file.FolderId;
            existingFile.OwnerId = file.OwnerId;
            existingFile.CreatedAt = file.CreatedAt;
            existingFile.UpdatedAt = file.UpdatedAt;
            existingFile.IsDeleted = file.IsDeleted;

            await _dataContext.SaveChangesAsync();
            return existingFile;
        }
        public async Task<File1> RemoveFileAsync(int id)
        {
            FileValidation.ValidateFileId(id);
            var fileToDelete = await _dataContext.Files.FirstOrDefaultAsync(f => f.Id == id);
            if (fileToDelete == null)
                throw new KeyNotFoundException("File not found");

            _dataContext.Files.Remove(fileToDelete);
            await _dataContext.SaveChangesAsync();
            return fileToDelete;
        }
    }
}