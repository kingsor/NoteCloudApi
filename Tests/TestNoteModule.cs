using Nancy;
using Nancy.Testing;
using Xunit;
using Moq;
using NoteCloud.DataAccess;
using NoteCloud.Modules;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NoteCloud.Test
{
    public class TestNoteModule
    {
        [Fact]
        public void CreateNote()
        {
            NoteGroup group = new NoteGroup() {
                Id = 1,
                UserId = 1,
                Title = "Test Group",
            };
            Note newNote = new Note() {
                Id = 0,
                Text = "Hello World",
                Priority = 0,
                IsComplete = false,
                IsPrivate = false,
                NoteGroupId = group.Id,
            };

            Mock<NoteCloudContext> context = new Mock<NoteCloudContext>();
            Mock<UnitOfWork> mockUOW = new Mock<UnitOfWork>(context.Object);
            Mock<NoteRepository> mockNoteRepo = new Mock<NoteRepository>();

            mockNoteRepo.Setup(x => x.Create(It.IsAny<Note>())).Verifiable();
            mockUOW.SetupGet(x => x.NoteRepository).Returns(mockNoteRepo.Object);

            NoteModule noteModule = new NoteModule(mockUOW.Object);
            var browser = new Browser(with => with.Module(noteModule));

            var result = browser.Post("/notes", with => {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.Body(JsonConvert.SerializeObject(newNote));
            });

            mockNoteRepo.Verify(x => x.Create(It.IsAny<Note>()), Times.Once());
            Assert.Equal(HttpStatusCode.OK, result.Result.StatusCode);
        }
    }
}