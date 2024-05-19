using Microsoft.AspNetCore.SignalR;
using SchoolManagement.Helpers.DBContext;
using SchoolManagement.Helpers.Models;
using SchoolManagement.Helpers.SignalR;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Repositories
{
    public class AuthUserRepository : RepositoryBase<AuthUser>, IAuthUserRepository
    {
        public AuthUserRepository(SchoolDbContext context, IHubContext<SchoolHub> hubContext) : base(context, hubContext)
        {
        }

        
    }

}
