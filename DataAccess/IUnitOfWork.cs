namespace NoteCloud.DataAccess
{
    public interface IUnitOfWork
    {
        void Dispose();
        void Save();
    }
}