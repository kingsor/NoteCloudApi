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
    }
}