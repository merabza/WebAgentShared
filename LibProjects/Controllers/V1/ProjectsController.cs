using CliToolsData.Models;
using Installer.AgentClients;
using Installer.Models;
using LibProjectsData.Contracts.V1;
using LibProjectsData.Contracts.V1.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SystemToolsShared;

namespace LibProjects.Controllers.V1;

//[Route("api/[controller]")]
public class ProjectsController : Controller
{
    private readonly ApiKeysChecker _apiKeysChecker;
    private readonly FileStorages _fileStorages;
    private readonly InstallerSettings _installerSettings;

    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(ILogger<ProjectsController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _apiKeysChecker = new ApiKeysChecker(_logger, configuration);
        _installerSettings = InstallerSettings.Create(configuration);
        _fileStorages = FileStorages.Create(configuration);
    }

    // POST api/project/updatesettings
    [HttpPost(InsApiRoutes.Projects.UpdateSettings)]
    public ActionResult UpdateSettings([FromBody] UpdateSettingsRequest? request)
    {
        try
        {
            //ProjectUpdateDataModel? psu =
            //    await _apiKeysChecker.DeserializeAsync<ProjectUpdateDataModel>(Request.Body);
            var remoteAddress = Request.HttpContext.Connection.RemoteIpAddress;

            if (remoteAddress is null)
                return BadRequest("invalid remoteAddress");

            if (!_apiKeysChecker.Check(request?.ApiKey, remoteAddress.MapToIPv4().ToString()))
                return BadRequest("API Key is invalid");

            if (request is null)
                return BadRequest("request is Empty");

            var parametersFileDateMask =
                request.ParametersFileDateMask ?? _installerSettings.ParametersFileDateMask;

            var parametersFileExtension =
                request.ParametersFileExtension ?? _installerSettings.ParametersFileExtension;

            if (request.ProjectName is null || request.ServiceName is null || request.AppSettingsFileName is null ||
                parametersFileDateMask is null || parametersFileExtension is null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(_installerSettings.ProgramExchangeFileStorageName))
                return BadRequest("ProgramExchangeFileStorageName does not specified");

            var fileStorageForUpload =
                _fileStorages.GetFileStorageDataByKey(_installerSettings.ProgramExchangeFileStorageName);

            if (fileStorageForUpload is null)
                return BadRequest(
                    $"FileStorage with name {_installerSettings.ProgramExchangeFileStorageName} does not exists");

            var agentClient =
                AgentClientsFabric.CreateAgentClientWithFileStorage(_logger, _installerSettings, fileStorageForUpload,
                    false);

            if (agentClient is null)
                return BadRequest("agentClient does not created");

            if (agentClient.UpdateAppParametersFile(request.ProjectName, request.ServiceName,
                    request.AppSettingsFileName, parametersFileDateMask, parametersFileExtension))
                return Ok();

            var message =
                $" Settings cannot be updated for Project {request.ProjectName} => service {request.ServiceName} can not stopped";
            _logger.LogError(message);
            return BadRequest(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, null);
            return BadRequest(e.Message + Environment.NewLine + e.StackTrace);
        }
    }

    // POST api/project/update
    [HttpPost(InsApiRoutes.Projects.UpdateService)]
    public ActionResult<string> UpdateService([FromBody] ProjectServiceUpdateRequest? request)
    {
        try
        {
            _logger.LogInformation(request is null ? "request is null" : $"request is - {request}");

            var remoteAddress = Request.HttpContext.Connection.RemoteIpAddress;

            if (remoteAddress is null)
                return BadRequest("invalid remoteAddress");

            if (!_apiKeysChecker.Check(request?.ApiKey, remoteAddress.MapToIPv4().ToString()))
                return BadRequest("API Key is invalid");

            if (request is null)
                return BadRequest("request is Empty");

            var programArchiveDateMask =
                request.ProgramArchiveDateMask ?? _installerSettings.ProgramArchiveDateMask;

            var programArchiveExtension =
                request.ProgramArchiveExtension ?? _installerSettings.ProgramArchiveExtension;

            var parametersFileDateMask =
                request.ParametersFileDateMask ?? _installerSettings.ParametersFileDateMask;

            var parametersFileExtension =
                request.ParametersFileExtension ?? _installerSettings.ParametersFileExtension;

            if (request.ProjectName is null || request.ServiceName is null || request.AppSettingsFileName is null ||
                request.ServiceUserName is null || programArchiveDateMask is null ||
                programArchiveExtension is null || parametersFileDateMask is null ||
                parametersFileExtension is null)
                return BadRequest("Some request parameters are not valid");

            if (string.IsNullOrWhiteSpace(_installerSettings.ProgramExchangeFileStorageName))
                return BadRequest("ProgramExchangeFileStorageName does not specified");

            var fileStorageForUpload =
                _fileStorages.GetFileStorageDataByKey(_installerSettings.ProgramExchangeFileStorageName);
            if (fileStorageForUpload is null)
                return BadRequest(
                    $"FileStorage with name {_installerSettings.ProgramExchangeFileStorageName} does not exists");

            var agentClient =
                AgentClientsFabric.CreateAgentClientWithFileStorage(_logger, _installerSettings,
                    fileStorageForUpload, false);

            if (agentClient is null)
                return BadRequest("agentClient does not created");

            var assemblyVersion = agentClient.InstallService(request.ProjectName, request.ServiceName,
                request.ServiceUserName, request.AppSettingsFileName, programArchiveDateMask, programArchiveExtension,
                parametersFileDateMask, parametersFileExtension);

            if (assemblyVersion != null)
                return Ok(assemblyVersion);

            var message = $"cannot be updated Project {request.ProjectName}";
            _logger.LogError(message);
            return BadRequest(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, null);
            return BadRequest(e.Message + Environment.NewLine + e.StackTrace);
        }
    }

