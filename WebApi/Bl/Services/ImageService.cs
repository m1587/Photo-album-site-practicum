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
    public class ImageService : IImageService
    {
        private readonly IDataContext _dataContext;
        public ImageService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Image> GetImagesByNameAsync(string imageName)
        {
            var image = await _dataContext.Images.FirstOrDefaultAsync(i => i.ImageURL.Contains(imageName));
            return image ?? throw new KeyNotFoundException("Image not found");
        }
        public async Task<Image> GetImagesByIdAsync(int id)
        {
            var image = await _dataContext.Images.FirstOrDefaultAsync(i => i.ID == id);
            return image ?? throw new KeyNotFoundException("Image not found");
        }
        //public async Task<List<Image>> GetImagesByChallengeIdAsync(int challengeId)
        //{
        //    var images = await _dataContext.Images.Where(i => i.ChallengeId == challengeId).ToListAsync();
        //    return images ?? throw new KeyNotFoundException("No images found for this challenge.");
        //}
        public async Task AddImageAsync(Image image)
        {
            await _dataContext.Images.AddAsync(image);
            await _dataContext.SaveChangesAsync();
        }
    }
}
