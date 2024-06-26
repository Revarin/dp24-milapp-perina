﻿using Kris.Common.Models;

namespace Kris.Client.Connection;

public interface IHttpClientFactory
{
    HttpClient CreateHttpClient(string controller);
    HttpClient CreateAuthentizedHttpClient(string controller, JwtToken token);
}
