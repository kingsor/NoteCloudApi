using Nancy;
using Nancy.ModelBinding;
using NoteCloud.DataAccess;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NoteCloud.Modules
{
    public class UserModule : NancyModule
    {
        private UnitOfWork _unitOfWork;
        public UserModule(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
            Get("/users", _ => {
                return this._unitOfWork.UserRepository.GetAllUsers();
            });

            Post("/users", args => {
                User user = this.Bind<User>();
                _unitOfWork.UserRepository.Create(user);
                _unitOfWork.Save();

                return HttpStatusCode.OK;
            });
        }
    }
}