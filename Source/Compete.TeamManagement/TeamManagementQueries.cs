﻿using System;
using System.Collections.Generic;
using System.Linq;
using Compete.Core.Infrastructure;
using Compete.Model;
using Compete.Model.Game;
using Compete.Model.Reports;
using Compete.Model.Repositories;

namespace Compete.TeamManagement
{
  public interface ITeamManagementQueries
  {
    IEnumerable<TeamSummary> GetTeamSummaries();
    IEnumerable<TeamInfo> GetTeamInfos();
    bool TeamNameIsAvailable(string name);
    IEnumerable<TeamStandingSummary> GetTeamStandings();
    IEnumerable<string> GetAllTeamNames();
    string GetMyTeamName();
    IEnumerable<RecentMatch> GetMyRecentMatches();
    string GetMyTeamDisplayName();
    string GetMyTeamUrl();
    bool IsSignedIn { get; }
  }

  public class TeamManagementQueries : ITeamManagementQueries
  {
    readonly ITeamRepository _teamRepository;
    readonly ILeaderboardRepository _leaderboardRepository;
    private IFormsAuthentication _formsAuthentication;

    public TeamManagementQueries(ITeamRepository teamRepository, ILeaderboardRepository leaderboardRepository, IFormsAuthentication formsAuthentication)
    {
      _teamRepository = teamRepository;
      _leaderboardRepository = leaderboardRepository;
      _formsAuthentication = formsAuthentication;
    }

    public IEnumerable<TeamSummary> GetTeamSummaries()
    {
      return _teamRepository.GetAllTeams().Select(team => new TeamSummary(team.Name, team.DisplayName, team is NetworkTeam ? ((NetworkTeam)team).Url : null, team.TeamMembers.Select(teamMember => teamMember.Name)));
    }

    public IEnumerable<TeamInfo> GetTeamInfos()
    {
      return _teamRepository.GetAllTeams().Select(team => new TeamInfo(team.Name, team is NetworkTeam ? ((NetworkTeam)team).Url : null));
    }

    public bool TeamNameIsAvailable(string name)
    {
      var team = _teamRepository.FindByTeamName(name);
      return team == null;
    }

    public IEnumerable<TeamStandingSummary> GetTeamStandings()
    {
      var teamNames = _teamRepository.GetTeamNamesMap();
      var leaderboard = _leaderboardRepository.GetLeaderboard();
      return leaderboard.ToStandingSummary(teamNames);
    }

    public IEnumerable<string> GetAllTeamNames()
    {
      var teams = _teamRepository.GetAllTeams();
      return teams.Select(x => x.Name);
    }

    public string GetMyTeamName()
    {
      return _formsAuthentication.IsCurrentlySignedIn ? _formsAuthentication.SignedInUserName : "not signed in!";
    }

    public IEnumerable<RecentMatch> GetMyRecentMatches()
    {
      var currentTeam = GetMyTeamName();
      var leaderBoard = _leaderboardRepository.GetLeaderboard();
      var results = leaderBoard.GetMatchResultsForTeam(currentTeam);
      if (results == null)
      {
        return new List<RecentMatch>();
      }
      return results.Select(x => new RecentMatch(currentTeam, x));
    }

    public string GetMyTeamDisplayName()
    {
      var teamName = _formsAuthentication.SignedInUserName;
      return _teamRepository.FindByTeamName(teamName).DisplayName;
    }

    public string GetMyTeamUrl()
    {
      var teamName = _formsAuthentication.SignedInUserName;
      NetworkTeam team = _teamRepository.FindByTeamName(teamName) as NetworkTeam;
      return team == null ? null : team.Url;
    }

    public bool IsSignedIn
    {
      get { return _formsAuthentication.IsCurrentlySignedIn; }
    }
  }
}