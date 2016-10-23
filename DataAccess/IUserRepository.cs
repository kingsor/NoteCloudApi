using System.Collections.Generic;

namespace NoteCloud.DataAccess
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        void Create(User user);
        User GetUser(string email);
    }
}