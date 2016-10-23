using System.Collections.Generic;

namespace NoteCloud.DataAccess
{
    public interface IFollowerRepository
    {
        IEnumerable<Follower> GetAllFollowers(int userId);
        void Create(Follower follower);
    }
}