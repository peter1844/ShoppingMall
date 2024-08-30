using ShoppingMall.Interface;
using ShoppingMall.Models.Enum;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingMall.App_Code
{
    public class Tools : ITools
    {
        private IContextHelper _contextHelper;
        private IConfigurationsHelper _configurationsHelper;

        public Tools(IContextHelper contextHelper, IConfigurationsHelper configurationsHelper)
        {
            _contextHelper = contextHelper;
            _configurationsHelper = configurationsHelper;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        public string AesEncrypt(string encryptData)
        {
            byte[] encrypted;

            // 创建一个 AES 实例
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = _configurationsHelper.GetKey();
                aesAlg.IV = _configurationsHelper.GetIv();

                // 创建加密器
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // 创建内存流来存储加密后的数据
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // 创建加密流
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        // 将数据写入加密流
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(encryptData);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        public string AesDecrypt(string decryptData)
        {
            byte[] cipherText = Convert.FromBase64String(decryptData);
            string plainText = null;

            // 创建一个 AES 实例
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = _configurationsHelper.GetKey();
                aesAlg.IV = _configurationsHelper.GetIv();

                // 创建解密器
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // 创建内存流来存储解密后的数据
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    // 创建加密流
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        // 从加密流中读取数据
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plainText;
        }

        /// <summary>
        /// 隨機產生N位數的字串(Base64)
        /// </summary>
        public string GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            string randomBase64String = Convert.ToBase64String(randomBytes);
            return randomBase64String;
        }

        /// <summary>
        /// 隨機產生N位數的字串
        /// </summary>
        public string GenerateRandomString(int length)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                char randomChar = chars[index];

                sb.Append(randomChar);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 檢查是否有權限執行
        /// </summary>
        /// <returns></returns>
        public bool CheckPermission(int permission)
        {
            if (_contextHelper.GetContext().Session["permissions"] == null) return false;
            string[] allPermissions = _contextHelper.GetContext().Session["permissions"].ToString().Split(',');

            return allPermissions.Contains(permission.ToString());
        }

        /// <summary>
        /// 回傳顯示用的錯誤訊息
        /// </summary>
        public string ReturnExceptionMessage(string message)
        {
            return Enum.IsDefined(typeof(StateCode), message) ? message : StateCode.ExceptionError.ToString();
        }
    }
}