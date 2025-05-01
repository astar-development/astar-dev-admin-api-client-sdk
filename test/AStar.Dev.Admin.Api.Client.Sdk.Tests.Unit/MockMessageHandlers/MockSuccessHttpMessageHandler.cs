using System.Net;
using System.Text.Json;
using AStar.Dev.Admin.Api.Client.Sdk.Models;
using AStar.Dev.Api.HealthChecks;

namespace AStar.Dev.Admin.Api.Client.Sdk.MockMessageHandlers;

public sealed class MockSuccessHttpMessageHandler(string responseRequired) : HttpMessageHandler
{
    public int Counter { get; set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken  cancellationToken)
    {
        HttpContent content;

#pragma warning disable IDE0045 // Convert to conditional expression
        if(responseRequired == "SiteConfigurations")
        {
            content = new StringContent(JsonSerializer.Serialize(new List<SiteConfiguration> { new () } ));
        }
        else if(responseRequired == "SiteConfiguration")
        {
            content = new StringContent(JsonSerializer.Serialize(new SiteConfiguration()));
        }
        else if(responseRequired == "SearchConfiguration")
        {
            content = new StringContent(JsonSerializer.Serialize(new List<SearchConfiguration> { new () } ));
        }
        else if(responseRequired == "TagToIgnore")
        {
            content = new StringContent(JsonSerializer.Serialize(new List<TagToIgnore> { new () } ));
        }
        else if(responseRequired == "ModelsToIgnore")
        {
            content = new StringContent(JsonSerializer.Serialize(new List<ModelToIgnore> { new () } ));
        }
        else if (responseRequired == "Health")
        {
            content = new StringContent(JsonSerializer.Serialize(new HealthStatusResponse { Status = "OK" }));
        }
        else
        {
            content = new StringContent(JsonSerializer.Serialize(new HealthStatusResponse { Status = "NotSureYet" }));
        }
#pragma warning restore IDE0045 // Convert to conditional expression

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = content });
    }
}