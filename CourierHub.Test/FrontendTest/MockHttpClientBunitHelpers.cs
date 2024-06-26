﻿using Bunit;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CourierHub.Test.FrontendTest {
    public static class MockHttpClientBunitHelpers {
        public static MockHttpMessageHandler AddMockHttpClient(this TestServiceProvider services) {
            var mockHttpHandler = new MockHttpMessageHandler();
            var httpClient = mockHttpHandler.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:7008/");
            services.AddSingleton(httpClient);
            return mockHttpHandler;
        }

        public static MockedRequest RespondJson<T>(this MockedRequest request, T content) {
            request.Respond(req => {
                var response = new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent(JsonSerializer.Serialize(content))
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return response;
            });
            return request;
        }

        public static MockedRequest RespondString(this MockedRequest request, string content) {
            request.Respond(req => {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(content);
                return response;
            });
            return request;
        }

        public static MockedRequest SendJson(this MockedRequest request) {
            request.Respond(req => {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                return response;
            });
            return request;
        }
    }
}