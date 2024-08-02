using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class BasePage : System.Web.UI.Page
{
    protected string GetVersionUrl(string url)
    {
        string filePath = Server.MapPath(url);

        if (File.Exists(filePath))
        {
            using (MD5 md5 = MD5.Create())
            {
                //string fileHash = BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(filePath))).Replace("-", "").ToLower();
                string fileHash = ConfigurationManager.AppSettings["Version"];
                return $"{url}?v={fileHash}";
            }
        }
        return url;
    }
}
