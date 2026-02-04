using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13PractMobilBD
{
    public class ClientServiceInfoDTO
    {
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string ServiceName { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime AppointmentDateTime { get; set; }

        public ClientServiceInfoDTO() { }
    }
}
