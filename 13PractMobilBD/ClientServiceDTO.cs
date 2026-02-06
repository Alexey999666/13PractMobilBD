using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13PractMobilBD
{
    public class ClientServiceDTO
    {
        public int IdclientServices { get; set; }
        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }

        public ClientServiceDTO() { }
    }
}
