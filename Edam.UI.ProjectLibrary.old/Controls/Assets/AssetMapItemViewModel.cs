using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.Data.AssetSchema;
using Edam.UI.Controls.DataModels;
using Edam.UI.Controls.Assets;
using Edam.Data.Asset;
using Edam.Data.AssetUseCases;

namespace Edam.UI.Controls.ViewModels
{

   /// <summary>
   /// The Map Item establish the set of items to be mapped to accomplish a 
   /// specific source to target mapping.
   /// </summary>
   public class AssetMapItemViewModel : ObservableObject
   {

      #region -- 1.00 - Properties and Fields

      public AssetMapItemControl ParentControl { get; set; }
      public DataMapContext Context { get; set; }

      public AssetUseCaseMap UseCase
      {
         get { return Context.UseCase; }
      }

      /// <summary>
      /// List of map-items containing source/target mappings and details
      /// </summary>
      private ObservableCollection<AssetDataMapItem> m_MapItemList;
      public ObservableCollection<AssetDataMapItem> MapItemList
      {
         get { return m_MapItemList; }
         set
         {
            if (m_MapItemList != value)
            {
               m_MapItemList = value;
               OnPropertyChanged(nameof(MapItemList));
            }
         }
      }

      private AssetDataMapItem m_SelectedMapItem;
      public AssetDataMapItem SelectedMapItem
      {
         get { return m_SelectedMapItem; }
         set
         { 
            if (m_SelectedMapItem != value)
            {
               m_SelectedMapItem = value;
               OnPropertyChanged(nameof(SelectedMapItem));

               if (!Context.IsControlKeyPressed)
               {
                  CurrentMapItem = value;
               }

               ManageSelectedMapItem(value);
            }
         }
      }

      public AssetDataMapItem CurrentMapItem
      {
         get { return UseCase.SelectedMapItem; }
         set
         {
            if (UseCase.SelectedMapItem != value)
            {
               UseCase.SelectedMapItem = value;
               OnPropertyChanged(nameof(CurrentMapItem));
            }
         }
      }

      private MapItemInfo m_SourceSelectedItem;
      public MapItemInfo SourceSelectedItem
      {
         get { return m_SourceSelectedItem; }
         set
         {
            if ( m_SourceSelectedItem != value)
            {
               m_SourceSelectedItem = value;
               OnPropertyChanged(nameof(SourceSelectedItem));
               Context.SelectedSourceItem = value;
               Context.SelectedItem = value;
            }
         }
      }

      private MapItemInfo m_TargetSelectedItem;
      public MapItemInfo TargetSelectedItem
      {
         get { return m_TargetSelectedItem; }
         set
         {
            if (m_TargetSelectedItem != value)
            {
               m_TargetSelectedItem = value;
               OnPropertyChanged(nameof(TargetSelectedItem));
               Context.SelectedTargetItem = value;
               Context.SelectedItem = value;
            }
         }
      }

      #endregion
      #region -- 1.50 - Constructor and Initialization

      public AssetMapItemViewModel()
      {
         MapItemList = new ObservableCollection<AssetDataMapItem>();
      }

      public void SetContext(DataMapContext context)
      {
         Context = context;
         Context.Source.ManageNotification = ManageSourceEvent;
         Context.Target.ManageNotification = ManageTargetEvent;
      }

      #endregion
      #region -- 4.00 - Get/Setup Map Items

      private MapItemInfo GetElementItem(
         string name, string path, NamespaceInfo ns, DataMapInstance instance)
      {
         return Context.GetMapItem(instance, name, path);
      }

      /// <summary>
      /// Set item as current item and add it to the groups as needed...
      /// </summary>
      /// <param name="item">map item</param>
      private void Setup(AssetDataMapItem item)
      {
         CurrentMapItem = item;

         if (item == null)
         {
            return;
         }

         foreach(var i in MapItemList)
         {
            if (i.ItemPath == item.ItemPath)
            {
               return;
            }
         }

         MapItemList.Add(item);
      }

