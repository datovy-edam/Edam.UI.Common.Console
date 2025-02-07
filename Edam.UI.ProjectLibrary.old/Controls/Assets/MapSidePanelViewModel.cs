using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

// -----------------------------------------------------------------------------
using Edam.UI.Controls.DataModels;
using Edam.UI.Common.Controls;
using Edam.Data.AssetSchema;
using Edam.Data.Asset;
using Edam.Application;
using Edam.UI.Controls.Assets;
using Edam.UI.Common;
using Edam.UI.ProjectLibrary.ViewModels;

namespace Edam.UI.Controls.ViewModels
{

    public enum AssetViewOption
   {
      Unknown = 0,
      DataAssetView = 1,
      DataTreeView = 2,
      DataMapView = 3
   }

   public class MapSidePanelViewModel : ObservableObject
   {
      private NotificationArgs m_LastSaveOptionArgs = null;
      private AssetViewOption m_AssetViewOption = AssetViewOption.DataAssetView;

      public AssetDataTreeControl DataTreeControl { get; set; }

      private Visibility m_TreePanelVisible;
      public Visibility TreePanelVisible
      {
         get { return m_TreePanelVisible; }
         set
         {
            if (m_TreePanelVisible != value)
            {
               m_TreePanelVisible = value;
               OnPropertyChanged(nameof(TreePanelVisible));
            }
         }
      }

      private Visibility m_SidePanelVisible;
      public Visibility SidePanelVisible
      {
         get { return m_SidePanelVisible; }
         set
         {
            if (m_SidePanelVisible != value)
            {
               m_SidePanelVisible = value;
               OnPropertyChanged(nameof(SidePanelVisible));
            }
         }
      }

      private ObservableCollection<string> m_SaveOptions;
      public ObservableCollection<string> SaveOptions
      {
         get { return m_SaveOptions; }
         set
         {
            if (m_SaveOptions != value)
            {
               m_SaveOptions = value;
               OnPropertyChanged(nameof(SaveOptions));
            }
         }
      }

      private ObservableCollection<NamespaceInfo> m_Namespaces;
      public ObservableCollection<NamespaceInfo> Namespaces
      {
         get { return m_Namespaces; }
         set
         {
            if (m_Namespaces != value)
            {
               m_Namespaces = value;
               OnPropertyChanged(nameof(Namespaces));
            }
         }
      }

      private ObservableCollection<ListItemInfo> m_Items;
      public ObservableCollection<ListItemInfo> Items
      {
         get { return m_Items; }
         set
         {
            if (m_Items != value)
            {
               m_Items = value;
               OnPropertyChanged(nameof(Items));
            }
         }
      }

      public LableToggleViewModel NamespacesToggle { get; set; }

      public NotificationEvent NotifyAssetSaveOptionChanged { get; set; }
      public NotificationEvent NotifyEvent { get; set; }

      public MapSidePanelViewModel()
      {
         SetSidePanelVisible(true);
         Items = new ObservableCollection<ListItemInfo>();
         Namespaces = new ObservableCollection<NamespaceInfo>();
         //LoadItems();

         SaveOptions = new ObservableCollection<string>();
         PrepareSaveOptions();

         NamespacesToggle = new LableToggleViewModel("NAMESPACES");
      }

      /// <summary>
      /// Set Asset (true) or Tree (false) Side Panel visibility.
      /// </summary>
      /// <param name="isVisible">true to make the asset side panel visible.
      /// set to false to make the tree panel visible</param>
      public void SetSidePanelVisible(bool? isVisible = null)
      {
         if (isVisible == null)
         {
            isVisible = SidePanelVisible == Visibility.Visible ?
               false : true;
         }

         SidePanelVisible = isVisible.Value ? 
            Visibility.Visible : Visibility.Collapsed;
         TreePanelVisible = SidePanelVisible == Visibility.Visible ?
            Visibility.Collapsed : Visibility.Visible;
      }

      /// <summary>
      /// Make sure that the Tree side panel and Map Viwer panel are visible.
      /// </summary>
      public void DataViewChanged(AssetViewOption option)
      {
         var previousOption = m_AssetViewOption;
         m_AssetViewOption = option;

         if (option == AssetViewOption.DataAssetView)
         {
            SetSidePanelVisible(true);
         }
         else if (option == AssetViewOption.DataTreeView)
         {
            SetSidePanelVisible(false);
         }
         else
         {
            SetSidePanelVisible(false);
         }

         // prepare and notify that (Use Case) Map Viewing is selected
         // notification should be managed by AssetMapViewerControl
         if (NotifyEvent != null)
         {
            // create a new data map context if needed...
            DataTreeControl.ViewModel.MapContext = 
               DataMapContext.CreateContext(
                  DataTreeControl.ViewModel.MapContext, 
                  DataTreeControl, ProjectContext.Arguments);

            NotificationArgs args = new NotificationArgs
            {
               EventData = DataTreeControl.ViewModel.MapContext,
               MessageText = option.ToString(),
               Type = NotificationType.AssetViewerChanged
            };

            NotifyEvent(this, args);
         }
      }

      public void PrepareSaveOptions()
      {
         SaveOptions.Add(SaveOptionInfo.XSD);
         SaveOptions.Add(SaveOptionInfo.JSD);
         SaveOptions.Add(SaveOptionInfo.JLD);
         SaveOptions.Add(SaveOptionInfo.GQL);
         SaveOptions.Add(SaveOptionInfo.DATABASE);
         SaveOptions.Add(SaveOptionInfo.DDL);
         SaveOptions.Add(SaveOptionInfo.EXCEL);
         SaveOptions.Add(SaveOptionInfo.LEXICON_DATABASE);
         SaveOptions.Add(SaveOptionInfo.LEXICON_WORKBOOK);
         SaveOptions.Add(SaveOptionInfo.DATA_TEMPLATE_FILE);

      }

      public void SetNamespaces(List<NamespaceInfo> items)
      {
         Namespaces.Clear();
         foreach(NamespaceInfo item in items)
         {
            Namespaces.Add(item);
         }
      }

      public void AssetDataChanged(object dataItem)
      {
         AssetData assetData = dataItem as AssetData;
         if (assetData != null)
         {
            SetNamespaces(assetData.Namespaces);
         }
         else
         {
            Namespaces.Clear();
         }
      }

      private void DoSaveItem(bool doit)
      {
         if (NotifyEvent != null && 
            m_LastSaveOptionArgs != null && doit)
         {
            NotifyAssetSaveOptionChanged(this, m_LastSaveOptionArgs);
         }
         m_LastSaveOptionArgs = null;
      }

      public void SaveOptionChanged(string name)
      {
         m_LastSaveOptionArgs = new NotificationArgs();
         m_LastSaveOptionArgs.MessageText = name;
         m_LastSaveOptionArgs.Type = NotificationType.AssetSaveOptionChanged;
         m_LastSaveOptionArgs.EventData = name;

         Session.ShowMessageBox("Continue with request?", 
            "Save to (" + name + ")?", DoSaveItem, MessageBoxType.YesNo);
      }

      //public void LoadItems()
      //{
      //   List<ListItemInfo> items = new List<ListItemInfo>
      //   {
      //      new ListItemInfo
      //      {
      //         IconName = ListItemInfo.CHECKBOX_ICON, 
      //         Name = "Show Elements" },
      //      new ListItemInfo 
      //      {
      //         IconName = ListItemInfo.PERSONAL_FOLDER_ICON, 
      //         Name = "Show Use Cases" }
      //   };

      //   foreach(ListItemInfo item in items)
      //   {
      //      Items.Add(item);
      //   }
      //}

   }

}
