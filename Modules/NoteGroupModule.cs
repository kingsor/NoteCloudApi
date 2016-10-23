using Nancy;
using NoteCloud.DataAccess;
using Nancy.ModelBinding;
using NoteCloud.Helpers;
using System.Linq;

namespace NoteCloud.Modules
{
    public class NoteGroupModule : NancyModule
    {
        private UnitOfWork _unitOfWork;
        private CurrentUser _currentUser;
        public NoteGroupModule(UnitOfWork unitOfWork, CurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            
            Get("/notegroups/user/{userId}", args => {
                return this._unitOfWork.NoteGroupRepository.GetUserNoteGroups(args.userId);
            });

            Post("/notegroups", args => {
                NoteGroup notegroup = this.Bind<NoteGroup>();
                _currentUser = _currentUser.GetFromAuthToken(_unitOfWork.UserRepository, Request.Headers["Authorize"].FirstOrDefault());

                notegroup.UserId = _currentUser.UserId;
                _unitOfWork.NoteGroupRepository.Create(notegroup);
                _unitOfWork.Save();

                return HttpStatusCode.OK;
            });
        }
    }
}