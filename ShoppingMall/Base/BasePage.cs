using ShoppingMall.Helper;
using ShoppingMall.Interface;
using System;
using System.IO;
using System.Security.Cryptography;

public class BasePage : System.Web.UI.Page
{
    private IConfigurationsHelper _configurationsHelper;

    public BasePage(IConfigurationsHelper configurationsHelper = null)
    {
        _configurationsHelper = configurationsHelper ?? new ConfigurationsHelper();
    }

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
        string typeVersion = type == "js" ? _configurationsHelper.GetJsVersion() : type == "css" ? _configurationsHelper.GetCssVersion() : "";
        string finalUrl = $"/{type}/{typeVersion}/{url}";

        string filePath = Server.MapPath(finalUrl);

        if (File.Exists(filePath))
        {
#if DEBUG
            using (MD5 md5 = MD5.Create())
            {
                string fileHash = BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(filePath))).Replace("-", "").ToLower();
                return $"{finalUrl}?v={fileHash}";
            }
#else
                return finalUrl;
#endif
        }

        return url;
    }
}
