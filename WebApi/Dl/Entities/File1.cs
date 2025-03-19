using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dl.Entities
{
    public class File1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public long Size { get; set; }
        public string S3Key { get; set; }
        public int FolderId { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

        public File1(int id, string name, int type, long size, string s3Key, int folderId, int ownerId, bool isDeleted)
        {
            Id = id;
            Name = name;
            Type = type;
            Size = size;
            S3Key = s3Key;
            FolderId = folderId;
            OwnerId = ownerId;
            IsDeleted = isDeleted;
        }
    }
}
