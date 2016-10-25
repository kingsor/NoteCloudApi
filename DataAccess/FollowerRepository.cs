using System.Collections.Generic;
using System.Linq;

namespace NoteCloud.DataAccess
{
    public class FollowerRepository : IFollowerRepository
    {
        private readonly NoteCloudContext _dbContext;
        public FollowerRepository() : base() { }
        public FollowerRepository(NoteCloudContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void Create(Follower follower) {
            _dbContext.Followers.Add(follower);
        }

        public virtual void Delete(int userId, int followerId) {
            Follower follower = _dbContext.Followers.SingleOrDefault(x => x.UserId == userId && x.FolloweeId == followerId);
            _dbContext.Followers.Remove(follower);
        }

        public virtual IEnumerable<Follower> GetAllFollowers(int userId) {
            return _dbContext.Followers.Where(x => x.UserId == userId);
        }

        public virtual bool IsFollower(int followerId, int userId) {
            return _dbContext.Followers.Where(x => x.UserId == userId && x.FolloweeId == followerId).Count() > 0;
        }
    }
}