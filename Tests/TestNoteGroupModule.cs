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
    public class TestNoteGroupModule
    {
        private Mock<IUnitOfWork> mockUOW;
        private Mock<INoteRepository> mockNoteRepo;
        private Mock<INoteGroupRepository> mockNoteGroupRepo;
        private Mock<IFollowerRepository> mockFollowerRepo;
        private Mock<CurrentUser> mockCurrentUser;
        private NoteGroupModule noteModule;
        private Browser browser;

        public TestNoteGroupModule() {
            mockUOW = new Mock<IUnitOfWork>();
            mockNoteRepo = new Mock<INoteRepository>();
            mockNoteGroupRepo = new Mock<INoteGroupRepository>();
            mockFollowerRepo = new Mock<IFollowerRepository>();
            mockCurrentUser = new Mock<CurrentUser>();

            mockUOW.SetupGet(x => x.NoteRepository).Returns(mockNoteRepo.Object);
            mockUOW.SetupGet(x => x.NoteGroupRepository).Returns(mockNoteGroupRepo.Object);
            mockUOW.SetupGet(x => x.FollowerRepository).Returns(mockFollowerRepo.Object);

            noteModule = new NoteGroupModule(mockUOW.Object, mockCurrentUser.Object);
            browser = new Browser(with => with.Module(noteModule));
        }

        [Fact]
        public void CreateNoteGroup()
        {
            NoteGroup group = new NoteGroup() {
                Id = 1,
                UserId = 1,
                Title = "Test Group",
                CanFollowerEdit = false
            };

            CurrentUser currUser = new CurrentUser() { Email = "heined50@uwosh.edu", UserId = 1 };
            mockCurrentUser.Setup(x => x.GetFromAuthToken(It.IsAny<IUserRepository>(), It.IsAny<string>())).Returns(currUser);
            mockNoteGroupRepo.Setup(x => x.Create(It.IsAny<NoteGroup>())).Verifiable();

            var result = browser.Post("/notegroups", with => {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.Header("Content-Type", "application/json");
                with.Body(JsonConvert.SerializeObject(group));
            });

            mockNoteGroupRepo.Verify(x => x.Create(It.IsAny<NoteGroup>()), Times.Once());
            Assert.Equal(HttpStatusCode.OK, result.Result.StatusCode);
        }
    }
}