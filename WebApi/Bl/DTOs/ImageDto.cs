using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.DTOs
{
    public class ImageDto
    {
        public int UserId { get; set; }
        public string ImageURL { get; set; }
        public string Caption { get; set; }
    }
}
