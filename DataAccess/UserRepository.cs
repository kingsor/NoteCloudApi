using System.Collections.Generic;

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

        public virtual void Create(User user)
        {
            _dbContext.Add(user);
        }
    }
}