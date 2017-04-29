namespace NoteCloud.DataAccess
{
    public interface IUnitOfWork
    {
        IFollowerRepository FollowerRepository { get; }
        INoteRepository NoteRepository { get; }
        INoteGroupRepository NoteGroupRepository { get; }
        IUserRepository UserRepository { get; }
        void Dispose();
        void Save();
    }
}