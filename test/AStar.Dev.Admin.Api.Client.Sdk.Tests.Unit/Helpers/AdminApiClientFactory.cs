using AStar.Dev.Admin.Api.Client.Sdk.AdminApi;
using AStar.Dev.Admin.Api.Client.Sdk.MockMessageHandlers;
using AStar.Dev.Logging.Extensions;
using Microsoft.Identity.Web;
using NSubstitute;

namespace AStar.Dev.Admin.Api.Client.Sdk.Helpers;

internal static class AdminApiClientFactory
{
    private                 const string                       IrrelevantUrl = "https://doesnot.matter.com";
    private static readonly       ILoggerAstar<AdminApiClient> DummyLogger   = Substitute.For<ILoggerAstar<AdminApiClient>>();

    public static AdminApiClient Create(HttpMessageHandler mockHttpMessageHandler)
    {
        var tokenAcquisitionServiceMock = Substitute.For<ITokenAcquisition>();
        var httpClient                  = new HttpClient(mockHttpMessageHandler) { BaseAddress = new(IrrelevantUrl) };

        return new(httpClient, tokenAcquisitionServiceMock, DummyLogger);
    }

    public static AdminApiClient CreateInternalServerErrorClient(string errorMessage)
    {
        var tokenAcquisitionServiceMock = Substitute.For<ITokenAcquisition>();
        var handler                     = new MockInternalServerErrorHttpMessageHandler(errorMessage);
        var httpClient                  = new HttpClient(handler) { BaseAddress = new(IrrelevantUrl) };

        return new(httpClient, tokenAcquisitionServiceMock, DummyLogger);
    }
}