using SchoolManagement.Helpers.DBContext;
using SchoolManagement.Helpers.Models;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Repositories
{
    public class AuthUserRepository : RepositoryBase<AuthUser>, IAuthUserRepository
    {
        public AuthUserRepository(SchoolDbContext context) : base(context)
        {
        }

        
    }

}