      /// <summary>
      /// Setup found map-item list that matched a given element path.
      /// </summary>
      /// <param name="items">map-item list</param>
      private void Setup(List<AssetDataMapItem> items)
      {
         MapItemList.Clear();
         foreach (var item in items)
         {
            MapItemList.Add(item);
         }

         if (items.Count > 0)
         {
            Setup(items[0]);
         }
         else
         {
            Setup((AssetDataMapItem)null);
         }
      }

      /// <summary>
      /// Setup found map-item list that matched a given element path.
      /// </summary>
      /// <param name="items">map-item list</param>
      private void Setup(ObservableCollection<AssetDataMapItem> items)
      {
         ObservableCollection<AssetDataMapItem> list = 
            new ObservableCollection<AssetDataMapItem>();
         foreach (var item in items)
         {
            if (item.SourceElement.Count > 0 &&
               item.TargetElement.Count > 0)
            {
               list.Add(item);
            }
         }
         MapItemList = list;

         if (items.Count > 0)
         {
            Setup(items[0]);
         }
         else
         {
            Setup((AssetDataMapItem)null);
         }
      }

      #endregion
      #region -- 4.00 - Find Map Items

      private static MapItemInfo Find(
         List<MapItemInfo> list,
         DataTreeModel item)
      {
         foreach (var i in list)
         {
            if (i.Path == item.Item.Element.ElementPath)
            {
               return i;
            }
         }
         return null;
      }

      private static MapItemInfo Find(
         List<MapItemInfo> list,
         MapItemInfo item)
      {
         foreach (var i in list)
         {
            if (i.Path == item.Path)
            {
               return i;
            }
         }
         return null;
      }

      #endregion
      #region -- 4.00 - Select/Add Map Item

      /// <summary>
      /// Add item to map-item list as needed...
      /// </summary>
      /// <param name="type">type identify to be source or target</param>
      /// <param name="args">Data Tree event arguments</param>
      private MapItemInfo Add(
         MapItemType type, DataTreeEventArgs args, 
         DataTreeEventType eventType)
      {
         // get tree item
         var item = args.DataItem as DataTreeModel;
         if (item == null)
         {
            return null;
         }

         // determine if need to add to current element list or find it...
         if (!args.KeyEventData.IsControlKeyPressed || CurrentMapItem == null)
         {
            Setup(UseCase.Add(item.Item.Element));
         }

         // identify the list
         List<MapItemInfo> list = type == MapItemType.Source ?
               CurrentMapItem.SourceElement :
               CurrentMapItem.TargetElement;

         // find/add selected tree element
         var i = Find(list, item);
         if (i != null)
         {
            return i;
         }

         item.IsVisited = true;
         var e = GetElementItem(
            item.Item.Element.ElementName,
            item.Item.Element.ElementPath,
            item.Item.Element.GetElementNamespace(),
            type == MapItemType.Source ? 
               args.Context.Source : args.Context.Target);

         e.TreeItem = item;
         e.MapItemId = CurrentMapItem.MapItemId;

         e.Namespace.Prefix = item.Item.Element.ElementQualifiedName.Prefix;
         e.Namespace.UriText = item.Item.Element.NamespaceText;

         list.Add(e);

         UseCase.Add(type, e);
         Setup(MapItemList);

         return e;
      }

      /// <summary>
      /// Item has been selected... identify and setup map-item list as needed
      /// </summary>
      /// <param name="type">type identify to be source or target</param>
      /// <param name="args">Data Tree event arguments</param>
      private MapItemInfo ItemSelected(
         MapItemType type, DataTreeEventArgs args)
      {
         if (args.KeyEventData.IsControlKeyPressed)
         {
            return Add(type, args, DataTreeEventType.ItemSelected);
         }

         // get tree item
         var item = args.DataItem as DataTreeModel;
         if (item == null)
         {
            return null;
         }

         // the following may return a collection of items that match with given
         // element-full-path since it may be part of multiple mapping groups.
         var flist = UseCase.SelectItem(type, item.ElementFullPath);
         Setup(flist);

         // from the possible multiple mapping groups always return the top one
         // since the user can select individual nodes and work with those later
         if (flist != null && flist.Count != 0)
         {
            var list = type == MapItemType.Source ? 
               flist[0].SourceElement : flist[0].TargetElement;
            if (list != null)
            {
               return list[0];
            }
         }
         return null;
      }

