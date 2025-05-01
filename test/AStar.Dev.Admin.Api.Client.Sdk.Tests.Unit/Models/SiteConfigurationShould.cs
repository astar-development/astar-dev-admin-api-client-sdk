using AStar.Dev.Utilities;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.Models;

[TestSubject(typeof(SiteConfiguration))]
public class SiteConfigurationShould
{
    [Fact]
    public void ContainTheExpectedProperties()
        => new SiteConfiguration
           {
               BaseUrl               = "https://not.relevant.here",
               Id                    = 1,
               LoginUrl              = "/login/not/relevant/here",
               LoginEmailAddress     = "not@relevant.com",
               Password              = "Nope, not this!",
               SiteConfigurationSlug = "slug-not-relevant-here",
               Username              = "username not relevant here"
           }
           .ToJson()
           .ShouldMatchApproved();
}