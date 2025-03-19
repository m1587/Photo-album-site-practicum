
using Dl.Entities;

namespace Bl.InterfaceServices
{
    public interface IFileService
    {
        public Task<File1> GetFileByIdAsync(int id);
        public Task<List<File1>> GetFilesAsync();
        public Task AddFileAsync(File1 file);
        public Task<File1> UpdateFileAsync(int id, File1 file);
        public Task<File1> RemoveFileAsync(int id);
    }
}
