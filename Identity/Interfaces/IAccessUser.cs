using Identity.Authentication;
using Identity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Interfaces
{
    public interface IAccessUser
    {
        public string GetUserId();
        string GetUsername();
        Task<ApplicationUser> GetUser();
        Task<string> GetRole();
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
    }
}
