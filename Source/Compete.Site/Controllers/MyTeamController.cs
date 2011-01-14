using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compete.Core.Infrastructure;
using Compete.Model.Game;
using Compete.Model.Repositories;
using Compete.Site.Filters;
using Compete.Site.Infrastructure;
using Compete.Site.Models;
using Compete.TeamManagement;

namespace Compete.Site.Controllers
{
  [RequireAuthenticationFilter]
  public class MyTeamController : CompeteController
  {
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITeamManagementQueries _teamManagementQueries;
    private readonly ITeamManagementCommands _teamManagementCommands;

    public MyTeamController(IConfigurationRepository configurationRepository, ITeamManagementQueries teamManagementQueries, ITeamManagementCommands teamManagementCommands)
    {
      _configurationRepository = configurationRepository;
      _teamManagementQueries = teamManagementQueries;
      _teamManagementCommands = teamManagementCommands;
    }

    [AcceptVerbs(HttpVerbs.Get)]
    public ActionResult Index()
    {
      var currentTeam = _teamManagementQueries.GetMyTeamName();
      if (currentTeam == null)
        throw new Exception("Cannot find team " + currentTeam);
      var currentTeamDisplayName = _teamManagementQueries.GetMyTeamDisplayName();
      var currentUrl = _teamManagementQueries.GetMyTeamUrl();

      ViewData["currentTeam"] = currentTeam;
      ViewData["currentTeamDisplayName"] = currentTeamDisplayName;
      ViewData["results"] = _teamManagementQueries.GetMyRecentMatches();
      ViewData["currentRound"] = _configurationRepository.GetConfiguration().RoundNumber;
      ViewData["currentTeamUrl"] = currentUrl;

      return View();
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Index(FormCollection form)
    {
      var url = form["url"];
      if (String.IsNullOrEmpty(url))
      {
          ViewData["ErrorMessage"] = "URL is invalid.";
          return Index();
      }

      var currentTeam = _teamManagementQueries.GetMyTeamName();
      if (currentTeam == null)
        throw new Exception("Cannot find team " + currentTeam);

      if (!_teamManagementCommands.UpdateUrl(currentTeam, url))
      {
          ViewData["ErrorMessage"] = "Couldn't update URL.";
          return Index();
      }

      ViewData["InfoMessage"] = "Updated.";
      return Index();
    }
  }
}
