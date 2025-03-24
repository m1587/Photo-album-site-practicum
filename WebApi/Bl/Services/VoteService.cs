using Bl.DTOs;
using Bl.InterfaceServices;
using Dl;
using Dl.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Services
{
    public class VoteService : IVoteService
    {
        private readonly IDataContext _dataContext;
        public VoteService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Vote> GetVoteAsync(int userId)
        {
            return await _dataContext.Votes.FirstOrDefaultAsync(v => v.UserId == userId);
        }

        public async Task AddVoteAsync(Vote vote)
        {
            var existingVote = await GetVoteAsync(vote.UserId);

            if (existingVote != null)
            {
                // אם יש הצבעה קיימת, נמחק אותה מהתמונה הקודמת
                var previousImage = await _dataContext.Images.FirstOrDefaultAsync(i => i.ID == existingVote.ImageId);
                if (previousImage != null)
                {
                    // הסר את ההצבעה מהתמונה הקודמת
                    previousImage.Votes.Remove(existingVote);
                }

                // עדכון הצבעה בתמונה החדשה
                var image = await _dataContext.Images.FirstOrDefaultAsync(i => i.ID == vote.ImageId);
                if (image != null)
                {
                    // הוסף את ההצבעה לתמונה החדשה
                    image.Votes.Add(vote);
                }

                // עדכון ההצבעה עצמה
                _dataContext.Votes.Update(vote);
            }
            else
            {
                // אם אין הצבעה קיימת, פשוט הוסף הצבעה חדשה
                var image = await _dataContext.Images.FirstOrDefaultAsync(i => i.ID == vote.ImageId);
                if (image != null)
                {
                    // הוסף את ההצבעה לתמונה
                    image.Votes.Add(vote);
                }

                // הוספת הצבעה חדשה למערכת
                await _dataContext.Votes.AddAsync(vote);
            }

            await _dataContext.SaveChangesAsync();

        }

        public async Task<int> GetVotesCountAsync(int imageId)
        {
            return await _dataContext.Votes.CountAsync(v => v.ImageId == imageId);
        }
    }
}
