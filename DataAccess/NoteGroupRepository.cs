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
    }
}