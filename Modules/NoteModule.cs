using Nancy;
using NoteCloud.DataAccess;
using Nancy.ModelBinding;
using NoteCloud.Helpers;
using System.Linq;
using System.Collections.Generic;

namespace NoteCloud.Modules
{
    public class NoteModule : NancyModule
    {
        private IUnitOfWork _unitOfWork;
        private CurrentUser _currentUser;
        public NoteModule(IUnitOfWork unitOfWork, CurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;

            Get("/public/notes/user/{userId}", args => {
                return this._unitOfWork.NoteRepository.GetUserNotes(args.userId);
            });

            Get("/private/notes/user/{userId}", args => {
                IEnumerable<Note> notes = this._unitOfWork.NoteRepository.GetPrivateUserNotes(args.userId);
                _currentUser = _currentUser.GetFromAuthToken(_unitOfWork.UserRepository, Request.Headers["Authorize"].FirstOrDefault());
                if(_currentUser == args.userId) {
                    return notes;
                } else {
                    return HttpStatusCode.Unauthorized;
                }
            });

            Post("/private/notes", args => {
                Note note = this.Bind<Note>();
                _currentUser = _currentUser.GetFromAuthToken(_unitOfWork.UserRepository, Request.Headers["Authorize"].FirstOrDefault());

                NoteGroup noteGroup = _unitOfWork.NoteGroupRepository.GetNoteGroup(note.NoteGroupId);
                bool canFollowerEdit = (noteGroup.CanFollowerEdit && _unitOfWork.FollowerRepository.IsFollower(_currentUser.UserId, noteGroup.UserId));
                if(noteGroup.UserId == _currentUser.UserId || (canFollowerEdit)) {
                    _unitOfWork.NoteRepository.Create(note);
                    _unitOfWork.Save();

                    return HttpStatusCode.OK;
                } else {
                    return HttpStatusCode.Unauthorized;
                }
            });

            Put("/private/notes/{noteId}", args => {
                Note toEdit = this._unitOfWork.NoteRepository.GetNote(args.noteId);
                Note note = this.Bind<Note>();
                _currentUser = _currentUser.GetFromAuthToken(_unitOfWork.UserRepository, Request.Headers["Authorize"].FirstOrDefault());

                NoteGroup noteGroup = _unitOfWork.NoteGroupRepository.GetNoteGroup(note.NoteGroupId);
                bool canFollowerEdit = (noteGroup.CanFollowerEdit && _unitOfWork.FollowerRepository.IsFollower(_currentUser.UserId, noteGroup.UserId));
                if(noteGroup.UserId == _currentUser.UserId || (canFollowerEdit)) {
                    toEdit.IsComplete = note.IsComplete;
                    toEdit.IsPrivate = note.IsPrivate;
                    toEdit.Text = note.Text;
                    toEdit.Priority = note.Priority;
                    toEdit.NoteGroupId = note.NoteGroupId;

                    _unitOfWork.NoteRepository.Update(toEdit);
                    _unitOfWork.Save();

                    return HttpStatusCode.OK;
                } else {
                    return HttpStatusCode.Unauthorized;
                }
            });

            Delete("/private/notes/{noteId}", args => {
                Note toDelete = this._unitOfWork.NoteRepository.GetNote(args.noteId);
                _currentUser = _currentUser.GetFromAuthToken(_unitOfWork.UserRepository, Request.Headers["Authorize"].FirstOrDefault());

                NoteGroup noteGroup = _unitOfWork.NoteGroupRepository.GetNoteGroup(toDelete.NoteGroupId);
                bool canFollowerEdit = (noteGroup.CanFollowerEdit && _unitOfWork.FollowerRepository.IsFollower(_currentUser.UserId, noteGroup.UserId));
                if(noteGroup.UserId == _currentUser.UserId || (canFollowerEdit)) {
                    _unitOfWork.NoteRepository.Delete(toDelete.Id);
                    _unitOfWork.Save();

                    return HttpStatusCode.OK;
                } else {
                    return HttpStatusCode.Unauthorized;
                }
            });
        }
    }
}