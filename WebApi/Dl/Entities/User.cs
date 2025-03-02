using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dl.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get;} = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public User() { }

        public User(int id, string name, string email, string password,int createdBy)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            CreatedBy = createdBy;
        }
        
    }
}
