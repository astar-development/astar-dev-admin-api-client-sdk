using AStar.Dev.Admin.Api.Client.Sdk.AdminApi;
using AStar.Dev.Logging.Extensions;
using JetBrains.Annotations;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using NSubstitute;

namespace AStar.Dev.Admin.Api.Client.Sdk;

[TestSubject(typeof(ServiceCollectionExtensions))]
public class ServiceCollectionExtensionsShould
{
    private readonly ServiceProvider serviceProvider;

    public ServiceCollectionExtensionsShould()
    {
        var configurationManager = new ConfigurationManager();
        configurationManager.AddJsonFile("appsettings.json");
        var serviceCollection           = new ServiceCollection();
        var tokenAcquisitionServiceMock = Substitute.For<ITokenAcquisition>();
        serviceCollection.AddSingleton(tokenAcquisitionServiceMock);
        var loggerMock = Substitute.For<ILoggerAstar<AdminApiClient>>();
        serviceCollection.AddSingleton(loggerMock);

        serviceCollection.AddAdminApiClient(configurationManager);

        serviceProvider = serviceCollection.BuildServiceProvider();
    }
    [Fact]
    public void AddTheExpectedServices()
    {
        serviceProvider.GetService<IOptions<AdminApiConfiguration>>().ShouldNotBeNull();
        serviceProvider.GetService<AdminApiClient>().ShouldNotBeNull();
    }

    [Fact]
    public void AddTheExpectedHttpClient()
        => serviceProvider.GetService<AdminApiClient>().ShouldNotBeNull();
}