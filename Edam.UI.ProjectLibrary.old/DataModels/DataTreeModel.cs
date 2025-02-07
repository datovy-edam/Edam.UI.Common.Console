using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.Data.AssetConsole;
using Edam.Data.AssetSchema;
using Edam.Data.AssetProject;
using Edam.InOut;
using Edam.UI.Controls.ViewModels;
using Edam.Data.Asset;
using Edam.UI.Controls.DataModels;
using System.Collections.ObjectModel;
using Edam.UI.Common.Controls;
using Windows.UI.Text;
using Microsoft.UI.Text;
using Edam.UI.Common.Resources;

namespace Edam.UI.Controls.DataModels
{

   public class DataTreeModel : ObservableObject
   {

      private Visibility m_TextVisibility;
      public Visibility TextVisibility
      {
         get { return m_TextVisibility; }
         set
         {
            if (m_TextVisibility != value)
            {
               m_TextVisibility = value;
               OnPropertyChanged(nameof(TextVisibility));
            }
         }
      }

      private SolidColorBrush m_VisitedBrush;
      public SolidColorBrush VisitedBrush
      {
         get { return m_VisitedBrush; }
         set
         {
            if (value != m_VisitedBrush)
            {
               m_VisitedBrush = value;
               OnPropertyChanged(nameof(VisitedBrush));
            }
         }
      }

      public AssetDataTree DataTree { get; set; }
      public string RootName
      {
         get
         {
            return DataTree == null ? String.Empty :
               DataTree.RootNamePath.FirstItem.Name;
         }
      }

      public AssetDataTreeItem Item { get; set; }

      public string LinkText
      {
         get { return String.IsNullOrWhiteSpace(Item.LinkText) ?
               " " : "*"; }
         set
         {
            if (Item.LinkText != value)
            {
               Item.LinkText = value;
               OnPropertyChanged(nameof(LinkText));
               OnPropertyChanged(nameof(ElementFullPath));
            }
         }
      }

      public string ElementFullPath
      {
         get
         { 
            return (String.IsNullOrWhiteSpace(Item.LinkText) ?
               String.Empty : Item.LinkText + " ") + Item.ElementFullPath; 
         }
         set
         {
            //if (Item.ElementFullPath != value)
            //{
            //   OnPropertyChanged(nameof(ElementFullPath));
            //}
         }
      }

      private SolidColorBrush m_TextBrush;
      public SolidColorBrush TextBrush
      {
         get { return m_TextBrush; }
         set
         {
            if (value != m_TextBrush)
            {
               m_TextBrush = value;
               OnPropertyChanged(nameof(TextBrush));
            }
         }
      }

      private FontWeight m_TextWeight;
      public FontWeight TextWeight
      {
         get { return m_TextWeight; }
         set
         {
            if (m_TextWeight != value)
            {
               m_TextWeight = value;
               OnPropertyChanged(nameof(TextWeight));
            }
         }
      }

      private bool m_IsVisited;
      public bool IsVisited
      {
         get { return m_IsVisited; }
         set
         {
            if (IsVisited != value)
            {
               m_IsVisited = value;
               Item.IsVisited = value;
               OnPropertyChanged(nameof(IsVisited));
               SetVisitedColor(value);
               VisitedCount = value ? 1 : 0;
            }
         }
      }

      private Visibility m_VisitedVisibility;
      public Visibility VisitedVisibility
      {
         get { return m_VisitedVisibility; }
         set
         {
            if (m_VisitedVisibility != value)
            {
               m_VisitedVisibility = value;
               OnPropertyChanged(nameof(VisitedVisibility));
            }
         }
      }

      private int m_VisitedCount;
      public int VisitedCount
      {
         get { return m_VisitedCount; }
         set
         {
            if (m_VisitedCount != value)
            {
               m_VisitedCount = value;
               OnPropertyChanged(nameof(VisitedCount));
               VisitedVisibility = value > 0 ? 
                  Visibility.Visible : Visibility.Collapsed;
               VisitedCountText = "( " + value.ToString() + " )";
            }
         }
      }

      private string m_VisitedCountText;
      public string VisitedCountText
      {
         get
         {
            return m_VisitedCountText;
         }
         set
         {
            if (m_VisitedCountText != value)
            {
               m_VisitedCountText = value;
               OnPropertyChanged(nameof(VisitedCountText));
            }
         }
      }

      private SolidColorBrush m_BlackColor =
         new SolidColorBrush(Microsoft.UI.Colors.Black);
      private SolidColorBrush m_LightGrayColor =
         new SolidColorBrush(Microsoft.UI.Colors.LightGray);

