using System.Net.Http.Json;
using System.Text.Json;
using AStar.Dev.Admin.Api.Client.Sdk.Models;
using AStar.Dev.Api.HealthChecks;
using AStar.Dev.Functional.Extensions;
using AStar.Dev.Logging.Extensions;
using Microsoft.Identity.Web;

namespace AStar.Dev.Admin.Api.Client.Sdk.AdminApi;

/// <summary>
///     The <see href="AdminApiClient"></see> class.
/// </summary>
public sealed class AdminApiClient(HttpClient httpClient, ITokenAcquisition tokenAcquisitionService, ILoggerAstar<AdminApiClient> logger) : IApiClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    /// <inheritdoc />
    public async Task<HealthStatusResponse> GetHealthAsync(CancellationToken cancellationToken = default)
    {
        logger.LogHealthCheckStart(Constants.ApiName);

        try
        {
            var response = await httpClient.GetAsync("/health/ready", cancellationToken);

            return response.IsSuccessStatusCode
                       ? (await JsonSerializer.DeserializeAsync<HealthStatusResponse>(await response.Content.ReadAsStreamAsync(cancellationToken), JsonSerializerOptions, cancellationToken))!
                       : ReturnLoggedFailure(response);
        }
        catch (HttpRequestException ex)
        {
            logger.LogException(ex);

            return new() { Status = $"Could not get a response from the {Constants.ApiName}." };
        }
    }

    /// <inheritdoc />
    public async Task<Result<string, HealthStatusResponse>> GetHealthCheckAsync(CancellationToken cancellationToken = new ())
    {
        logger.LogHealthCheckStart(Constants.ApiName);

        try
        {
            var response = await httpClient.GetAsync("/health/ready", cancellationToken);

            return response.IsSuccessStatusCode
                       ? (await JsonSerializer.DeserializeAsync<HealthStatusResponse>(await response.Content.ReadAsStreamAsync(cancellationToken), JsonSerializerOptions, cancellationToken))!
                       : ReturnLoggedFailure(response);
        }
        catch (HttpRequestException ex)
        {
            logger.LogException(ex);

            return new HealthStatusResponse { Status = $"Could not get a response from the {Constants.ApiName}." };
        }
    }

    /// <summary>
    ///     The GetSiteConfigurationAsync method will get the User Configuration.
    /// </summary>
    /// <returns>The Site Configuration - populated or empty</returns>
    public async Task<Result<string, IEnumerable<SiteConfiguration>>> GetSiteConfigurationAsync()
    {
        var token = await tokenAcquisitionService.GetAccessTokenForUserAsync(["api://2ca26585-5929-4aae-86a7-a00c3fc2d061/ToDoList.Write"]);

        httpClient.AddBearerToken(token);
        var response = await GetSafelyAsync<IEnumerable<SiteConfiguration>>("site-configurations?version=1.0");

        return response.IsSuccess
                   ? response
                   : ReturnLoggedFailure("GetSiteConfiguration", response.Error);
    }

    private async Task<Result<string, TResponse>> GetSafelyAsync<TResponse>( string uri)
    {
        try
        {
            var response = await httpClient.GetAsync(uri);

            if(response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<TResponse>())!;
            }

            logger.LogApiCallFailed("AStar.Dev.Admin.Api", uri, $"StatusCode: {response.StatusCode}, ResponseCode: {response.ReasonPhrase}");

            return Result<string, TResponse>.Failure($"Call to {uri} failed with {(response.ReasonPhrase ?? response?.ReasonPhrase)}")!;
        }
        catch(Exception ex)
        {
            logger.LogException(ex);

            return Result<string, TResponse>.Failure(ex.Message)!;
        }
    }

    private HealthStatusResponse ReturnLoggedFailure(HttpResponseMessage response)
    {
        logger.LogHealthCheckFailure(Constants.ApiName, response.ReasonPhrase ?? "Failure reason not known");

        return new() { Status = $"Health Check failed - {response.ReasonPhrase}." };
    }

    private string ReturnLoggedFailure(string endpointName, string? reasonPhrase)
    {
        logger.LogApiCallFailed(Constants.ApiName, endpointName,reasonPhrase ?? "Failure reason not known");

        return $"Call to {endpointName} failed - {reasonPhrase}.";
    }

    //
    // /// <summary>
    // ///     The GetModelsToIgnoreAsync method will get the models to ignore.
    // /// </summary>
    // /// <returns>A collection of 0 or more models to ignore</returns>
    // public async Task<IEnumerable<ModelToIgnore>> GetModelsToIgnoreAsync()
    // {
    //     logger.LogInformation("Getting the models-to-ignore.");
    //     var response = await httpClient.GetAsync("models-to-ignore?version=1");
    //
    //     return response.IsSuccessStatusCode
    //                ? (await response.Content.ReadAsStringAsync()).FromJson<IEnumerable<ModelToIgnore>>(Utilities.Constants
    //                                                                                                             .WebDeserialisationSettings)
    //                : [];
    // }
    //
    // /// <summary>
    // ///     The GetScrapeDirectoriesAsync method will get the Scrape Directories.
    // /// </summary>
    // /// <returns>The Scrape Directories - populated or empty</returns>
    // public async Task<ScrapeDirectories> GetScrapeDirectoriesAsync()
    // {
    //     logger.LogInformation("Getting the scrape-directories.");
    //     var response = await httpClient.GetAsync("scrape-directories?version=1");
    //
    //     return response.IsSuccessStatusCode
    //                ? (await response.Content.ReadAsStringAsync()).FromJson<ScrapeDirectories>(Utilities.Constants
    //                                                                                                    .WebDeserialisationSettings)
    //                : new ScrapeDirectories();
    // }
    //
    // /// <summary>
    // ///     The GetSearchConfigurationAsync method will get the Search Configuration.
    // /// </summary>
    // /// <returns>The Search Configuration - populated or empty</returns>
    // public async Task<SearchConfiguration> GetSearchConfigurationAsync()
    // {
    //     logger.LogInformation("Getting the search-configuration.");
    //     var response = await httpClient.GetAsync("search-configuration?version=1");
    //
    //     return response.IsSuccessStatusCode
    //                ? (await response.Content.ReadAsStringAsync()).FromJson<SearchConfiguration>(Utilities.Constants
    //                                                                                                      .WebDeserialisationSettings)
    //                : new SearchConfiguration();
    // }
    //
    // /// <summary>
    // ///     The GetTagsToIgnoreAsync method will get the Tags to Ignore.
    // /// </summary>
    // /// <returns>A collection of 0 or more Tags to Ignore</returns>
    // public async Task<IEnumerable<TagToIgnore>> GetTagsToIgnoreAsync()
    // {
    //     logger.LogInformation("Getting the Tags-to-ignore.");
    //     var response = await httpClient.GetAsync("tags-to-ignore?version=1");
    //
    //     return response.IsSuccessStatusCode
    //                ? (await response.Content.ReadAsStringAsync()).FromJson<IEnumerable<TagToIgnore>>(Utilities.Constants
    //                                                                                                           .WebDeserialisationSettings)
    //                : [];
    // }
    //
    // /// <summary>
    // ///     The GetUserConfigurationAsync method will get the User Configuration.
    // /// </summary>
    // /// <returns>The User Configuration - populated or empty</returns>
    // public async Task<UserConfiguration> GetUserConfigurationAsync()
    // {
    //     logger.LogInformation("Getting the User Configuration.");
    //     var response = await httpClient.GetAsync("user-configuration?version=1");
    //
    //     return response.IsSuccessStatusCode
    //                ? (await response.Content.ReadAsStringAsync()).FromJson<UserConfiguration>(Utilities.Constants
    //                                                                                                    .WebDeserialisationSettings)
    //                : new UserConfiguration();
    // }
}