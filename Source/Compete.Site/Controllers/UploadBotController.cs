﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compete.Site.Models;

namespace Compete.Site.Controllers
{
  public class UploadBotController : CompeteController
  {
    readonly AssemblyFileRepository _assemblyFileRepository = new AssemblyFileRepository();

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Index(string teamName, string password)
    {

      if (Request.Files.Count != 1)
      {
        throw new FileLoadException("only one file at a time, please. you loaded "+Request.Files.Count+".");
      }

      foreach (string file in Request.Files)
      {
        var hpf = Request.Files[file];

        if (hpf.ContentLength == 0)
        {
          continue;
        }
        if (!hpf.FileName.Split('.').Last().ToLower().Equals("dll"))
        {
          throw new ArgumentException("only .dll files only, please");
        }

        _assemblyFileRepository.Add(hpf, hpf.FileName);
      }
      return Redirect("~/MyTeam");
    }
  }
}