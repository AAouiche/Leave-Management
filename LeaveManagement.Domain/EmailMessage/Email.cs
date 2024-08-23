using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Entities.Email
{
    public class Email
    {
        public string Reciever { get; set; }
        public string MessageBody { get; set; }
        public string Subject { get; set; }

    }
}
