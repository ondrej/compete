﻿using System;
using System.Collections.Generic;

namespace Compete.Model.Game
{
  [Serializable]
  public class MatchResult
  {
    public string TeamName1
    { 
      get; private set;
    }

    public string TeamName2
    { 
      get; private set;
    }

    public string WinnerTeamName
    {
      get; private set;
    }

    public string LoserTeamName
    {
      get; private set;
    }

    public bool IsTie
    {
      get; private set;
    }

    public string Log
    {
      get; private set;
    }

    public static MatchResult Tie(string teamName1, string teamName2, string log)
    {
      return new MatchResult()
      {
        TeamName1 = teamName1,
        TeamName2 = teamName2,
        IsTie = true,
        Log = log
      };
    }

    public static MatchResult WinnerAndLoser(string winnerTeamName, string loserTeamName, string log)
    {
      return new MatchResult()
      {
        TeamName1 = winnerTeamName,
        TeamName2 = loserTeamName,
        IsTie = false,
        WinnerTeamName = winnerTeamName,
        LoserTeamName = loserTeamName,
        Log = log
      };
    }

    public bool IsSameMatchup(MatchResult matchResult)
    {
      return matchResult.TeamName1 == TeamName1 && matchResult.TeamName2 == TeamName2
          || matchResult.TeamName1 == TeamName2 && matchResult.TeamName2 == TeamName1;
    }
  }
}
