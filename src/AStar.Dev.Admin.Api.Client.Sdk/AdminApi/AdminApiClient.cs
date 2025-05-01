using System.Net.Http.Json;
using System.Text.Json;
using AStar.Dev.Admin.Api.Client.Sdk.Models;
using AStar.Dev.Api.HealthChecks;
using AStar.Dev.Functional.Extensions;
using AStar.Dev.Logging.Extensions;
using AStar.Dev.Technical.Debt.Reporting;
using AStar.Dev.Utilities;
using Microsoft.Identity.Web;

namespace AStar.Dev.Admin.Api.Client.Sdk.AdminApi;

/// <summary>
///     The <see href="AdminApiClient"></see> class.
/// </summary>
public sealed class AdminApiClient(HttpClient httpClient, ITokenAcquisition tokenAcquisitionService, ILoggerAstar<AdminApiClient> logger) : IApiClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    /// <inheritdoc />
    [Refactor(1,1,"Remove from the Interface")]public async Task<HealthStatusResponse> GetHealthAsync(CancellationToken cancellationToken = default)
    {
        logger.LogHealthCheckStart(Constants.ApiName);

        try
        {
            var response = await httpClient.GetAsync("/health/ready", cancellationToken);

            return response.IsSuccessStatusCode
                       ? (await JsonSerializer.DeserializeAsync<HealthStatusResponse>(await response.Content.ReadAsStreamAsync(cancellationToken), JsonSerializerOptions, cancellationToken))!
                       : logger.ReturnLoggedFailure(response, Constants.ApiName);
        }
        catch (HttpRequestException ex)
        {
            logger.LogException(ex);

            return new() { Status = $"Could not get a response from the {Constants.ApiName}." };
        }
    }

    /// <inheritdoc />
    public async Task<Result<string, HealthStatusResponse>> GetHealthCheckAsync(CancellationToken cancellationToken = new ())
    => await GetSafelyAsync<HealthStatusResponse>("/health/ready?version=1.0");

    /// <summary>
    ///     The GetSiteConfigurationAsync method will get the User Configuration.
    /// </summary>
    /// <returns>The Site Configuration - populated or empty</returns>
    public async Task<Result<string, IEnumerable<SiteConfiguration>>> GetSiteConfigurationAsync()
    => await GetSafelyAsync<IEnumerable<SiteConfiguration>>("site-configurations?version=1.0");

    /// <summary>
    ///     The GetModelsToIgnoreAsync method will get the models to ignore.
    /// </summary>
    /// <returns>A collection of 0 or more models to ignore</returns>
    public async Task<Result<string, IEnumerable<ModelToIgnore>>> GetModelsToIgnoreAsync()
    => await GetSafelyAsync<IEnumerable<ModelToIgnore>>("models-to-ignore?version=1");

    /// <summary>
    ///     The GetScrapeDirectoriesAsync method will get the Scrape Directories.
    /// </summary>
    /// <returns>The Scrape Directories - populated or empty</returns>
    public async Task<Result<string, ScrapeDirectories>> GetScrapeDirectoriesAsync()
        => await GetSafelyAsync<ScrapeDirectories>("scrape-directories?version=1");

    /// <summary>
    ///     The GetSearchConfigurationAsync method will get the Search Configuration.
    /// </summary>
    /// <returns>The Search Configuration - populated or empty</returns>
    public async Task<Result<string, SearchConfiguration>> GetSearchConfigurationAsync()
        => await GetSafelyAsync<SearchConfiguration>("search-configuration?version=1");

    /// <summary>
    ///     The GetTagsToIgnoreAsync method will get the Tags to Ignore.
    /// </summary>
    /// <returns>A collection of 0 or more Tags to Ignore</returns>
    public async Task<Result<string, IEnumerable<TagToIgnore>>> GetTagsToIgnoreAsync()
    => await GetSafelyAsync<IEnumerable<TagToIgnore>>("search-configuration?version=1");

    private async Task<Result<string, TResponse>> GetSafelyAsync<TResponse>( string uri)
    {
        try
        {
            logger.LogApiCallStart(Constants.ApiName, uri);
            var token = await tokenAcquisitionService.GetAccessTokenForUserAsync(["api://2ca26585-5929-4aae-86a7-a00c3fc2d061/ToDoList.Write"]);

            _ = httpClient.AddBearerToken(token);
            var response = await httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                return logger.ReturnLoggedFailure<TResponse>( Constants.ApiName, uri, response.ReasonPhrase ?? response.StatusCode.ToString());
            }

            var result = (await response.Content.ReadFromJsonAsync<TResponse>(Utilities.Constants.WebDeserialisationSettings))!;
            return logger.ReturnLoggedSuccess(result, Constants.ApiName, "uri");
        }
        catch(Exception ex)
        {
            logger.LogException(ex);

            return Result<string, TResponse>.Failure(ex.Message)!;
        }
    }
}