using AStar.Dev.Utilities;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.AdminApi;

[TestSubject(typeof(AdminApiConfiguration))]
public class AdminApiConfigurationShould
{
    [Fact]
    public void ContainTheExpectedProperties()
        => new AdminApiConfiguration { Scopes = ["Not Relevant Here"], BaseUrl = new ("https://www.example.com") }.ToJson().ShouldMatchApproved();
}