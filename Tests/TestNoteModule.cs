using Nancy;
using Nancy.Testing;
using Xunit;
using Moq;
using NoteCloud.DataAccess;
using NoteCloud.Modules;
using NoteCloud.Helpers;
using Newtonsoft.Json;

namespace NoteCloud.Test
{
    public class TestNoteModule
    {
        private Mock<NoteCloudContext> context;
        private Mock<UnitOfWork> mockUOW;
        private Mock<NoteRepository> mockNoteRepo;
        private Mock<NoteGroupRepository> mockNoteGroupRepo;
        private Mock<FollowerRepository> mockFollowerRepo;
        private Mock<CurrentUser> mockCurrentUser;
        private NoteModule noteModule;
        private Browser browser;

        public TestNoteModule() {
            context = new Mock<NoteCloudContext>();
            mockUOW = new Mock<UnitOfWork>(context.Object);
            mockNoteRepo = new Mock<NoteRepository>();
            mockNoteGroupRepo = new Mock<NoteGroupRepository>();
            mockFollowerRepo = new Mock<FollowerRepository>();
            mockCurrentUser = new Mock<CurrentUser>();

            mockUOW.SetupGet(x => x.NoteRepository).Returns(mockNoteRepo.Object);
            mockUOW.SetupGet(x => x.NoteGroupRepository).Returns(mockNoteGroupRepo.Object);
            mockUOW.SetupGet(x => x.FollowerRepository).Returns(mockFollowerRepo.Object);

            noteModule = new NoteModule(mockUOW.Object, mockCurrentUser.Object);
            browser = new Browser(with => with.Module(noteModule));
        }

        [Fact]
        public void CreateNote()
        {
            NoteGroup group = new NoteGroup() {
                Id = 1,
                UserId = 1,
                Title = "Test Group",
                CanFollowerEdit = true
            };
            Note newNote = new Note() {
                Id = 0,
                Text = "Hello World",
                Priority = 0,
                IsComplete = false,
                IsPrivate = false,
                NoteGroupId = group.Id,
            };

            CurrentUser currUser = new CurrentUser() { Email = "heined50@uwosh.edu", UserId = 1 };

            mockCurrentUser.Setup(x => x.GetFromAuthToken(It.IsAny<IUserRepository>(), It.IsAny<string>())).Returns(currUser);
            mockNoteRepo.Setup(x => x.Create(It.IsAny<Note>())).Verifiable();
            mockNoteGroupRepo.Setup(x => x.GetNoteGroup(It.IsAny<int>())).Returns(group);
            mockFollowerRepo.Setup(x => x.IsFollower(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var result = browser.Post("/notes", with => {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.Header("Content-Type", "application/json");
                with.Body(JsonConvert.SerializeObject(newNote));
            });

            mockNoteRepo.Verify(x => x.Create(It.IsAny<Note>()), Times.Once());
            Assert.Equal(HttpStatusCode.OK, result.Result.StatusCode);
        }
    }
}