using AStar.Dev.Utilities;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.Models;

[TestSubject(typeof(TagToIgnore))]
public class TagToIgnoreShould
{
    [Fact]
    public void ContainTheExpectedProperties()
        => new TagToIgnore { Value = "Value not relevant here", IgnoreImage = true }.ToJson().ShouldMatchApproved();
}