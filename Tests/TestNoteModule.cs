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
        public void RootRouteResult()
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
            Mock<UnitOfWork> mockUOW = new Mock<UnitOfWork>();
            Mock<NoteRepository> mockNoteRepo = new Mock<NoteRepository>();

            mockUOW.SetupGet(x => x.NoteRepository).Returns(mockNoteRepo.Object);
            NoteModule noteModule = new NoteModule(mockUOW.Object);
            var browser = new Browser(with => with.Module(noteModule));

            var result = browser.Get("/notes", with => {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.Body(JsonConvert.SerializeObject(newNote));
            });

            mockNoteRepo.Verify(x => x.Create(It.IsAny<Note>()), Times.Exactly(1));
            Assert.Equal(HttpStatusCode.OK, result.Result.StatusCode);
        }
    }
}