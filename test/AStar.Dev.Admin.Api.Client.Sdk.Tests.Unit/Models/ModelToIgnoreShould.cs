using AStar.Dev.Utilities;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.Models;

[TestSubject(typeof(ModelToIgnore))]
public class ModelToIgnoreShould
{
    [Fact]
    public void ContainTheExpectedProperties()
        => new ModelToIgnore { Value = "Not Relevant Here" }.ToJson().ShouldMatchApproved();
}