using OnlineChat.Models.Users;
using System;
using System.IO;
using System.Security.Cryptography;

namespace OnlineChat.Models
{
	public class AESCrypt
	{
		private AesManaged aes = new AesManaged();
		private byte[] IV;
		private byte[] Key;
		public AESCrypt()
		{
			IV = aes.IV;
			Key = aes.Key;
		}
		public string Encrypt(string raw)
		{
			byte[] encrypted;
			using (AesManaged aes = new AesManaged())
			{
				ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter sw = new StreamWriter(cs))
							sw.Write(raw);
						encrypted = ms.ToArray();
					}
				}
			}
			return Convert.ToBase64String(encrypted);
		}
		public string Decrypt(string encryptedText)
		{
			string decryptedText = null;
			byte[] byteText = Convert.FromBase64String(encryptedText);
			using (AesManaged aes = new AesManaged())
			{ 
				ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);    
				using (MemoryStream ms = new MemoryStream(byteText))
				{    
					using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader reader = new StreamReader(cs))
							decryptedText = reader.ReadToEnd();
					}
				}
			}
			return decryptedText;
		}
        public User EncryptUser(User user)
        {
            User encryptedUser = new User();
            encryptedUser.Id = user.Id;
            encryptedUser.FIO = Encrypt(user.FIO);

            encryptedUser.BirthDate = user.BirthDate;
            encryptedUser.Email = user.Email;
            encryptedUser.Password = user.Password;
            
            encryptedUser.Status = Encrypt(user.Status);
            encryptedUser.IV = IV;
            encryptedUser.Key = Key;
            return encryptedUser;
        }
        public User DecryptUser(User user)
        {
            User decryptedUser = new User();
            IV = user.IV;
            Key = user.Key;
            decryptedUser.Id = user.Id;
            decryptedUser.FIO = Decrypt(user.FIO);
       
            decryptedUser.BirthDate = user.BirthDate;
      
            decryptedUser.Email = user.Email;
            decryptedUser.Password = user.Password;

            decryptedUser.Status = Decrypt(user.Status);
            decryptedUser.IV = IV;
            decryptedUser.Key = Key;
            return decryptedUser;
        }
    }
}
