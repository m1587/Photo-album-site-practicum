using Dl.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.InterfaceServices
{
    public interface IImageService
    {
        //public Task<List<Image>> GetImagesByChallengeIdAsync(int challengeId);
        public Task<Image> GetImagesByNameAsync(string nameImage);
        public Task AddImageAsync(Image image);
        public Task<Image> GetImagesByIdAsync(int id);
    }
}
