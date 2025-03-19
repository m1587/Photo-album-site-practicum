using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dl.Entities
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentFolderId { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

        public Folder(int id, string name, int parentFolderId, int ownerId, bool isDeleted)
        {
            Id = id;
            Name = name;
            ParentFolderId = parentFolderId;
            OwnerId = ownerId;
            IsDeleted = isDeleted;
        }
    }
}
