using ShoppingMall.Api.Login;
using ShoppingMall.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class TokenValidationHandler : DelegatingHandler
{
    private Base baseClass;
    private LoginByToken loginByToken;

    public TokenValidationHandler()
    {
        baseClass = new Base();
        loginByToken = new LoginByToken();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        IEnumerable<string> authHeaders;

        if (!IsIgnorePath(request.RequestUri))
        {
            if (request.Headers.TryGetValues("token", out authHeaders))
            {
                string token = authHeaders.FirstOrDefault();

                // Token驗證
                if (!loginByToken.IsValidToken(token))
                {
                    // 無效Token
                    return request.CreateResponse(HttpStatusCode.Unauthorized, "token無效或過期，請重新登入");
                }
            }
            else
            {
                // header沒有token
                return request.CreateResponse(HttpStatusCode.Unauthorized, "未帶token");
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