      public ObservableCollection<DataTreeModel> Children { get; set; } = 
         new ObservableCollection<DataTreeModel>();

      public DataTreeModel()
      {
         SetVisitedColor();
         IsVisited = false;
         VisitedCount = 0;
      }

      public void SetVisitedColor(bool visited = false)
      {
         VisitedBrush = visited ?
            m_BlackColor : m_LightGrayColor;
         TextWeight = visited ?
            FontWeights.Bold : FontWeights.Normal;
      }

      /// <summary>
      /// Clear all visited nodes...
      /// </summary>
      /// <param name="node"></param>
      public static void ClearVisited(DataTreeModel node)
      {
         node.IsVisited = false;
         node.VisitedCount = 0;
         node.VisitedCountText = String.Empty;
         foreach(var c in node.Children)
         {
            ClearVisited(c);
            c.IsVisited = false;
            c.VisitedCount = 0;
            c.VisitedCountText = String.Empty;
         }
      }

      /// <summary>
      /// Set Visited Count...
      /// </summary>
      /// <param name="node"></param>
      public static int SetVisitedCount(DataTreeModel node)
      {
         if (node.Children == null)
         {
            if (node.IsVisited)
            {
               node.VisitedCount = 1;
            }
         }

         int count = 0;
         int visitedCount = 0;
         foreach (var c in node.Children)
         {
            if (c.IsVisited)
            {
               node.VisitedCount++;
               count++;
            }
            int cnt = SetVisitedCount(c);
            c.VisitedCount = cnt;
            visitedCount += cnt;
         }
         node.VisitedCount = count + visitedCount;
         return count;
      }

      /// <summary>
      /// Find a node that supportes the element with the given path.
      /// </summary>
      /// <param name="node"></param>
      /// <param name="elementPath"></param>
      /// <returns></returns>
      public static DataTreeModel Find(DataTreeModel node, string elementPath)
      {
         if (node.Item.ElementFullPath == elementPath)
         {
            return node;
         }
         foreach(DataTreeModel child in node.Children)
         {
            var item = Find(child, elementPath);
            if (item != null)
            {
               return item;
            }
         }
         return null;
      }

      #region -- 4.00 - Prepare Data Tree Model

      /// <summary>
      /// Prepare model using given DataTreeModel...
      /// </summary>
      /// <param name="item"></param>
      /// <returns></returns>
      private static DataTreeModel PrepareModel(DataTreeModel item)
      {
         int count = 0;

         // if item don't have children then review its DataType for inheritance
         if (item.Item.Children.Count == 0)
         {

            return item;
         }

         // manage children
         foreach(var c in item.Item.Children)
         {
            string colorKey = String.Empty;
            if (c.OriginalElement.Kind == DataElementKind.AuditProperty)
            {
               // TODO: replace the hardcoded value
               colorKey = StringToBrushConverter.AuditBrush;
            }
            else if (
               (c.OriginalElement.Kind == DataElementKind.CodeSet ||
                c.Type == ItemType.Folder) &&
               (c.OriginalElement.ElementName.EndsWith("Code") ||
                c.OriginalElement.ElementName.EndsWith("Type") ||
                c.OriginalElement.ElementName.EndsWith("Category")))
            {
               colorKey = StringToBrushConverter.CodeSetBrush;
            }
            else if (
               (c.OriginalElement.Kind == DataElementKind.CodeSet ||
                c.Type == ItemType.Folder) &&
               (c.OriginalElement.ElementName.EndsWith("Association")))
            {
               colorKey = StringToBrushConverter.AssociationBrush;
            }
            //else if (c.Type == ItemType.Folder && count == 0)f
            //{
            //   colorKey = StringToBrushConverter.RootBrush;
            //}
            else if (c.Type == ItemType.Folder)
            {
               colorKey = StringToBrushConverter.FolderBrush;
            }

            var m = new DataTreeModel
            {
               TextBrush = StringToBrushConverter.ToBrush(colorKey),
               Item = c,
               TextVisibility = Visibility.Visible
            };
            item.Children.Add(m);
            PrepareModel(m);
            count++;
         }
         return item;
      }

      public static DataTreeModel PrepareTree(AssetDataTree tree)
      {
         DataTreeModel r = new DataTreeModel
         {
            Item = tree.Root,
            DataTree = tree,
            TextVisibility = Visibility.Visible
         };
         return PrepareModel(r);
      }

      #endregion

   }

}
