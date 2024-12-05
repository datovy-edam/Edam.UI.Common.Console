using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using baseApp = Edam.Application;

namespace Edam.UI.Application;

public class ApplicationLog : baseApp.ApplicationLog, baseApp.IApplicationLog
{

  public ApplicationLog() : base()
  {

  }

  public new void WriteMessage(Exception exception)
  {

  }

  public new void WriteMessage(string message)
  {
  }

  public new void WriteEmptyLine()
  {
  }

}
