using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.validation
{
    public class UserValidation
    {
        public static void ValidateUserId(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("id", "User id cannot be less than zero.");
            }
        }
        public static void ValidateUserName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {

                throw new ArgumentException("User name cannot be null or empty.", "name");
            }
        }
    }
}
