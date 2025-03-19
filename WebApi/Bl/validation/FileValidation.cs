using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.validation
{
    public class FileValidation
    {
        public static void ValidateFileId(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("id", "File id cannot be less than zero.");
            }
        }
        public static void ValidateFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {

                throw new ArgumentException("File name cannot be null or empty.", "name");
            }
        }
    }
}
