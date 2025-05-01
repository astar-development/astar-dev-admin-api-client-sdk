using AStar.Dev.Admin.Api.Client.Sdk.Helpers;
using AStar.Dev.Admin.Api.Client.Sdk.MockMessageHandlers;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.AdminApi;

[TestSubject(typeof(AdminApiClient))]
public class AdminApiClientGetScrapeDirectoriesShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetScrapeDirectoriesAsyncWhenTheApiIsUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetScrapeDirectoriesAsync();

        response.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetScrapeDirectoriesAsyncWhenCheckFails()
    {
        var sut = AdminApiClientFactory.CreateInternalServerErrorClient("ScrapeDirectories");

        var response = await sut.GetScrapeDirectoriesAsync();

        response.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetScrapeDirectoriesAsyncWhenCheckSucceeds()
    {
        var handler = new MockSuccessHttpMessageHandler("ScrapeDirectories");
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetScrapeDirectoriesAsync();

        response.IsSuccess.ShouldBeTrue();
    }
}