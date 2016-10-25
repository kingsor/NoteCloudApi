using System.Collections.Generic;
using System.Linq;

namespace NoteCloud.DataAccess
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteCloudContext _dbContext;
        public NoteRepository() : base() { }
        public NoteRepository(NoteCloudContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual IEnumerable<Note> GetAllNotes() {
            return _dbContext.Notes;
        }

        public virtual IEnumerable<Note> GetUserNotes(int userId) {
            List<NoteGroup> userNoteGroups = _dbContext.NoteGroups.Where(x => x.UserId == userId).ToList();
            IEnumerable<Note> userNotes = _dbContext.Notes.Where(x => userNoteGroups.Any(y => y.Id == x.NoteGroupId));
            return userNotes;
        }

        public virtual void Create(Note note) {
            _dbContext.Add(note);
        }

        public virtual Note GetNote(int id) {
            return _dbContext.Notes.FirstOrDefault(x => x.Id == id);
        }

        public virtual void Update(Note note) {
            _dbContext.Update(note);
        }

        public virtual void Delete(int id) {
            Note item = GetNote(id);
            _dbContext.Remove(item);
        }
    }
}