using AStar.Dev.Admin.Api.Client.Sdk.Helpers;
using AStar.Dev.Admin.Api.Client.Sdk.MockMessageHandlers;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.AdminApi;

[TestSubject(typeof(AdminApiClient))]
public class AdminApiClientGetModelToIgnoreShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetModelsToIgnoreAsyncWhenTheApiIsUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetModelsToIgnoreAsync();

        response.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetModelsToIgnoreAsyncWhenCheckFails()
    {
        var sut = AdminApiClientFactory.CreateInternalServerErrorClient("ModelsToIgnore");

        var response = await sut.GetModelsToIgnoreAsync();

        response.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetModelsToIgnoreAsyncWhenCheckSucceeds()
    {
        var handler = new MockSuccessHttpMessageHandler("ModelsToIgnore");
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetModelsToIgnoreAsync();

        response.IsSuccess.ShouldBeTrue();
    }
}