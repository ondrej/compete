using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Compete.Core.Infrastructure;
using Compete.Model.Game;
using Compete.Site.Infrastructure;
using Compete.Site.Models;

using Microsoft.Practices.ServiceLocation;

namespace Compete.Site.Refereeing
{
  public class Referee
  {
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Referee));
    readonly RoundParameters _parameters;

    public Referee(RoundParameters parameters)
    {
      _parameters = parameters;
    }

    public void StartRound()
    {
      using (var staging = new StagingArea(_parameters.AssemblyFiles))
      {
        var sw = new Stopwatch();
        sw.Start();
        var rr = AppDomainHelper.InSeparateAppDomain<RoundParameters, IEnumerable<MatchResult>>(staging.Root, new RoundParameters(staging.StagedAssemblies, _parameters.Teams, _parameters.NetworkTeams), RunRound);
        sw.Stop();
        _log.Info("Done: " + sw.Elapsed);
        ServiceLocator.Current.GetInstance<IScoreKeeper>().Record(rr);
      }
    }

    private static IEnumerable<MatchResult> RunRound(RoundParameters parameters)
    {
      var competitionFactory = new CompetitionFactory();
      var competition = competitionFactory.CreateCompetition(parameters.AssemblyFiles, parameters.Teams, parameters.NetworkTeams);
      var allTeamNames = parameters.Teams.Select(t => t.Name).Concat(parameters.NetworkTeams.Select(t => t.Name));
      return competition.PlayRound(allTeamNames);
    }
  }

  [Serializable]
  public class RoundParameters
  {
    public AssemblyFile[] AssemblyFiles
    {
      get;
      private set;
    }

    public List<TeamInfo> Teams
    {
      get;
      private set;
    }

    public List<TeamInfo> NetworkTeams
    {
      get;
      private set;
    }

    public RoundParameters(AssemblyFile[] files, IEnumerable<TeamInfo> teams, IEnumerable<TeamInfo> networkTeams)
    {
      AssemblyFiles = files;
      Teams = teams.ToList();
      NetworkTeams = networkTeams.ToList();
    }

    public static RoundParameters Merge(params RoundParameters[] parameters)
    {
      List<TeamInfo> teams = new List<TeamInfo>();
      List<TeamInfo> networkTeams = new List<TeamInfo>();
      List<AssemblyFile> assemblyFiles = new List<AssemblyFile>();

      foreach (var p in parameters)
      {
        teams.AddRange(p.Teams);
        networkTeams.AddRange(p.NetworkTeams);
        assemblyFiles.AddRange(p.AssemblyFiles);
      }

      return new RoundParameters(assemblyFiles.Distinct().ToArray(), teams.Distinct(), networkTeams.Distinct());
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      foreach (TeamInfo name in this.Teams.Concat(this.NetworkTeams))
      {
        if (sb.Length > 0)
        {
          sb.Append(", ");
        }
        sb.Append(name.Name);
      }

      return sb.ToString();
    }
  }
}