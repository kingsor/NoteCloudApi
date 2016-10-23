using NoteCloud.DataAccess;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NoteCloud.Helpers {
    public class CurrentUser : ICurrentUser {
        public string Email { get; set; }
        public int UserId { get; set; }

        public CurrentUser GetFromAuthToken(IUserRepository userRepo, string token) {
            string[] items = token.Split('.');
            string claimsDecoded = JWT.Base64Decode(items[1]);

            Dictionary<string, object> claims = JsonConvert.DeserializeObject<Dictionary<string, object>>(claimsDecoded);
            User user = userRepo.GetUser((string) claims["email"]);
            return new CurrentUser() { Email = user.Email, UserId = user.Id };
        }
    }

    public interface ICurrentUser {
        CurrentUser GetFromAuthToken(IUserRepository userRepo, string token);
    }
}