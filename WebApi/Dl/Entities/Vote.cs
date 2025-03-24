using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dl.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
