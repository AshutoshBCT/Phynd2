using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhyndMVC.Models
{
    public class Token
    {
        public int userID { get; set; }
        public string email { get; set; }
        public int hospitalI { get; set; }
        public string tokenString { get; set; }
    }
}
