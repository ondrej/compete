using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compete.Model.Reports
{
  public class TeamSummary
  {
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Url { get; set; }
    public IEnumerable<string> TeamMembers { get; set; }

    public TeamSummary(string teamName, string displayName, string url, IEnumerable<string> teamMembers)
    {
      Name = teamName;
      DisplayName = displayName;
      Url = url;
      TeamMembers = teamMembers;
    }
  }
}
