using System.Collections.Generic;

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
            return this._dbContext.Notes;
        }

        public virtual void Create(Note note) {
            this._dbContext.Add(note);
        }
    }
}