using System;

namespace Compete.Model.Game
{
  [Serializable]
  public class TeamInfo
  {
    public string Name { get; set; }
    public string Url { get; set; }

    public TeamInfo(string teamName, string url)
    {
      Name = teamName;
      Url = url;
    }
  }
}
