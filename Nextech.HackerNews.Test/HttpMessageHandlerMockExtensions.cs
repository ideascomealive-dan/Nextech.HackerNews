using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Nextech.HackerNews.Test
{
    public static class HttpMessageHandlerMockExtensions
    {
        public static void SetupRequest<T>(
         this Mock<HttpMessageHandler> handler,
         string url,
         T responseObject,
         HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var json = JsonSerializer.Serialize(responseObject);
            var response = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(json),
            };

            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.ToString() == url),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response)
                .Verifiable();
        }

        public static void ReturnsJson<T>(this Mock<HttpMessageHandler> handler, string url, T responseObject)
        {
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString() == url),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() =>
                {
                    var json = JsonSerializer.Serialize(responseObject);
                    var response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(json),
                    };
                    return response;
                })
                .Verifiable();
        }

        public static void VerifyRequest(this Mock<HttpMessageHandler> handler, string url, Times times)
        {
            handler.Protected().Verify(
                "SendAsync",
                times,
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString() == url),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
