using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.Data.AssetConsole;
using Edam.Data.AssetSchema;
using Edam.Data.AssetProject;
using Edam.InOut;
using Edam.UI.Controls.ViewModels;
using Edam.UI.Controls.DataModels;
using System.Collections.ObjectModel;
using Edam.Json.JsonDataTree;
using Edam.UI.Common;

namespace Edam.UI.Controls.ViewModels
{

   public class AssetDataTreeViewModel : ObservableObject
   {

      /// <summary>
      /// The TreeItem refers to the root of the visual tree...
      /// </summary>
      private DataTreeModel m_TreeView;
      public DataTreeModel TreeView
      {
         get { return m_TreeView; }
         set
         {
            if (m_TreeView != value)
            {
               m_TreeView = value;
               OnPropertyChanged(nameof(TreeView));
            }
         }
      }

      private DataTreeModel m_SelectedItem;
      public DataTreeModel SelectedItem
      {
         get { return m_SelectedItem; }
         set
         {
            if (m_SelectedItem != value && value != null)
            {
               m_SelectedItem = value;
               OnPropertyChanged(nameof(SelectedItem));
               
               CurrentItem = m_SelectedItem;
            }
            if (value != null)
            {
               SetCurrentItem(value, DataTreeEventType.ItemSelected);
            }
         }
      }

      private DataTreeModel m_CurrentItem = null;
      public DataTreeModel CurrentItem
      {
         get { return m_CurrentItem; }
         set
         {
            if (m_CurrentItem != value)
            {
               m_CurrentItem = value;
               OnPropertyChanged(nameof(CurrentItem));
            }
         }
      }

      private MapItemType m_MapType = MapItemType.Unknown;
      public MapItemType MapType
      {
         get { return m_MapType; }
      }

      public DataMapContext m_MapContext = null;
      public DataMapContext MapContext
      {
         get { return m_MapContext; }
         set 
         { 
            if (m_MapContext != value)
            {
               m_MapContext = value;
               OnPropertyChanged(nameof(MapContext));
            }
         }
      }

      public KeyEventData KeyEventData
      {
         get { return m_MapContext.KeyEventData; }
         set { m_MapContext.SetKeyEventData(value); }
      }

      public string GetSampleInstance()
      {
         if (TreeView == null)
         {
            return String.Empty;
         }
         var inst = JsonDataTreeInstance.PrepareInstance(TreeView.Item);
         return inst.JsonText;
      }

      public void SetMapContext(DataMapContext context, MapItemType type)
      {
         m_MapType = type;
         m_MapContext = context;

         switch(type)
         {
            case MapItemType.Source:
               context.Source.TreeModel = TreeView;
               if (String.IsNullOrWhiteSpace(context.Source.JsonInstanceSample))
               {
                  context.Source.JsonInstanceSample = GetSampleInstance();
               }
               break;
            case MapItemType.Target:
               context.Target.TreeModel = TreeView;
               break;
            default:
               break;
         }

      }

      public void SetCurrentItem(DataTreeModel item = null,
         DataTreeEventType type = DataTreeEventType.ItemSelected)
      {
         if (item == null)
         {
            item = SelectedItem;
         }
         CurrentItem = item;

         DataTreeEventArgs args = new DataTreeEventArgs();
         args.DataItem = item;
         args.Type = type;
         args.Context = m_MapContext;
         args.MapType = MapType;
         args.KeyEventData = KeyEventData == null ? 
            new KeyEventData() : KeyEventData;

         DataTreeEvent treeEvent = (MapType == MapItemType.Source) ?
            m_MapContext.Source.ManageNotification :
            m_MapContext.Target.ManageNotification;

         if (treeEvent != null)
         {
            treeEvent(this, args);
         }
      }

   }

}
