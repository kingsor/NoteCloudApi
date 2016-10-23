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
    public class TestUserModule
    {
        [Fact]
        public void RootRouteResult()
        {
            List<User> expected = new List<User>();
            expected.Add(new User() { Id = 1, Email = "heined50@uwosh.edu", PasswordHash = "heined50"});
            Mock<UnitOfWork> mockUOW = new Mock<UnitOfWork>();
            Mock<UserRepository> mockUserRepo = new Mock<UserRepository>();

            mockUserRepo.Setup(x => x.GetAllUsers()).Returns(expected);
            mockUOW.SetupGet(x => x.UserRepository).Returns(mockUserRepo.Object);
            UserModule userModule = new UserModule(mockUOW.Object);
            var browser = new Browser(with => with.Module(userModule));

            var result = browser.Get("/users", with => {
                with.HttpRequest();
                with.Header("Accept", "application/json");
            });

            var resultObject = JsonConvert.DeserializeObject<List<User>>(result.Result.Body.AsString());

            //We want to disallow getting all users
            Assert.Equal(HttpStatusCode.MethodNotAllowed, result.Result.StatusCode);
        }
    }
}