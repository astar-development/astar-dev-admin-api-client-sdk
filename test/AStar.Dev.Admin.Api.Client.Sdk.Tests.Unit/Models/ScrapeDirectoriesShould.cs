using AStar.Dev.Utilities;
using JetBrains.Annotations;

namespace AStar.Dev.Admin.Api.Client.Sdk.Models;

[TestSubject(typeof(ScrapeDirectories))]
public class ScrapeDirectoriesShould
{
    [Fact]
    public void ContainTheExpectedProperties()
        => new ScrapeDirectories
           {
               BaseSaveDirectory   = "Base Save Directory Not Relevant Here",
               Id                  = 1,
               BaseDirectory       = "Base Directory Not Relevant Here",
               SubDirectoryName    = "Sub Directory Not Relevant Here",
               BaseDirectoryFamous = "Base Directory Famous Not Relevant Here",
               RootDirectory       = "Root Directory Not Relevant Here"
           }
           .ToJson()
           .ShouldMatchApproved();
}