﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compete.Core.Infrastructure;
using Compete.Site.Infrastructure;
using Compete.TeamManagement;
using Newtonsoft.Json;

namespace Compete.Site.Controllers
{
  public class StandingsController : CompeteController
  {
    private readonly ITeamManagementQueries _teamManagementQueries;

    public StandingsController(ITeamManagementQueries teamManagementQueries)
    {
      _teamManagementQueries = teamManagementQueries;
    }

    public ActionResult Index()
    {
      ViewData["TeamStandings"] = JavaScriptConvert.SerializeObject(_teamManagementQueries.GetTeamStandings());
      ViewData["Username"] = _teamManagementQueries.GetMyTeamName();
      return View();
    }

    [AcceptVerbs(HttpVerbs.Get)]
    public ActionResult Latest()
    {
      var standings = _teamManagementQueries.GetTeamStandings();
      return Json(standings);
    }
  }
}
