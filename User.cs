using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotSteam
{
    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public List<int> Library { get; set; } = new List<int>();
        public List<int> Cart { get; set; } = new List<int>();
        public string Nickname { get; set; }
    }
}
