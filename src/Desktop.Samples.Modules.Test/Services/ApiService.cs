using Desktop.Samples.Common;
using Microsoft.Practices.Prism.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Desktop.Samples.Modules.Test.Services
{
    public class ApiService
    {
        private readonly ILoggerFacade _logger;

        public ApiService(
            ILoggerFacade logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger?.Debug($"{GetType().FullName} ... ctor.");
        }

        protected virtual void SetSecurityProtocol(string requestUri)
        {
            if (requestUri.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            }
            else
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)240;
            }
        }

        protected virtual string GetUriWithQueryaParams(
            string requestUri,
            Dictionary<string, object> requestArgs = null)
        {
            if (requestArgs != null)
            {
                if (requestArgs.Any())
                {
                    var args = requestArgs.Select(a => $"{a.Key}={a.Value}");
                    var queryArgs = string.Join("&", args);

                    requestUri = string.Join("?", requestUri, queryArgs);
                }
            }

            return requestUri;
        }

        protected virtual HttpWebRequest WebRequestGenerator(
            string requestUri,
            string requestMethod = "GET",
            string contentType = "application/json;charset=utf-8",
            int timeout = 30000)
        {
            var httpWebRequest = WebRequest.Create(requestUri) as HttpWebRequest;
            //req.Headers.Clear();
            httpWebRequest.Method = requestMethod.ToUpper();
            httpWebRequest.ContentType = contentType;
            httpWebRequest.Timeout = timeout;

            return httpWebRequest;
        }

        protected virtual void SetRequestBody(HttpWebRequest httpWebRequest, string requestBody, Encoding encoding = null)
        {
            if (httpWebRequest == null)
            {
                throw new ArgumentException($"{nameof(httpWebRequest)} is null.");
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (httpWebRequest.Method == "POST")
            {
                if (!string.IsNullOrEmpty(requestBody))
                {
                    var buffer = encoding.GetBytes(requestBody);

                    httpWebRequest.ContentLength = buffer.Length;

                    using (var stream = httpWebRequest.GetRequestStream())
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        protected virtual void SetRequestHeader(HttpWebRequest httpWebRequest, Dictionary<string, string> requestHeaders = null)
        {
            if (httpWebRequest == null)
            {
                throw new ArgumentException($"{nameof(httpWebRequest)} is null.");
            }

            if (requestHeaders != null && requestHeaders.Any())
            {
                foreach (var item in requestHeaders)
                {
                    httpWebRequest.Headers.Add(item.Key, item.Value);
                }
            }
        }

        protected virtual TBody RequestTo<TBody>(
            string requestUri,
            string requestBody = "",
            string requestMethod = "GET",
            Dictionary<string, object> requestArgs = null,
            Dictionary<string, string> requestHeaders = null,
            string contentType = "application/json;charset=utf-8",
            Encoding encoding = null,
            int timeout = 30000) where TBody : class, new()
        {
            var responseContent = string.Empty;

            try
            {
                var requestLog = new
                {
                    requestUri,
                    requestBody,
                    requestMethod,
                    requestArgs,
                    requestHeaders,
                    contentType,
                    encoding,
                    timeout
                }.ToJson();
                _logger.Debug($"{GetType().Name} ... {nameof(RequestTo)} ... request:{requestLog}.");

                SetSecurityProtocol(requestUri);

                requestUri = GetUriWithQueryaParams(requestUri, requestArgs);

                var request = WebRequestGenerator(requestUri, requestMethod, contentType, timeout);

                SetRequestHeader(request, requestHeaders);

                SetRequestBody(request, requestBody, encoding);

                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            responseContent = reader.ReadToEnd();
                            _logger.Debug($"{GetType().Name} ... {nameof(RequestTo)} ... response:{responseContent}.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return string.IsNullOrEmpty(responseContent) ? null : responseContent.ParseTo<TBody>();
        }

        public virtual TBody Post<TBody>(
            string requestUri,
            string requestBody = "",
            Dictionary<string, object> requestArgs = null,
            Dictionary<string, string> requestHeaders = null,
            string contentType = "application/json;charset=utf-8",
            Encoding encoding = null,
            int timeout = 30000) where TBody : class, new()
        {
            return RequestTo<TBody>(requestUri, requestBody, "POST", requestArgs, requestHeaders, contentType, encoding, timeout);
        }

        public virtual TBody Get<TBody>(
            string requestUri,
            Dictionary<string, object> requestArgs = null,
            Dictionary<string, string> requestHeaders = null,
            string contentType = "application/json;charset=utf-8",
            Encoding encoding = null,
            int timeout = 30000) where TBody : class, new()
        {
            return RequestTo<TBody>(requestUri, null, "GET", requestArgs, requestHeaders, contentType, encoding, timeout);
        }
    }

    public static class ApiServiceExtensions
    {
        public static SendCodeResult SendCode(this ApiService api, string clientId = "ih-doctor", string mobile = "13761609127")
        {
            var body = new { clientId, mobile };
            var jsonBody = body.ToJson();

            return api.Post<SendCodeResult>("https://mp-dev.ijia120.com/auth-service/login/send/code", jsonBody);
        }

        public static LoginWithSmsCodeResult LoginWithSmsCode(this ApiService api, string clientId = "ih-doctor", string mobile = "13761609127", string grantType = "sms_code", string code = "0000")
        {
            var body = new { clientId, mobile, grantType, code };
            var jsonBody = body.ToJson();

            return api.Post<LoginWithSmsCodeResult>("https://mp-dev.ijia120.com/auth-service/login", jsonBody);
        }

        public static GetTokenResult GetToken(this ApiService api, string loginCode = "FYXMNZ8W17")
        {
            var body = new { loginCode };
            var jsonBody = body.ToJson();

            return api.Post<GetTokenResult>("https://hospitals-gateway-dev.ijia120.com/hospital-doctor/login/token", jsonBody, requestHeaders: new Dictionary<string, string> { { "Device-Id", "doctor-pc" } });
        }
    }

    public abstract class ApiResultData { }

    public class GetTokenData : ApiResultData
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string scope { get; set; }

        public string company { get; set; }

        public string message { get; set; }

        public string jti { get; set; }
    }

    public class GetTokenResult : ApiResult<GetTokenData> { }

    public class GrantServer
    {
        public string serverId { get; set; }

        public string serverName { get; set; }
    }

    public class LoginWithSmsCodeData : ApiResultData
    {
        public string loginCode { get; set; }

        public GrantServer[] grantServerList { get; set; }
    }

    public class LoginWithSmsCodeResult : ApiResult<LoginWithSmsCodeData> { }

    public class SendCodeResult : ApiResult { }

    public class ApiResult
    {
        public int code { get; set; }

        public string msg { get; set; }

        public string traceId { get; set; }
    }

    public class ApiResult<TData> : ApiResult where TData : ApiResultData
    {
        public TData data { get; set; }
    }

    public static class ApiResultExtensions
    {
        public static bool IsSuccessful(this ApiResult result)
        {
            return result.code == 0;
        }
    }
}
