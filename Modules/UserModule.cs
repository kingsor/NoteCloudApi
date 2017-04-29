using Nancy;
using Nancy.ModelBinding;
using NoteCloud.DataAccess;
using NoteCloud.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace NoteCloud.Modules
{
    public class UserModule : NancyModule
    {
        private IUnitOfWork _unitOfWork;
        private Secrets _secrets;
        public UserModule(IUnitOfWork unitOfWork, IOptions<Secrets> secrets)
        {
            _unitOfWork = unitOfWork;
            _secrets = secrets.Value;
            
            Get("/public/users", _ => {
                return _unitOfWork.UserRepository.GetAllUsers().Select(x => new DTO.User() { Email = x.Email, UserId = x.Id });
            });

            Post("/public/users", args => {
                DTO.LoginCredentials user = this.Bind();

                if(_unitOfWork.UserRepository.GetUser(user.Email) == null) {
                    User userToAdd = new User();
                    //Do hashing here
                    string password = user.Password;
                    Tuple<string, string> result = PasswordHash.hash(password);
                    userToAdd.PasswordHash = result.Item1;
                    userToAdd.Salt = result.Item2;
                    userToAdd.Email = user.Email;

                    _unitOfWork.UserRepository.Create(userToAdd);
                    _unitOfWork.Save();

                    return HttpStatusCode.OK;
                } else {
                    return HttpStatusCode.Unauthorized;
                }
            });

            Post("/public/users/login", args => {
                DTO.LoginCredentials user = this.Bind<DTO.LoginCredentials>();
                
                User fromDb = _unitOfWork.UserRepository.GetUser(user.Email);
                if(fromDb != null && PasswordHash.verify(user.Password, fromDb.PasswordHash, fromDb.Salt)) {
                    Dictionary<string, object> claims = new Dictionary<string, object>();
                    claims.Add("email", fromDb.Email);
                    claims.Add("userId", fromDb.Id);

                    return JWT.Create(_secrets, claims);
                } else {
                    return HttpStatusCode.Unauthorized;
                }
            });
        }
    }
}