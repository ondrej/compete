using System.Collections.Generic;
using System.IO;
using System.Linq;

using Compete.Bot;
using Compete.Model.Game;
using Compete.Site.Infrastructure;
using Compete.Site.Models;

namespace Compete.Site.Refereeing
{
  public class CompetitionFactory
  {
    public Competition CreateCompetition(AssemblyFile[] files, IEnumerable<TeamInfo> teams, IEnumerable<TeamInfo> networkTeams)
    {
      DynamicAssemblyTypeFinder dynamicAssemblyTypeFinder = new DynamicAssemblyTypeFinder();
      dynamicAssemblyTypeFinder.AddAll(files);

      IGame game = dynamicAssemblyTypeFinder.Create<IGame>().Single();
      Competition competition = new Competition(game);

      IEnumerable<string> teamNames = teams.Select(team => team.Name);
      foreach (IBotFactory botFactory in dynamicAssemblyTypeFinder.Create<IBotFactory>())
      {
        string teamName = Path.GetFileNameWithoutExtension(botFactory.GetType().Assembly.Location);
        if (teamNames.Contains(teamName))
        {
          competition.AddPlayer(new BotPlayer(teamName, botFactory.CreateBot()));
        }
      }

      INetworkBotFactory factory = dynamicAssemblyTypeFinder.Create<INetworkBotFactory>().Single();
      foreach (TeamInfo team in networkTeams)
      {
        competition.AddPlayer(new BotPlayer(team.Name, factory.CreateBot(team.Url)));
      }

      return competition;
    }
  }
}
