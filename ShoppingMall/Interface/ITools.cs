using ShoppingMall.Models.Commodity;
using ShoppingMall.Models.Login;
using System.Collections.Generic;
using System.Web;

namespace ShoppingMall.Interface
{
    public interface ITools
    {
        /// <summary>
        /// AES加密
        /// </summary>
        string AesEncrypt(string encryptData);

        /// <summary>
        /// AES解密
        /// </summary>
        string AesDecrypt(string decryptData);

        /// <summary>
        /// 隨機產生N位數的字串(Base64)
        /// </summary>
        string GenerateRandomBytes(int length);

        /// <summary>
        /// 隨機產生N位數的字串
        /// </summary>
        string GenerateRandomString(int length);

        /// <summary>
        /// 檢查是否有權限執行
        /// </summary>
        /// <returns></returns>
        bool CheckPermission(int permission);

        /// <summary>
        /// 回傳顯示用的錯誤訊息
        /// </summary>
        string ReturnExceptionMessage(string message);
    }
}
