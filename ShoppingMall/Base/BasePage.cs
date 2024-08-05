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

    /// <summary>
    /// 取得對應的語言包
    /// </summary>
    protected string GetLanguagePackage() 
    {
        string languageUrl;

        switch (Session["lang"])
        {
            case "tw":
                languageUrl = this.GetVersionUrl("lang/Tw.js", "js");
                break;
            case "en":
                languageUrl = this.GetVersionUrl("lang/En.js", "js");
                break;
            default:
                languageUrl = this.GetVersionUrl("lang/Tw.js", "js");
                break;
        }

        return languageUrl;
    }

    /// <summary>
    /// 取得對應版本的js或css
    /// </summary>
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
