using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebAPIProject.Model
{
    public interface IUserRepository
    {
        Task<User> Register(User user);
        Task<string> Login(User user);
        Task<bool> IsUniqueUser(string userName);
        void CreatePasswordHahs(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
    }
}
