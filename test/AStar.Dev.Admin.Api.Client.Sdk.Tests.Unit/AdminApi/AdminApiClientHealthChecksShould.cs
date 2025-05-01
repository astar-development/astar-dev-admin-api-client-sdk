using AStar.Dev.Admin.Api.Client.Sdk.Helpers;
using AStar.Dev.Admin.Api.Client.Sdk.MockMessageHandlers;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.AdminApi;

[TestSubject(typeof(AdminApiClient))]
public class AdminApiClientHealthChecksShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetHealthAsyncWhenTheApiIsUnreachableVersion2()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetHealthCheckAsync();

        response.IsFailure.ShouldBeTrue();
        response.Value?.Description.ShouldBe("Unable to retrieve the description of the Health Status");
        response.Value?.Status.ShouldBe("Could not get a response from the AStar.Dev.Admin.Api.");
    }

    [Fact]
    public async Task ReturnExpectedFailureFromGetHealthAsyncWhenTheApiIsUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetHealthAsync();

        response.Status.ShouldBe("Could not get a response from the AStar.Dev.Admin.Api.");
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetHealthAsyncWhenCheckFails()
    {
        var sut = AdminApiClientFactory.CreateInternalServerErrorClient("Health Check failed.");

        var response = await sut.GetHealthAsync();

        response.Status.ShouldBe("Health Check failed - Internal Server Error.");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetHealthAsyncWhenCheckSucceeds()
    {
        var handler = new MockSuccessHttpMessageHandler("Health");
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetHealthAsync();

        response.Status.ShouldBe("OK");
    }
}