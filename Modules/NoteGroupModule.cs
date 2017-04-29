using Nancy;
using NoteCloud.DataAccess;
using Nancy.ModelBinding;
using NoteCloud.Helpers;
using System.Linq;

namespace NoteCloud.Modules
{
    public class NoteGroupModule : NancyModule
    {
        private IUnitOfWork _unitOfWork;
        private CurrentUser _currentUser;
        public NoteGroupModule(IUnitOfWork unitOfWork, CurrentUser currentUser)
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

            Put("/notegroups/{notegroupId}", args => {
                NoteGroup toEdit = _unitOfWork.NoteGroupRepository.GetNoteGroup(args.notegroupId);
                NoteGroup noteGroup = this.Bind<NoteGroup>();
                _currentUser = _currentUser.GetFromAuthToken(_unitOfWork.UserRepository, Request.Headers["Authorize"].FirstOrDefault());

                bool canFollowerEdit = (toEdit.CanFollowerEdit && _unitOfWork.FollowerRepository.IsFollower(_currentUser.UserId, toEdit.UserId));
                if(toEdit.UserId == _currentUser.UserId || (canFollowerEdit)) {
                    toEdit.Title = noteGroup.Title;
                    toEdit.UserId = noteGroup.UserId;
                    toEdit.CanFollowerEdit = noteGroup.CanFollowerEdit;

                    _unitOfWork.NoteGroupRepository.Update(toEdit);
                    _unitOfWork.Save();

                    return HttpStatusCode.OK;
                } else {
                    return HttpStatusCode.Unauthorized;
                }
            });

            Delete("/notegroups/{notegroupId}", args => {
                NoteGroup toDelete = _unitOfWork.NoteGroupRepository.GetNoteGroup(args.notegroupId);
                _currentUser = _currentUser.GetFromAuthToken(_unitOfWork.UserRepository, Request.Headers["Authorize"].FirstOrDefault());

                bool canFollowerEdit = (toDelete.CanFollowerEdit && _unitOfWork.FollowerRepository.IsFollower(_currentUser.UserId, toDelete.UserId));
                if(toDelete.UserId == _currentUser.UserId || (canFollowerEdit)) {
                    _unitOfWork.NoteGroupRepository.Delete(toDelete.Id);
                    _unitOfWork.Save();

                    return HttpStatusCode.OK;
                } else {
                    return HttpStatusCode.Unauthorized;
                }
            });
        }
    }
}