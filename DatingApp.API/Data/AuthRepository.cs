using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext _Context { get; }

        public AuthRepository(DataContext context)
        {
            _Context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            var Existinguser=await _Context.Users.FirstOrDefaultAsync(z=>z.Username==username);

            if(Existinguser==null)
            return null;

            if(!VerifyUser(password,Existinguser.PasswordHash,Existinguser.PasswordSalt))
            return null;

            return Existinguser;
        }

        private bool VerifyUser(string password, byte[] passwordHash, byte[] passwordSalt)
        {   
            byte[] ComputedHash;
            using(var encrypter=new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {   
                ComputedHash=encrypter.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0;i< ComputedHash.Length;i++)
                {
                    if(passwordHash[i]!=ComputedHash[i])
                    return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] PasswordHash,PasswordSalt;
            CreatePasswordHash(password,out PasswordHash,out PasswordSalt);
            user.PasswordHash=PasswordHash;
            user.PasswordSalt=PasswordSalt;
            await _Context.Users.AddAsync(user);
            await _Context.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var encrypter=new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt=encrypter.Key;
                passwordHash=encrypter.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> Userexits(string username)
        {
           if(await _Context.Users.AnyAsync(z=>z.Username.ToLower()==username.ToLower()))
            {
            return false;
            }
            return true;
        }
    }
}