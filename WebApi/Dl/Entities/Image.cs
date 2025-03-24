using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dl.Entities
{
    public class Image
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } 
        //public int ChallengeId { get; set; }
        //public Challenge Challenge { get; set; } 
        public string ImageURL { get; set; }
        public string Caption { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        public List<Vote> Votes { get; set; } = new List<Vote>();
    }
}
