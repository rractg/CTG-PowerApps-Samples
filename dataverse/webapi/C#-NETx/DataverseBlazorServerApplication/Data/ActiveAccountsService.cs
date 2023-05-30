using PowerApps.Samples.Messages;

namespace DataverseBlazorServerApplication.Data;

public class ActiveAccountsService
{
    private readonly DataverseService _dataverseService;

    public ActiveAccountsService(DataverseService dataverseService)
    {
        _dataverseService = dataverseService;
    }

    public async Task<RetrieveMultipleResponse> GetActiveAccountsAsync()
    {
        return await _dataverseService.GetActiveAccountsAsync();
    }
}