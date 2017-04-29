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
    public class TestFollowerModule
    {
        private Mock<IUnitOfWork> mockUOW;
        private Mock<INoteRepository> mockNoteRepo;
        private Mock<INoteGroupRepository> mockNoteGroupRepo;
        private Mock<IFollowerRepository> mockFollowerRepo;
        private Mock<CurrentUser> mockCurrentUser;
        private FollowerModule followerModule;
        private Browser browser;

        public TestFollowerModule() {
            mockUOW = new Mock<IUnitOfWork>();
            mockNoteRepo = new Mock<INoteRepository>();
            mockNoteGroupRepo = new Mock<INoteGroupRepository>();
            mockFollowerRepo = new Mock<IFollowerRepository>();
            mockCurrentUser = new Mock<CurrentUser>();

            mockUOW.SetupGet(x => x.NoteRepository).Returns(mockNoteRepo.Object);
            mockUOW.SetupGet(x => x.NoteGroupRepository).Returns(mockNoteGroupRepo.Object);
            mockUOW.SetupGet(x => x.FollowerRepository).Returns(mockFollowerRepo.Object);

            followerModule = new FollowerModule(mockUOW.Object, mockCurrentUser.Object);
            browser = new Browser(with => with.Module(followerModule));
        }

        [Fact]
        public void CreateFollower()
        {
            Follower follower = new Follower() {
                Id = 1,
                UserId = 1,
                FolloweeId = 1
            };

            CurrentUser currUser = new CurrentUser() { Email = "heined50@uwosh.edu", UserId = 1 };
            mockCurrentUser.Setup(x => x.GetFromAuthToken(It.IsAny<IUserRepository>(), It.IsAny<string>())).Returns(currUser);
            mockFollowerRepo.Setup(x => x.Create(It.IsAny<Follower>())).Verifiable();

            var result = browser.Post("/followers/" + follower.UserId, with => {
                with.HttpRequest();
                with.Header("Accept", "application/json");
                with.Header("Content-Type", "application/json");
                with.Body(JsonConvert.SerializeObject(follower));
            });

            mockFollowerRepo.Verify(x => x.Create(It.IsAny<Follower>()), Times.Once());
            Assert.Equal(HttpStatusCode.OK, result.Result.StatusCode);
        }
    }
}