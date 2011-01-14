using System;
using System.Collections.Generic;
using System.Linq;

using Compete.Model.Game;
using Compete.Site.Models;
using Compete.Site.Refereeing;
using Compete.TeamManagement;

namespace Compete.Site.Infrastructure
{
  public class MatchStarter
  {
    readonly AssemblyFileRepository _assemblyFileRepository = new AssemblyFileRepository();
    readonly IRefereeThread _refereeThread;
    readonly ITeamManagementQueries _teamManagementQueries;

    public MatchStarter(IRefereeThread refereeThread, ITeamManagementQueries teamManagementQueries)
    {
      _refereeThread = refereeThread;
      _teamManagementQueries = teamManagementQueries;
    }

    public void Queue(string teamName)
    {
      var allTeams = _teamManagementQueries.GetTeamInfos();
      Queue(allTeams.Where(t => t.Url == null), allTeams.Where(t => t.Url != null));
    }

    public void QueueForAll()
    {
      var allTeams = _teamManagementQueries.GetTeamInfos();
      Queue(allTeams.Where(t => t.Url == null) , allTeams.Where(t => t.Url != null));
    }

    private void Queue(IEnumerable<TeamInfo> localTeams, IEnumerable<TeamInfo> networkTeams)
    {
      var parameters = new RoundParameters(_assemblyFileRepository.FindAllGamesAndPlayers().ToArray(), localTeams, networkTeams);
      _refereeThread.Start(parameters);
    }
  }
}
