using Xunit;
using System.Collections.Generic;
using NoteCloud.Helpers;
using Microsoft.Extensions.Options;
using Moq;

namespace NoteCloud.Test {
    public class TestJWT {
        [Fact]
        public void TestVerifyWorks() {
            Secrets settings = new Secrets() { SecretKey = "Hello, World" };

            Dictionary<string, object> claims = new Dictionary<string, object>();
            claims.Add("email", "heined50@uwosh.edu");
            
            string token = JWT.Create(settings, claims);
            Assert.True(JWT.Verify(settings, token));

            string badToken = "P" + token;
            Assert.False(JWT.Verify(settings, badToken));
        }
    }
}