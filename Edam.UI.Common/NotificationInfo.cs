using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.Diagnostics;

namespace Edam.UI.Common
{

   public enum NotificationType
   {
      Unknown = 0,
      ProjectItemSelected = 10,
      RunProjectItem = 20,
      AssetDataSetAvailable = 30,
      AssetViewerTabChanged = 40,
      AssetSaveOptionChanged = 50,
      AssetSaveTextRequested = 60,
      AssetViewerChanged = 61,
      DatePeriodChanged = 70,
      ParticipantSelected = 80,
      CodeSetAvailable = 90,
      KeyEvent = 100,

      AddItem = 110,
      SaveItem = 111,
      RemoveItem = 112,
      ExecuteItem = 115,

      ItemSelected = 200
   }

   public class NotificationArgs : EventArgs
   {
      public NotificationType Type { get; set; }
      public object EventData { get; set; }
      public string MessageText { get; set; }
      public IResultsLog ResultsLog { get; set; }
   }

   public delegate void NotificationEvent(object sender, NotificationArgs args);

}
