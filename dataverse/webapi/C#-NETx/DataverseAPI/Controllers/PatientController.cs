using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace DataverseAPI.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class PatientController
{
    private readonly ILogger<PatientController> _logger;

    public PatientController(ILogger<PatientController> logger)
    {
        _logger = logger;
    }   
    
    [HttpPost]
    public ActionResult<Patient> AddPatient(Patient patient)
    {
        try
        {
            // Generate a new ID for the patient
            //int newId = _patients.Max(p => p.Id) + 1;
            patient.Id = 1;

            // Add the patient to the list
            

            // Return the newly added patient
            return new OkObjectResult(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            // Handle any exceptions that occur during the operation
            return new StatusCodeResult(500);
        }
    }
}