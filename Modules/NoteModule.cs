using Nancy;
using Nancy.ModelBinding;
using NoteCloud.DataAccess;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NoteCloud.Modules
{
    public class NoteModule : NancyModule
    {
        private UnitOfWork _unitOfWork;
        public NoteModule(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
            Get("/notes", _ => {
                return this._unitOfWork.NoteRepository.GetAllNotes();
            });
        }
    }
}