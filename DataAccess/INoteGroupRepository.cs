using System.Collections.Generic;

namespace NoteCloud.DataAccess
{
    public interface INoteGroupRepository
    {
        void Create(NoteGroup group);
        IEnumerable<NoteGroup> GetUserNoteGroups(int userId);
        NoteGroup GetNoteGroup(int id);
        void Update(NoteGroup group);
        void Delete(int id);
    }
}