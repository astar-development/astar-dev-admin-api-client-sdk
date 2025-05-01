using AStar.Dev.Utilities;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.Models;

[TestSubject(typeof(SearchConfiguration))]
public class SearchConfigurationShould
{
    [Fact]
    public void ContainTheExpectedProperties()
        => new SearchConfiguration
           {
               BaseUrl             = "https://not.relevant.here",
               TotalPages          = 9,
               Id                  = 1,
               ImagePauseInSeconds = 5,
               LoginUrl            = "/not/relevant/here",
               SearchCategories =
               [
                   new()
                   {
                       Id                  = "111",
                       TotalPages          = 7,
                       LastKnownImageCount = 8,
                       LastPageVisited     = 9,
                       Name                = "Name NotRelevant Here",
                       Order               = 2
                   }
               ]
           }
           .ToJson()
           .ShouldMatchApproved();
}