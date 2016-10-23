using Xunit;
using System.Collections.Generic;
using NoteCloud.Helpers;

namespace NoteCloud.Test {
    public class TestJWT {
        [Fact]
        public void TestVerifyWorks() {
            Dictionary<string, object> claims = new Dictionary<string, object>();
            claims.Add("email", "heined50@uwosh.edu");
            
            string token = JWT.Create(claims);
            Assert.True(JWT.Verify(token));

            string badToken = "P" + token;
            Assert.False(JWT.Verify(badToken));
        }
    }
}