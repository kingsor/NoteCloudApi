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

        public virtual IEnumerable<Follower> GetAllFollowers(int userId) {
            return _dbContext.Followers.Where(x => x.UserId == userId);
        }
    }
}