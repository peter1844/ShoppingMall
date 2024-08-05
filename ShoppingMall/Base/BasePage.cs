using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class BasePage : System.Web.UI.Page
{
    private readonly string JS_VERSION = "1.0.0";
    private readonly string CSS_VERSION = "1.0.0";

    protected void Page_Init(object sender, EventArgs e)
    {
        Session["lang"] = (Session["lang"] ?? "tw").ToString();
    }

    protected string GetVersionUrl(string url, string type)
    {
        string typeVersion = type == "js" ? this.JS_VERSION : type == "css" ? this.CSS_VERSION : "";
        string finalUrl = $"/{type}/{typeVersion}/{url}";

        string filePath = Server.MapPath(finalUrl);

        if (File.Exists(filePath))
        {
            using (MD5 md5 = MD5.Create())
            {
                string fileHash = BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(filePath))).Replace("-", "").ToLower();
                return $"{finalUrl}?v={fileHash}";
            }
        }

        return url;
    }
}
