using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;

// -----------------------------------------------------------------------------

using Edam.DataObjects.ReferenceData;
using Edam.DataObjects.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;

namespace Edam.UI.Common.Controls.Helpers
{

   public class GridInfo
   {
      public int RowCount { get; set; }
      public int ColumnCount { get; set; }
      public void ClearFields()
      {
         RowCount = 0;
         ColumnCount = 0;
      }
   }
   public class GridItemInfo
   {
      public ModelColumnInfo Column { get; set; }
      public int RowNo { get; set; }
      public int ColumnNo { get; set; }
      public int ColumnSpan { get; set; }
      public int RowSpan { get; set; }

      public object GetControl()
      {
         return Column.EditControl;
      }

      public void Dispose()
      {
         if (Column == null || Column.EditControl == null)
            return;
         Column.Dispose();
      }
   }

   public class FormControl : ObservableObject
   {
      private IElementNodeGroup m_ElementGroup;
      private Grid m_Grid = null;
      private GridInfo m_GridInfo = new GridInfo();
      private List<GridItemInfo> m_Items = new List<GridItemInfo>();
      private ModelData m_ModelData = new ModelData();
   
      public string FormTitle { get; set; }

      private ElementNodeInfo m_Node;
      public List<ModelGroupInfo> Groups
      {
         get { return m_Node.Groups; }
      }

      public ModelData ModelData
      {
         get { return m_ModelData; }
      }
      public List<GridItemInfo> Items
      {
         get { return m_Items; }
      }
      private ModelGroupInfo m_SelectedItem;
      public ModelGroupInfo SelectedItem
      {
         get { return m_SelectedItem; }
      }

      public FormControl(ElementNodeInfo node, IElementNodeGroup group)
      {
         m_ModelData.Columns = new List<ModelColumnInfo>();
         m_ElementGroup = group;
         Initialize(node);
      }

      public void Initialize(ElementNodeInfo node)
      {
         m_Node = node;
         if (node == null || node.Groups == null || node.Groups.Count == 0)
         {
            return;
         }
         SetGroup(0);
      }

      public void SetGroup(int groupNo)
      {
         m_SelectedItem = m_Node.Groups[0];
         m_SelectedItem.SetParent(m_Node);
      }

      private void PrepareFields()
      {
         m_Items.Clear();
         int crow = 0;
         int ccol;
         foreach (PresentationRowInfo i in m_SelectedItem.Content.Rows)
         {
            ccol = 0;
            foreach (PresentationColumnInfo c in i.Columns)
            {
               ElementItemInfo ce = m_SelectedItem.Find(c);
               GridItemInfo gi = new GridItemInfo
               {
                  Column = m_Node.GetLeaf(ce),
                  RowNo = crow,
                  ColumnNo = ccol,
                  ColumnSpan = c.ColumnSpan
               };
               m_Items.Add(gi);
               ccol += c.ColumnSpan;
            }
            crow++;
         }
      }

      private void PrepareGridDefinitions()
      {
         int cols;
         int maxColumns = 0;
         foreach (PresentationRowInfo i in m_SelectedItem.Content.Rows)
         {
            cols = 0;
            foreach (PresentationColumnInfo c in i.Columns)
            {
               cols += c.ColumnSpan;
            }
            if (cols > maxColumns)
            {
               maxColumns = cols;
            }
         }

         //m_Grid.Background = new SolidColorBrush(Colors.Beige);
         m_GridInfo.RowCount = m_SelectedItem.Content.Rows.Count;
         m_GridInfo.ColumnCount = maxColumns;
         m_Grid.Margin = ControlHelper.PrepareThickness(10,10,25,10);

         // add column definitions
         for (int c = 0; c < maxColumns; c++)
         {
            ColumnDefinition cd = new ColumnDefinition();
            m_Grid.ColumnDefinitions.Add(cd);
         }

         // add row definitions
         for (int r = 0; r < m_GridInfo.RowCount; r++)
         {
            RowDefinition rd = new RowDefinition();
            m_Grid.RowDefinitions.Add(rd);
         }
      }

      private async Task PrepareControls()
      {
         foreach (GridItemInfo c in m_Items)
         {
            MapInfo map = await m_ElementGroup.GetMapAsync(
               m_Node, c.Column.ColumnName);
            FrameworkElement control = ControlHelper.GetControl(
               c.Column, map, false);

            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            control.Margin = ControlHelper.PrepareThickness(2, 5);
            if (control.Height == double.NaN)
            {
               control.MaxHeight = ControlHelper.DEFAULT_HEIGHT;
            }

            m_Grid.Children.Add(control);
            Grid.SetColumn(control, c.ColumnNo);
            Grid.SetRow(control, c.RowNo);
            Grid.SetColumnSpan(control, c.ColumnSpan);
         }
      }

      public void PrepareModelData()
      {
         m_ModelData.Columns.Clear();
         foreach(GridItemInfo i in Items)
         {
            m_ModelData.Columns.Add(i.Column);
         }
      }

      public Grid PrepareForm(Grid grid = null)
      {
         m_Grid = grid == null ? new Grid() : grid;
         PrepareGridDefinitions();
         PrepareFields();
         PrepareControls();
         PrepareModelData();
         return m_Grid;
      }

      /// <summary>
      /// Make sure to dispose of any disposable item...
      /// </summary>
      public void Dispose()
      {
         if (m_Items != null && m_Items.Count > 0)
         {
            foreach(var i in m_Items)
            {
               i.Dispose();
            }
            m_Items.Clear();
         }
         if (m_Grid != null)
         {
            m_Grid.Children.Clear();
            m_Grid.Padding = new Thickness(0);
            m_Grid = null;
         }
      }

   }

}
