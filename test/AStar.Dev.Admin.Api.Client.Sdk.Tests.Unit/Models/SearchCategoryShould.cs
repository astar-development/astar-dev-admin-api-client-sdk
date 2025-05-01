using AStar.Dev.Utilities;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.Models;

[TestSubject(typeof(SearchCategory))]
public class SearchCategoryShould
{
    [Fact]
    public void ContainTheExpectedProperties()
        => new SearchCategory
           {
               Id                  = "1",
               Name                = "Name Not Relevant Here",
               LastKnownImageCount = 1,
               LastPageVisited     = 2,
               Order               = 3,
               TotalPages          = 4
           }.ToJson().ShouldMatchApproved();
}