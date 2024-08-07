using ShoppingMall.Helper;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ShoppingMall.App_Code
{
    /// <summary>
    /// 角色
    /// </summary>
    public enum Roles
    {
        /// <summary>
        /// 超級管理員
        /// </summary>
        SuperAdmin = 1,
        /// <summary>
        /// 一般管理員
        /// </summary>
        Admin = 2,
        /// <summary>
        /// 客服部
        /// </summary>
        CustomerService = 3,
        /// <summary>
        /// 產品部
        /// </summary>
        Commodity = 4
    }

    /// <summary>
    /// 權限
    /// </summary>
    public enum Permissions
    {
        /// <summary>
        /// 會員管理
        /// </summary>
        Member = 1,
        /// <summary>
        /// 會員管理-編輯
        /// </summary>
        MemberUpdate = 2,
        /// <summary>
        /// 商品管理
        /// </summary>
        Commodity = 3,
        /// <summary>
        /// 商品管理-新增
        /// </summary>
        CommodityInsert = 4,
        /// <summary>
        /// 商品管理-編輯
        /// </summary>
        CommodityUpdate = 5,
        /// <summary>
        /// 訂單管理
        /// </summary>
        Order = 6,
        /// <summary>
        /// 訂單管理-模擬下單
        /// </summary>
        OrderInsert = 7,
        /// <summary>
        /// 訂單管理-編輯
        /// </summary>
        OrderUpdate = 8,
        /// <summary>
        /// 管理者帳號管理
        /// </summary>
        Admin = 10,
        /// <summary>
        /// 管理者帳號管理-新增
        /// </summary>
        AdminInsert = 11,
        /// <summary>
        /// 管理者帳號管理-編輯
        /// </summary>
        AdminUpdate = 12,
        /// <summary>
        /// 管理者帳號管理-刪除
        /// </summary>
        AdminDelete = 13
    }

    /// <summary>
    /// 狀態碼
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 傳入參數驗證失敗
        /// </summary>
        InvaildInputData = 100,
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// DB操作失敗
        /// </summary>
        DbError = 300,
        /// <summary>
        /// 無效Token
        /// </summary>
        InvaildToken = 401,
        /// <summary>
        /// Header未夾帶Token
        /// </summary>
        NoHeaderToken = 4011,
        /// <summary>
        /// 登入失敗
        /// </summary>
        InvaildLogin = 4012,
        /// <summary>
        /// 沒有權限
        /// </summary>
        NoPermission = 4013,
        /// <summary>
        /// 庫存量不足
        /// </summary>
        StockError = 700
    }

    /// <summary>
    /// 付款方式
    /// </summary>
    public enum PayTypeCode
    {
        /// <summary>
        /// 線上刷卡
        /// </summary>
        Credit = 1,
        /// <summary>
        /// 匯款
        /// </summary>
        Remittance = 2,
        /// <summary>
        /// LinePay
        /// </summary>
        LinePay = 3,
        /// <summary>
        /// 貨到付款
        /// </summary>
        CashOnDelivery = 4
    }

    /// <summary>
    /// 付款狀態
    /// </summary>
    public enum PayStateCode
    {
        /// <summary>
        /// 未付款
        /// </summary>
        UnPaid = 0,
        /// <summary>
        /// 已付款
        /// </summary>
        AlreadyPaid = 1
    }

    /// <summary>
    /// 配送方式
    /// </summary>
    public enum DeliveryTypeCode
    {
        /// <summary>
        /// 陸運
        /// </summary>
        LandTransportation = 1,
        /// <summary>
        /// 海運
        /// </summary>
        Shipping = 2,
        /// <summary>
        /// 空運
        /// </summary>
        AirTransportation = 3
    }

    /// <summary>
    /// 配送狀態
    /// </summary>
    public enum DeliveryStateCode
    {
        /// <summary>
        /// 未出貨
        /// </summary>
        NotShipped = 0,
        /// <summary>
        /// 已出貨
        /// </summary>
        Shipped = 1,
        /// <summary>
        /// 退貨
        /// </summary>
        Return = 2,
    }

    public static class Tools
    {
        /// <summary>
        /// AES加密
        /// </summary>
        public static string AesEncrypt(string encryptData)
        {
            byte[] encrypted;

            // 创建一个 AES 实例
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = ConfigurationsHelper.KEY;
                aesAlg.IV = ConfigurationsHelper.IV;

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
        public static string AesDecrypt(string decryptData)
        {
            byte[] cipherText = Convert.FromBase64String(decryptData);
            string plainText = null;

            // 创建一个 AES 实例
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = ConfigurationsHelper.KEY;
                aesAlg.IV = ConfigurationsHelper.IV;

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
        public static string GenerateRandomBytes(int length)
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
        public static string GenerateRandomString(int length)
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
        public static bool CheckPermission(int permission)
        {
            HttpContext context = HttpContext.Current;

            if (context.Session["permissions"] == null) return false;
            string[] allPermissions = context.Session["permissions"].ToString().Split(',');

            return allPermissions.Contains(permission.ToString());
        }
    }
}