    // POST api/project/update
    [HttpPost(InsApiRoutes.Projects.Update)]
    public ActionResult<string> Update([FromBody] ProjectUpdateRequest? request)
    {
        try
        {
            _logger.LogInformation(request is null ? "request is null" : $"request is - {request}");

            var remoteAddress = Request.HttpContext.Connection.RemoteIpAddress;

            if (remoteAddress is null)
                return BadRequest("invalid remoteAddress");

            if (!_apiKeysChecker.Check(request?.ApiKey, remoteAddress.MapToIPv4().ToString()))
                return BadRequest("API Key is invalid");

            if (request is null)
                return BadRequest("request is Empty");

            var programArchiveDateMask =
                request.ProgramArchiveDateMask ?? _installerSettings.ProgramArchiveDateMask;

            var programArchiveExtension =
                request.ProgramArchiveExtension ?? _installerSettings.ProgramArchiveExtension;

            var parametersFileDateMask =
                request.ParametersFileDateMask ?? _installerSettings.ParametersFileDateMask;

            var parametersFileExtension =
                request.ParametersFileExtension ?? _installerSettings.ParametersFileExtension;

            if (request.ProjectName is null || programArchiveDateMask is null || programArchiveExtension is null ||
                parametersFileDateMask is null || parametersFileExtension is null)
                return BadRequest("Some request parameters are not valid");

            if (string.IsNullOrWhiteSpace(_installerSettings.ProgramExchangeFileStorageName))
                return BadRequest("ProgramExchangeFileStorageName does not specified");

            var fileStorageForUpload =
                _fileStorages.GetFileStorageDataByKey(_installerSettings.ProgramExchangeFileStorageName);
            if (fileStorageForUpload is null)
                return BadRequest(
                    $"FileStorage with name {_installerSettings.ProgramExchangeFileStorageName} does not exists");

            var agentClient =
                AgentClientsFabric.CreateAgentClientWithFileStorage(_logger, _installerSettings,
                    fileStorageForUpload, false);

            if (agentClient is null)
                return BadRequest("agentClient does not created");

            var assemblyVersion = agentClient.InstallProgram(request.ProjectName, programArchiveDateMask,
                programArchiveExtension, parametersFileDateMask, parametersFileExtension);

            if (assemblyVersion != null)
                return Ok(assemblyVersion);

            var message = $"cannot be updated Project {request.ProjectName}";
            _logger.LogError(message);
            return BadRequest(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, null);
            return BadRequest(e.Message + Environment.NewLine + e.StackTrace);
        }
    }

    // POST api/project/stop
    [HttpPost(InsApiRoutes.Projects.StopService)]
    public ActionResult Stop(string serviceName, [FromQuery] string? apiKey)
    {
        try
        {
            var remoteAddress = Request.HttpContext.Connection.RemoteIpAddress;

            if (remoteAddress is null)
                return BadRequest("invalid remoteAddress");

            if (!_apiKeysChecker.Check(apiKey, remoteAddress.MapToIPv4().ToString()))
                return BadRequest("API Key is invalid");

            var agentClient =
                AgentClientsFabric.CreateAgentClient(_logger, false, _installerSettings.InstallFolder);

            if (agentClient is null) return BadRequest("API Client does not created");

            var message = $"Project {serviceName} service can not stopped";

            if (agentClient.StopService(serviceName))
                return Ok();

            _logger.LogError(message);
            return BadRequest(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, null);
            return BadRequest(e.Message + Environment.NewLine + e.StackTrace);
        }
    }

