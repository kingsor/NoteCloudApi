using System.Collections.Generic;
using System.Linq;

namespace NoteCloud.DataAccess
{
    public class NoteGroupRepository : INoteGroupRepository
    {
        private readonly NoteCloudContext _dbContext;
        public NoteGroupRepository() : base() { }
        public NoteGroupRepository(NoteCloudContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void Create(NoteGroup group) {
            _dbContext.NoteGroups.Add(group);
        }

        public virtual IEnumerable<NoteGroup> GetUserNoteGroups(int userId) {
            return _dbContext.NoteGroups.Where(x => x.UserId == userId);
        }

        public virtual NoteGroup GetNoteGroup(int id) {
            return _dbContext.NoteGroups.FirstOrDefault(x => x.Id == id);
        }

        public virtual void Update(NoteGroup group) {
            _dbContext.Update(group);
        }

        public virtual void Delete(int id) {
            NoteGroup item = GetNoteGroup(id);
            _dbContext.Remove(item);
        }
    }
}