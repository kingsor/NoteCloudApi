using Nancy;
using Nancy.ModelBinding;
using NoteCloud.DataAccess;
using NoteCloud.Helpers;
using System;
using System.Collections.Generic;

namespace NoteCloud.Modules
{
    public class UserModule : NancyModule
    {
        private UnitOfWork _unitOfWork;
        public UserModule(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
            Post("/users", args => {
                User user = this.Bind();

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
                
                User fromDb = _unitOfWork.UserRepository.GetUser(user.Email);
                if(fromDb != null && PasswordHash.verify(user.PasswordHash, fromDb.PasswordHash, fromDb.Salt)) {
                    Dictionary<string, object> claims = new Dictionary<string, object>();
                    claims.Add("email", user.Email);
                    claims.Add("userId", fromDb.Id);

                    return JWT.Create(claims);
                } else {
                    return HttpStatusCode.Unauthorized;
                }
            });
        }
    }
}