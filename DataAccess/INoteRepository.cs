using System.Collections.Generic;

namespace NoteCloud.DataAccess
{
    public interface INoteRepository
    {
        IEnumerable<Note> GetAllNotes();
        IEnumerable<Note> GetPrivateUserNotes(int userId);
        IEnumerable<Note> GetUserNotes(int userId);
        void Create(Note note);
        Note GetNote(int id);
        void Update(Note note);
        void Delete(int id);
    }
}