    // POST api/project/start
    [HttpPost(InsApiRoutes.Projects.StartService)]
    public ActionResult Start(string serviceName, [FromQuery] string? apiKey)
    {
        try
        {
            var remoteAddress = Request.HttpContext.Connection.RemoteIpAddress;

            if (remoteAddress is null)
                return BadRequest("invalid remoteAddress");

            if (!_apiKeysChecker.Check(apiKey, remoteAddress.MapToIPv4().ToString()))
                return BadRequest("API Key is invalid");

            var agentClient =
                AgentClientsFabric.CreateAgentClient(_logger, false, _installerSettings.InstallFolder);

            if (agentClient is null) return BadRequest("API Client does not created");


            if (agentClient.StartService(serviceName))
                return Ok();

            var errorMessage = $"Service {serviceName} service can not started";
            _logger.LogError(errorMessage);
            return BadRequest(errorMessage);
        }
        catch (Exception e)
        {
            _logger.LogError(e, null);
            return BadRequest(e.Message + Environment.NewLine + e.StackTrace);
        }
    }

    // POST api/project/remove
    [HttpDelete(InsApiRoutes.Projects.RemoveProject)]
    public ActionResult Remove(string projectName, [FromQuery] string? apiKey)
    {
        //ეს არის პროგრამის წაშლის ის ვარიანტი, როცა პროგრამა სერვისი არ არის
        return RemoveProjectService(projectName, null, apiKey);
    }

    // POST api/project/removeservice
    [HttpDelete(InsApiRoutes.Projects.RemoveService)]
    public ActionResult RemoveService(string projectName, string serviceName, [FromQuery] string? apiKey)
    {
        //ეს არის პროგრამის წაშლის ის ვარიანტი, როცა პროგრამა სერვისია
        return RemoveProjectService(projectName, serviceName, apiKey);
    }

    // GET api/project/getappsettingsversion
    [HttpGet(InsApiRoutes.Projects.GetAppSettingsVersion)]
    public async Task<ActionResult<string>> GetAppSettingsVersion(int serverSidePort, string apiVersionId,
        [FromQuery] string? apiKey)
    {
        try
        {
            var remoteAddress = Request.HttpContext.Connection.RemoteIpAddress;

            if (remoteAddress is null)
                return BadRequest("invalid remoteAddress");

            if (!_apiKeysChecker.Check(apiKey, remoteAddress.MapToIPv4().ToString()))
                return BadRequest("API Key is invalid");

            var webAgentClient =
                new WebAgentClient(_logger, $"http://localhost:{serverSidePort}/api/{apiVersionId}/", apiKey);
            return Ok(await webAgentClient.GetAppSettingsVersion());
        }
        catch (Exception e)
        {
            _logger.LogError(e, null);
            //return BadRequest(e.Message + Environment.NewLine + e.StackTrace);
            return BadRequest(e.Message);
        }
    }

    // POST api/project/getversion
    [HttpGet(InsApiRoutes.Projects.GetVersion)]
    public async Task<ActionResult<string>> GetVersion(int serverSidePort, string apiVersionId,
        [FromQuery] string? apiKey)
    {
        try
        {
            var remoteAddress = Request.HttpContext.Connection.RemoteIpAddress;

            if (remoteAddress is null)
                return BadRequest("invalid remoteAddress");

            if (!_apiKeysChecker.Check(apiKey, remoteAddress.MapToIPv4().ToString()))
                return BadRequest("API Key is invalid");

            var webAgentClient =
                new WebAgentClient(_logger, $"http://localhost:{serverSidePort}/api/{apiVersionId}/", apiKey);
            return Ok(await webAgentClient.GetVersion());
        }
        catch (Exception e)
        {
            _logger.LogError(e, null);
            return BadRequest(e.Message);
        }
    }

    private ActionResult RemoveProjectService(string projectName, string? serviceName, string? apiKey)
    {
        try
        {
            var remoteAddress = Request.HttpContext.Connection.RemoteIpAddress;

            if (remoteAddress is null)
                return BadRequest("invalid remoteAddress");

            if (!_apiKeysChecker.Check(apiKey, remoteAddress.MapToIPv4().ToString()))
                return BadRequest("API Key is invalid");

            var agentClient =
                AgentClientsFabric.CreateAgentClient(_logger, false, _installerSettings.InstallFolder);

            if (agentClient is null) return BadRequest("API Client does not created");

            if (serviceName is null)
            {
                if (agentClient.RemoveProject(projectName))
                    return Ok();
            }
            else
            {
                if (agentClient.RemoveProjectAndService(projectName, serviceName))
                    return Ok();
            }

            var errorMessage = $"Project {projectName} => service {serviceName} can not be removed";
            _logger.LogError(errorMessage);
            return BadRequest(errorMessage);
        }
        catch (Exception e)
        {
            _logger.LogError(e, null);
            return BadRequest(e.Message + Environment.NewLine + e.StackTrace);
        }
    }

}