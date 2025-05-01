using AStar.Dev.Admin.Api.Client.Sdk.Helpers;
using AStar.Dev.Admin.Api.Client.Sdk.MockMessageHandlers;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.AdminApi;

[TestSubject(typeof(AdminApiClient))]
public class AdminApiClientGetSiteConfigurationShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetSiteConfigurationAsyncWhenTheApiIsUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetSiteConfigurationAsync();

        response.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetSiteConfigurationAsyncWhenCheckFails()
    {
        var sut = AdminApiClientFactory.CreateInternalServerErrorClient("Health Check failed.");

        var response = await sut.GetSiteConfigurationAsync();

        response.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetSiteConfigurationAsyncWhenCheckSucceeds()
    {
        var handler = new MockSuccessHttpMessageHandler("");
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetSiteConfigurationAsync();

        response.IsSuccess.ShouldBeTrue();
    }
}