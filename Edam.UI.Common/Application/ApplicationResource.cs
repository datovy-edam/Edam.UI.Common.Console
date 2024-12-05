using System;
using Windows.ApplicationModel.Resources;

// -----------------------------------------------------------------------------
using Edam.Application;

namespace Edam.UI.Application;

public class ApplicationResource : IApplicationResource
{
  public String GetString(string key)
  {
     var loader = 
        ResourceLoader.GetForViewIndependentUse("ApplicationStrings");
     return loader.GetString(key);
  }
  public String GetLocalString(string key)
  {
     var loader = ResourceLoader.GetForViewIndependentUse("LocalStrings");
     return loader.GetString(key);
  }
}
