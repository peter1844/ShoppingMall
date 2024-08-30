using ShoppingMall.Interface;
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
    private IToken _token;
    private ILogout _logout;

    public TokenValidationHandler(IToken token, ILogout logout)
    {
        _token = token;
        _logout = logout;
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
                if (!_token.IsValidToken(token))
                {
                    _logout.LogoutProccess();

                    // 無效Token
                    return request.CreateResponse(HttpStatusCode.OK, new ExceptionData { ErrorMessage = StateCode.InvaildToken.ToString() });
                }
                else
                {
                    _token.ExtendRedisLoginToken(token);
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
