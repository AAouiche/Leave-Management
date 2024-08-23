using LeaveManagement.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.Security
{
    public interface IAccessUser
    {
        public string GetUserId();
        string GetUsername();
        Task<ApplicationUser> GetUser();
    }
}
