﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Compete.Site.Infrastructure
{
  public interface IFormsAuthentication
  {
    void SignIn(string userName);
    void SignOut();
    bool IsCurrentlySignedIn { get; }
    string SignedInUserName { get; }
  }

  public class FormsAuthenticationService : IFormsAuthentication
  {
    public void SignIn(string userName)
    {
      FormsAuthentication.SetAuthCookie(userName, true);
    }

    public void SignOut()
    {
      FormsAuthentication.SignOut();
    }

    public bool IsCurrentlySignedIn
    {
      get { return HttpContext.Current.User != null; }
    }

    public string SignedInUserName
    {
      get { return HttpContext.Current.User.Identity.Name; }
    }
  }
}