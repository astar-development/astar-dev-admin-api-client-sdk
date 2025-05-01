using AStar.Dev.Admin.Api.Client.Sdk.Helpers;
using AStar.Dev.Admin.Api.Client.Sdk.MockMessageHandlers;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.AdminApi;

[TestSubject(typeof(AdminApiClient))]
public class AdminApiClientTagsToIgnoreShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetTagsToIgnoreAsyncWhenTheApiIsUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetTagsToIgnoreAsync();

        response.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetTagsToIgnoreAsyncWhenCheckFails()
    {
        var sut = AdminApiClientFactory.CreateInternalServerErrorClient("tags-to-ignore");

        var response = await sut.GetTagsToIgnoreAsync();

        response.IsFailure.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetTagsToIgnoreAsyncWhenCheckSucceeds()
    {
        var handler = new MockSuccessHttpMessageHandler("TagToIgnore");
        var sut     = AdminApiClientFactory.Create(handler);

        var response = await sut.GetTagsToIgnoreAsync();

        response.IsSuccess.ShouldBeTrue();
    }
}