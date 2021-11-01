using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Service.Common
{
    public class HTTPHelper
    {
        private static IHttpClientFactory _clientFactory { get; set; }

        public HTTPHelper(IHttpClientFactory client)
        {
            _clientFactory = client;
        }

        public static HTTPResult Post(string url, string body, string contentType = "application/json")
        {
            var client = _clientFactory.CreateClient(new Uri(url).Host);

            var response = client.PostAsync(url, new StringContent(body, Encoding.UTF8, contentType));
            var success = response.Wait(150000);
            if (!success)
                return new HTTPResult((int)response.Result.StatusCode, response.Result.ReasonPhrase);

            return new HTTPResult(response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public static HTTPResult Post(string url, Dictionary<string, string> heads, string body, string contentType = "application/json")
        {
            var client = _clientFactory.CreateClient(new Uri(url).Host);

            foreach (var head in heads)
            {
                if (client.DefaultRequestHeaders.Contains(head.Key))
                    client.DefaultRequestHeaders.Remove(head.Key);
                client.DefaultRequestHeaders.Add(head.Key, head.Value);
            }

            var response = client.PostAsync(url, new StringContent(body, Encoding.UTF8, contentType));
            var success = response.Wait(150000);
            if (!success)
                return new HTTPResult((int)response.Result.StatusCode, response.Result.ReasonPhrase);

            return new HTTPResult(response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public static HTTPResult Get(string url)
        {
            var client = _clientFactory.CreateClient();

            var response = client.GetAsync(url);
            var success = response.Wait(150000);
            if (!success)
                return new HTTPResult((int)response.Result.StatusCode, response.Result.ReasonPhrase);

            return new HTTPResult(response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }

        public static HTTPResult Get(string url, Dictionary<string, string> heads)
        {
            var client = _clientFactory.CreateClient();

            foreach (var head in heads)
            {
                if (client.DefaultRequestHeaders.Contains(head.Key))
                    client.DefaultRequestHeaders.Remove(head.Key);
                client.DefaultRequestHeaders.Add(head.Key, head.Value);
            }

            var response = client.GetAsync(url);
            var success = response.Wait(150000);
            if (!success)
                return new HTTPResult((int)response.Result.StatusCode, response.Result.ReasonPhrase);

            return new HTTPResult(response.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }

    public class HTTPResult
    {
        private bool _success;
        public HTTPResult(string content)
        {
            StatusCode = StatusCodes.Status200OK;
            Content = content;
            _success = true;
        }

        public HTTPResult(int statusCode, string errorMsg)
        {
            StatusCode = statusCode;
            ErrorMsg = errorMsg;
            _success = false;
        }

        public bool IsSuccess => this._success;

        public string GetValue()
        {
            if (Content == null)
                return "";
            else
                return Content;
        }

        public int StatusCode { get; set; }
        public string Content { get; }
        public string ErrorMsg { get; set; }
    }
}
