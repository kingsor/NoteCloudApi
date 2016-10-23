using Nancy;
using Nancy.ModelBinding;
using NoteCloud.DataAccess;
using NoteCloud.Helpers;
using System;

namespace NoteCloud.Modules
{
    public class UserModule : NancyModule
    {
        private UnitOfWork _unitOfWork;
        public UserModule(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
            Post("/users", args => {
                User user = this.Bind<User>();

                //Do hashing here
                string password = user.PasswordHash;
                Tuple<string, string> result = PasswordHash.hash(password);
                user.PasswordHash = result.Item1;
                user.Salt = result.Item2;

                _unitOfWork.UserRepository.Create(user);
                _unitOfWork.Save();

                return HttpStatusCode.OK;
            });

            Post("/users/login", args => {
                User user = this.Bind<User>();
                
                

                return HttpStatusCode.OK;
            });
        }
    }
}