using System.Collections.Generic;

namespace NoteCloud.DataAccess
{
    public interface IFollowerRepository
    {
        IEnumerable<Follower> GetAllFollowers(int userId);
        void Create(Follower follower);
        void Delete(int userId, int followerId);
        bool IsFollower(int followerId, int userId);
    }
}