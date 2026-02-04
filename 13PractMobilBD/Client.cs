using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13PractMobilBD
{
    public class Client
    {
        public int CardNumber { get; set; }
        public string LastName { get; set; } = null!;
        public DateOnly BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public override string ToString()
        {
            return $"{LastName} (Карта №{CardNumber})";
        }
    }
}
