using AStar.Dev.Admin.Api.Client.Sdk.Helpers;
using AStar.Dev.Admin.Api.Client.Sdk.MockMessageHandlers;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.AdminApi;

[TestSubject(typeof(AdminApiClient))]
public class AdminApiClientSearchConfigurationShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetSearchConfigurationAsyncWhenTheApiIsUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetSearchConfigurationAsync();

        response.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetSearchConfigurationAsyncWhenCheckFails()
    {
        var sut = AdminApiClientFactory.CreateInternalServerErrorClient("SearchConfiguration");

        var response = await sut.GetSearchConfigurationAsync();

        response.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetSearchConfigurationAsyncWhenCheckSucceeds()
    {
        var handler = new MockSuccessHttpMessageHandler("SearchConfigurations");
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetSearchConfigurationAsync();

        response.IsSuccess.ShouldBeTrue();
    }
}