using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Services {

    public class CryptoService : ICryptoService {

        public string GenerateSalt() {

            var data = new byte[0x10];
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider()) {

                cryptoServiceProvider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }

        public string EncryptPassword(string password) {

            return EncryptPassword(password, GenerateSalt());
        }

        public string EncryptPassword(string password, string salt) {

            if (string.IsNullOrEmpty(password)) {

                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrEmpty(salt)) {

                throw new ArgumentNullException("salt");
            }

            using (var sha256 = SHA256.Create()) {

                var saltedPassword = 
                    string.Format("{0}{1}", GenerateSalt(), password);
   
                byte[] saltedPasswordAsBytes = 
                    Encoding.UTF8.GetBytes(saltedPassword);

                return Convert.ToBase64String(
                    sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }
    }
}
