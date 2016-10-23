using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace NoteCloud.Helpers {
    public class JWT {
        private JWT() { /* SEALED CONSTRUCTOR */ }
        
        public static string Create(Secrets secrets, Dictionary<string, object> claims) {
            //JsonConvert.Serialize
            string claimsString = JsonConvert.SerializeObject(claims);
            string algorithmInfo = "{\"alg\": \"HS256\",\"typ\": \"JWT\"}";
            string secret = secrets.SecretKey;
            string result = "";

            result += Base64Encode(algorithmInfo) + "." + Base64Encode(claimsString);
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            using(var hasher = new HMACSHA256(secretBytes)) {
                byte[] raw = hasher.ComputeHash(Encoding.UTF8.GetBytes(result));
                result += "." + Convert.ToBase64String(raw);
            }

            return result;
        }

        public static bool Verify(Secrets secrets, string jwt) {
            string[] items = jwt.Split('.');
            string receivedHeaderAndPayload = items[0] + "." + items[1];
            string actual = items[2];

            string expected = "";
            string secret = secrets.SecretKey;
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            using(var hasher = new HMACSHA256(secretBytes)) {
                byte[] raw = hasher.ComputeHash(Encoding.UTF8.GetBytes(receivedHeaderAndPayload));
                expected = Convert.ToBase64String(raw);
            }

            return (expected == actual);
        }

        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}