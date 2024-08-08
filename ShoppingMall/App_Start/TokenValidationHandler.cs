using ShoppingMall.Api.Login;
using ShoppingMall.Api.Logout;
using ShoppingMall.Models.Common;
using ShoppingMall.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

public class TokenValidationHandler : DelegatingHandler
{
    private LoginByToken loginByTokenClass;
    private Logout logoutClass;
    private TokenExtend tokenExtendClass;

    public TokenValidationHandler()
    {
        loginByTokenClass = new LoginByToken();
        logoutClass = new Logout();
        tokenExtendClass = new TokenExtend();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        IEnumerable<string> authHeaders;
        HttpContext context = HttpContext.Current;

        // 檢查請求來源是否忽略不需檢查
        if (!IsIgnorePath(request.RequestUri))
        {
            // 檢查Header有沒有帶Token
            if (request.Headers.TryGetValues("token", out authHeaders))
            {
                string token = authHeaders.FirstOrDefault();

                // Token驗證
                if (!loginByTokenClass.IsValidToken(token))
                {
                    logoutClass.LogoutProccess();

                    // 無效Token
                    return request.CreateResponse(HttpStatusCode.OK, new ExceptionData { ErrorMessage = StateCode.InvaildToken.ToString() });
                }
                else
                {
                    tokenExtendClass.ExtendRedisLoginToken(token);
                }
            }
            else
            {
                // header沒有token
                return request.CreateResponse(HttpStatusCode.OK, new ExceptionData { ErrorMessage = StateCode.NoHeaderToken.ToString() });
            }
        }

        // 回傳request繼續往下處理
        return await base.SendAsync(request, cancellationToken);
    }
    private bool IsIgnorePath(Uri requestUri)
    {
        bool path1 = requestUri.AbsolutePath.ToLower().Contains("/api/login");
        bool path2 = requestUri.AbsolutePath.ToLower().Contains("/api/logout");

        return path1 || path2 ? true : false;
    }
}
