using Nancy;
using NoteCloud.DataAccess;
using Nancy.ModelBinding;
using NoteCloud.Helpers;
using System.Linq;

namespace NoteCloud.Modules
{
    public class FollowerModule : NancyModule
    {
        private IUnitOfWork _unitOfWork;
        private CurrentUser _currentUser;
        public FollowerModule(IUnitOfWork unitOfWork, CurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;

            Get("/followers/{userId}", args => {
                return this._unitOfWork.FollowerRepository.GetAllFollowers(args.userId);
            });

            Post("/followers/{userId}", args => {
                _currentUser = _currentUser.GetFromAuthToken(_unitOfWork.UserRepository, Request.Headers["Authorize"].FirstOrDefault());
                Follower follower = new Follower() { UserId = args.userId, FolloweeId = _currentUser.UserId };

                this._unitOfWork.FollowerRepository.Create(follower);
                this._unitOfWork.Save();

                return HttpStatusCode.OK;
            });

            Delete("/followers/{userId}", args => {
                _currentUser = _currentUser.GetFromAuthToken(_unitOfWork.UserRepository, Request.Headers["Authorize"].FirstOrDefault());

                this._unitOfWork.FollowerRepository.Delete(args.userId, _currentUser.UserId);
                return HttpStatusCode.OK;
            });
        }
    }
}