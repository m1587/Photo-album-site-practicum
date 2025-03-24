using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dl.Entities
{
    public enum ERole
    {
        Admin, User
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ERole Role { get; set; }
        public DateTime CreatedAt { get;} = DateTime.Now;
        public bool IsDeleted { get; set; }

    }
}
