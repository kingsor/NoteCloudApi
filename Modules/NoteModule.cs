using Nancy;
using NoteCloud.DataAccess;
using Nancy.ModelBinding;

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

            Post("/notes", args => {
                Note note = this.Bind<Note>();

                System.Console.WriteLine(_unitOfWork != null);
                _unitOfWork.NoteRepository.Create(note);
                _unitOfWork.Save();

                return HttpStatusCode.OK;
            });
        }
    }
}