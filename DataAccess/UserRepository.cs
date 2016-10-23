using System.Collections.Generic;
using System.Linq;

namespace NoteCloud.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly NoteCloudContext _dbContext;
        
        public UserRepository() : base() { }
        public UserRepository(NoteCloudContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual IEnumerable<User> GetAllUsers()
        {
            return _dbContext.Users;
        }

        public virtual User GetUser(string email) {
            return _dbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public virtual void Create(User user) {
            _dbContext.Users.Add(user);
        }
    }
}