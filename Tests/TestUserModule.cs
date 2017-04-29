using Nancy;
using Nancy.Testing;
using Xunit;
using Moq;
using NoteCloud.DataAccess;
using NoteCloud.Modules;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using NoteCloud.Helpers;

namespace NoteCloud.Test
{
    public class TestUserModule
    {
        private Mock<UnitOfWork> mockUOW;
        private Mock<IUserRepository> mockUserRepo;
        private Mock<IOptions<Secrets>> mockOptions;
        private UserModule userModule;
        private Browser browser;

        public TestUserModule() {
            mockUOW = new Mock<UnitOfWork>();
            mockUserRepo = new Mock<IUserRepository>();
            mockOptions = new Mock<IOptions<Secrets>>();

            mockOptions.Setup(x => x.Value).Returns(new Secrets() { SecretKey = "Hello World" });
            mockUOW.SetupGet(x => x.UserRepository).Returns(mockUserRepo.Object);
            userModule = new UserModule(mockUOW.Object, mockOptions.Object);
            browser = new Browser(with => with.Module(userModule));
        }

        [Fact]
        public void RootRouteResult()
        {
            List<User> expected = new List<User>();
            expected.Add(new User() { Id = 1, Email = "heined50@uwosh.edu", PasswordHash = "heined50"});
            expected.Add(new User() { Id = 2, Email = "kaufmk43@uwosh.edu", PasswordHash = "kaufmk43"});

            mockUserRepo.Setup(x => x.GetAllUsers()).Returns(expected);

            var result = browser.Get("/users", with => {
                with.HttpRequest();
                with.Header("Accept", "application/json");
            });

            var resultObject = JsonConvert.DeserializeObject<List<DTO.User>>(result.Result.Body.AsString());

            //We want to disallow getting all users
            Assert.Equal(HttpStatusCode.OK, result.Result.StatusCode);
            Assert.Equal("heined50@uwosh.edu", resultObject[0].Email);
            Assert.Equal(1, resultObject[0].UserId);

            Assert.Equal("kaufmk43@uwosh.edu", resultObject[1].Email);
            Assert.Equal(2, resultObject[1].UserId);
        }
    }
}