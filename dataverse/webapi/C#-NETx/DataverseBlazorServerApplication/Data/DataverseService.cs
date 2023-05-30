using Newtonsoft.Json.Linq;
using PowerApps.Samples;
using PowerApps.Samples.Messages;
using PowerApps.Samples.Methods;

namespace DataverseBlazorServerApplication.Data;

public class DataverseService
{
    private readonly Service _service;
    private readonly DataverseTokenService _dataverseTokenService;

    public DataverseService(IConfiguration configuration, DataverseTokenService dataverseTokenService)
    {
        _dataverseTokenService = dataverseTokenService;
        
        Config config = new()
        {
            Url = configuration["DataverseConfig:Url"],
            GetAccessToken = _dataverseTokenService.GetBearerToken, //Function defined below to manage getting OAuth token
            MaxRetries = byte.Parse(configuration["DataverseConfig:MaxRetries"]), 
            TimeoutInSeconds = ushort.Parse(configuration["DataverseConfig:TimeoutInSeconds"]),
            Version = configuration["DataverseConfig:Version"],
            CallerObjectId = new Guid(configuration["DataverseConfig:CallerObjectId"]), 
            DisableCookies = false
        };
        
        _service = new Service(config);
    }

    public async Task<RetrieveMultipleResponse> GetActiveAccountsAsync()
    {
        RetrieveMultipleResponse activeAccountsSavedQueryIdResponse =
            await _service.RetrieveMultiple(
                queryUri: "savedqueries?$select=name,savedqueryid" +
                          "&$filter=name eq 'Active Accounts'");
        var activeAccountsSavedQueryId = (Guid)activeAccountsSavedQueryIdResponse.Records.FirstOrDefault()["savedqueryid"];

        return await _service.RetrieveMultiple(
            queryUri: $"accounts?savedQuery={activeAccountsSavedQueryId}",
            maxPageSize: 3,
            includeAnnotations: true);
    }
}