namespace OysterVPNLibrary
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    public class HttpClient : WebClient, IHttpClient
    {
        public HttpClient()
        {
            this.Timeout = 0xbb8;
        }

        public string GetData(Uri url)
        {
            if (url != null)
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.Headers.Add("X-PLATFORM", "windows");
                    client.Headers.Add("X-APP-VERSION-WINDOWS", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    client.Headers.Add("Accept", "application/json");
                    try
                    {
                        using (new HttpClient())
                        {
                            this.Timeout = 0xbb8;
                            return client.DownloadString(url);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return string.Empty;
        }

        public string GetData(Uri url, bool istoken)
        {
            if (url != null)
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.Headers.Add("x-platform", "windows");
                    client.Headers.Add("Accept", "application/json");
                    client.Headers.Add("X-APP-VERSION-WINDOWS", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    try
                    {
                        using (new HttpClient())
                        {
                            this.Timeout = 0xbb8;
                            return client.DownloadString(url);
                        }
                    }
                    catch (WebException exception)
                    {
                        if ((exception.Status == WebExceptionStatus.ProtocolError) && (((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.NotFound))
                        {
                        }
                    }
                }
            }
            return string.Empty;
        }

        public string GetData(Uri url, string token)
        {
            if (url != null)
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.Headers.Add("Authorization", "Bearer " + token);
                    client.Headers.Add("x-platform", "windows");
                    client.Headers.Add("Accept", "application/json");
                    client.Headers.Add("X-APP-VERSION-WINDOWS", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    try
                    {
                        using (new HttpClient())
                        {
                            this.Timeout = 0xbb8;
                            return client.DownloadString(url);
                        }
                    }
                    catch (WebException exception)
                    {
                        if (exception.Response != null)
                        {
                            if ((exception.Status == WebExceptionStatus.ProtocolError) && (((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.NotFound))
                            {

                            }
                            if ((((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.Unauthorized))
                            {
                                return "401";
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(uri);
            if (webRequest == null)
            {
                webRequest.Timeout = this.Timeout;
            }
            webRequest.KeepAlive = false;
            webRequest.ProtocolVersion = HttpVersion.Version10;
            return webRequest;
        }

        public string PostAuth(string url, NameValueCollection data, Encoding encoding)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = encoding;
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers.Add("X-PLATFORM", "windows");
                client.Headers.Add("X-APP-VERSION-WINDOWS", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                byte[] bytes = null;
                if (data != null)
                {
                    bytes = encoding.GetBytes(RequestDataToString(data));
                }
                return ((bytes != null) ? encoding.GetString(client.UploadData(url, "POST", bytes)) : string.Empty);
            }
        }

        public string PostData(string url, bool istoken)
        {
            Encoding encoding = Encoding.UTF8;
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = encoding;
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.Headers.Add("x-platform", "windows");
                    client.Headers.Add("X-APP-VERSION-WINDOWS", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    NameValueCollection nv = new NameValueCollection();
                    byte[] bytes = encoding.GetBytes(RequestDataToString(nv));
                    byte[] buffer2 = client.UploadData(url, "POST", bytes);
                    return encoding.GetString(buffer2);
                }
            }
            catch (WebException exception)
            {
                if ((exception.Status == WebExceptionStatus.ProtocolError) && (((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.NotFound))
                {

                }
                if ((((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.Unauthorized))
                {
                    return "401";
                }
            }
            return null;
        }


        public string PostData(string url, NameValueCollection data,string token)
        {

            try
            {
                using (WebClient client = new WebClient())

                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.Headers.Add("x-platform", "windows");
                    client.Headers.Add("X-APP-VERSION-WINDOWS", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    client.Headers.Add("Authorization", "Bearer " + token);
                    client.Headers.Add("Accept", "application/json");
                    byte[] bytes = null;
                    if (data != null)
                    {
                        bytes = client.Encoding.GetBytes(RequestDataToString(data));
                    }
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                    return ((bytes != null) ? client.Encoding.GetString(client.UploadData(url, "POST", bytes)) : string.Empty);
                }
            }
            catch (WebException exception)
            {
                if ((exception.Status == WebExceptionStatus.ProtocolError) && (((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.NotFound))
                {

                }
                if ((((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.Unauthorized))
                {
                    return "401";
                }
            }
            return null;
        }

        public string PostData(string url, NameValueCollection data)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.Headers.Add("X-PLATFORM", "windows");
                    client.Headers.Add("X-APP-VERSION-WINDOWS", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    client.Headers.Add("Accept", "application/json");
                    byte[] bytes = null;
                    if (data != null)
                    {
                        bytes = client.Encoding.GetBytes(RequestDataToString(data));
                    }
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                    return ((bytes != null) ? client.Encoding.GetString(client.UploadData(url, "POST", bytes)) : string.Empty);
                }
            }
            catch (WebException exception)
            {
                if ((exception.Status == WebExceptionStatus.ProtocolError) && (((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.NotFound))
                {

                }
                if ((((HttpWebResponse)exception.Response).StatusCode == HttpStatusCode.Unauthorized))
                {
                    return "401";
                }
            }
            return null;
        }

        public string PostData(string url, NameValueCollection data, bool istoken)
        {
            Encoding encoding = Encoding.UTF8;
            using (WebClient client = new WebClient())
            {
                client.Encoding = encoding;
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers.Add("x-platform", "windows");
                client.Headers.Add("X-APP-VERSION-WINDOWS", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                byte[] bytes = null;
                if (data != null)
                {
                    bytes = encoding.GetBytes(RequestDataToString(data));
                }
                return ((bytes != null) ? encoding.GetString(client.UploadData(url, "POST", bytes)) : string.Empty);
            }
        }

        public string PostRegister(string url, NameValueCollection data, Encoding encoding)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = encoding;
                client.Headers[HttpRequestHeader.Accept] = "application/json";
                client.Headers.Add("X-PLATFORM", "windows");
                client.Headers.Add("X-APP-VERSION-WINDOWS", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                byte[] bytes = null;
                if (data != null)
                {
                    bytes = encoding.GetBytes(RequestDataToString(data));
                }
                return ((bytes != null) ? encoding.GetString(client.UploadData(url, "POST", bytes)) : string.Empty);
            }
        }

        private static string RequestDataToString(NameValueCollection nv)
        {
            StringBuilder builder = new StringBuilder();
            bool flag = true;
            foreach (string str in nv.AllKeys)
            {
                if (!flag)
                {
                    builder.Append("&");
                }
                string str2 = nv[str];
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = Uri.EscapeDataString(str2);
                }
                builder.AppendFormat("{0}={1}", str, str2);
                flag = false;
            }
            return builder.ToString();
        }

        public int Timeout { get; set; }



        [DebuggerHidden]
        private void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}

