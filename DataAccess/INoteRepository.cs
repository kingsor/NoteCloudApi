using System.Collections.Generic;

namespace NoteCloud.DataAccess
{
    public interface INoteRepository
    {
        IEnumerable<Note> GetAllNotes();
    }
}