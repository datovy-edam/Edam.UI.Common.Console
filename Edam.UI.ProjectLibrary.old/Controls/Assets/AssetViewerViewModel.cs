using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Microsoft.UI;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.Data.AssetSchema;
using Edam.UI.Common.Controls;
using Edam.Data.Asset.Services;
using Edam.InOut;
using Edam.Diagnostics;
using apphelper = Edam.UI.Common.Application;
using Edam.UI.Controls.DataModels;
using Edam.Data.AssetConsole;
using Edam.UI.Common;

namespace Edam.UI.Controls.ViewModels
{

   /// <summary>
   /// Asset Viewer View Model
   /// </summary>
   public class AssetViewerViewModel : ObservableObject
   {

      private List<AssetData> m_AssetDataSet = null;

      private AssetType m_AssetType;
      public AssetType AssetType
      {
         get { return m_AssetType; }
         set
         {
            m_AssetType = value;
         }
      }

      private ObservableCollection<AssetDataElement> m_Items;
      public ObservableCollection<AssetDataElement> Items
      {
         get { return m_Items; }
         set
         {
            if (m_Items != value)
            {
               m_Items = value;
               OnPropertyChanged("Items");
            }
         }
      }

      //public List<AssetDataElement> Items { get; set; }
      public string ItemsRecordCountText
      {
         get { return "Elements: " + Items.Count.ToString(); }
      }

      public NotificationEvent NotifyEventCompletion { get; set; }

      public AssetViewerViewModel()
      {
         AssetType = AssetType.Asset;
         Items = new ObservableCollection<AssetDataElement>();
         // new List<AssetDataElement>();
      }

      public void NotifyTabChanged(string name, object tabItem)
      {
         if (NotifyEventCompletion != null)
         {
            NotificationArgs args = new NotificationArgs();
            args.Type = NotificationType.AssetViewerTabChanged;
            args.EventData = tabItem;
            args.MessageText = name;
            args.ResultsLog = null;
            NotifyEventCompletion(this, args);
         }
      }

      #region -- 4.00 - Setup Use Cases

      public void SetupUseCase(AssetData asset)
      {
         Items.Clear();

         if (asset == null)
         {
            return;
         }

         int cnt = 0;
         bool updateElementNo = false;

         foreach (var uc in asset.UseCases)
         {
            if (uc.Items.Count > 1)
            {
               updateElementNo =
                  uc.Items[0].ElementNo == uc.Items[1].ElementNo;
            }
            foreach (var e in uc.Items)
            {
               e.ElementNo = updateElementNo ? cnt++ : e.ElementNo;
               Items.Add(e);
            }
         }
      }

      #endregion
      #region -- 4.00 - Project Item Processing

      /// <summary>
      /// Process Project Item.
      /// </summary>
      /// <param name="projectItem">item to process</param>
      public void ProcessProjectItem(object projectItem)
      {
         ProjectItem prj = (ProjectItem)projectItem;

         // TODO: make the following Async...
         var results = ProjectHelper.Execute(prj);
         if (results.Success)
         {
            m_AssetDataSet = results.Instance;

            Items.Clear();

            // if available, use the first asset data item...
            if (results.Instance != null && results.Instance.Count > 0 &&
               results.Instance[0] != null)
            {
               foreach (var i in results.Instance[0].Items)
               {
                  Items.Add(i);
               }
            }

            // notify that we do have the project asset data items...
            if (NotifyEventCompletion != null)
            {
               NotificationArgs args = new NotificationArgs();
               args.Type = NotificationType.AssetDataSetAvailable;
               args.EventData = ProjectContext.AssetData;
               args.MessageText = ItemsRecordCountText;
               args.ResultsLog = results;
               NotifyEventCompletion(this, args);
            }

            return;
         }

         // TODO: Unify all app-logs using Diagnostics ResultLog Messaging
         if (apphelper.ApplicationHelper.ApplicationLog != null)
         {
            apphelper.ApplicationHelper.ApplicationLog.WriteMessage(
               results.LastException);
         }
      }

      /// <summary>
      /// Manage Notification...
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="args"></param>
      public void ManageNotification(object sender, NotificationArgs args)
      {
         if (args == null || args.EventData == null ||
            AssetType != AssetType.Asset)
         {
            return;
         }

         switch(args.Type)
         {
            case NotificationType.RunProjectItem:
               ProcessProjectItem(args.EventData);
               break;
            case NotificationType.AssetSaveOptionChanged:
               ProjectHelper.ProcessSaveOption(args.MessageText);
               break;
            default:
               break;
         }
      }

      #endregion

   }

}
