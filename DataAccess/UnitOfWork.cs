using System;

namespace NoteCloud.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private INoteRepository _noteRepository;
        private INoteGroupRepository _noteGroupRepository;
        private IUserRepository _userRepository;
        private IFollowerRepository _followerRepository;
        private NoteCloudContext _context;
        private bool disposed = false;

        public UnitOfWork() : base() { }
        public UnitOfWork(NoteCloudContext context)
        {
            _context = context;
        }

        public virtual IFollowerRepository FollowerRepository {
            get {
                if(_followerRepository == null) {
                    _followerRepository = new FollowerRepository(_context);
                }

                return _followerRepository;
            }
        }

        public virtual INoteRepository NoteRepository
        {
            get {
                if(_noteRepository == null) {
                    _noteRepository = new NoteRepository(_context);
                }

                return _noteRepository;
            }
        }

        public virtual INoteGroupRepository NoteGroupRepository
        {
            get {
                if(_noteGroupRepository == null) {
                    _noteGroupRepository = new NoteGroupRepository(_context);
                }

                return _noteGroupRepository;
            }
        }

        public virtual IUserRepository UserRepository
        {
            get {
                if(_userRepository == null) {
                    _userRepository = new UserRepository(_context);
                }

                return _userRepository;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}