      /// <summary>
      /// Rebuild Book/Booklet items related to the selected map-item.
      /// </summary>
      /// <param name="type">source or destination</param>
      /// <param name="item">map-item</param>
      private void ManageSelectedMapItem(
         MapItemType type, MapItemInfo item, DataTreeEventArgs args)
      {
         if (item != null)
         {
            Context.SetSelectedMapItem(type, item);
         }
         else
         {
            Context.ClearBookletItems();
         }

         ParentControl.BookletChanged(this, args);
      }

      /// <summary>
      /// Rebuild Book/Booklet items related to the selected map-item.
      /// </summary>
      /// <param name="item">asset map item</param>
      private void ManageSelectedMapItem(AssetDataMapItem item)
      {
         if (item != null && item.SourceElement.Count == 0)
         {
            return;
         }
         ManageSelectedMapItem(
            MapItemType.Source, 
            item == null ? null : item.SourceElement[0], null);
      }

      #endregion
      #region -- 4.00 - Manage Source/Target Events - Tree item selected

      /// <summary>
      /// Manage Source Event - Tree Item has been selected
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="args"></param>
      public void ManageSourceEvent(object sender, DataTreeEventArgs args)
      {
         MapItemInfo item;
         if (args.Type == DataTreeEventType.DoubleTapped)
         {
            Add(MapItemType.Source, args, args.Type);
         }

         item = ItemSelected(MapItemType.Source, args);

         ManageSelectedMapItem(MapItemType.Source, item, args);
         if (Context.BookModel != null && 
            args.Type != DataTreeEventType.ItemSelected)
         {
            Context.BookModel.Model.ClearAll();
         }
      }

      /// <summary>
      /// Manage Target Event - Tree Item has been selected
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="args"></param>
      public void ManageTargetEvent(object sender, DataTreeEventArgs args)
      {
         MapItemInfo item;
         if (args.Type == DataTreeEventType.DoubleTapped)
         {
            item = Add(MapItemType.Target, args, args.Type);
         }
         
         item = ItemSelected(MapItemType.Target, args);

         ManageSelectedMapItem(MapItemType.Target, item, args);
      }

      #endregion
      #region -- 4.00 - Map Item Delete Support

      private void Delete(
         MapItemType type,
         List<MapItemInfo> list,
         MapItemInfo item)
      {
         if (item == null)
         {
            return;
         }

         var i = Find(list, item);
         if (i != null)
         {
            DataTreeModel m = i.TreeItem as DataTreeModel;
            if (m != null)
            {
               m.IsVisited = false;
            }
            list.Remove(i);

            UseCase.Delete(type, i);
            Setup(MapItemList);
         }
      }

      public void DeleteItem(MapItemType type)
      {
         if (type == MapItemType.Source)
         {
            Delete(type, CurrentMapItem.SourceElement, SourceSelectedItem);
         }
         else
         {
            Delete(type, CurrentMapItem.TargetElement, TargetSelectedItem);
         }
      }

      /// <summary>
      /// Delete a complete map item source-target collection...
      /// </summary>
      /// <param name="item">asset data map item to delete</param>
      public void DeleteItem(AssetDataMapItem item)
      {
         AssetDataMapItem mitem = null;
         foreach(var i in MapItemList)
         {
            if (i.MapItemId == item.MapItemId)
            {
               mitem = i;
               break;
            }
         }
         if (mitem == null)
         {
            return;
         }

         Context.Delete(item);
         MapItemList.Remove(mitem);
      }

      #endregion

   }

}
