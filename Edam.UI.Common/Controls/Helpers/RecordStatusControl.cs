using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.DataObjects.DataCodes;
using Edam.Application;
using Edam.DataObjects.Models;

namespace Edam.WinUI.Controls.Helpers
{

   public class RecordStatusControl : ObservableObject
   {
      public const string RECORD_STATUS_CODE = "RecordStatusCode";
      public const string RECORD_STATUS_TITLE = "Record Status";
      public const string RECORD_STATUS_ITEMS = "RecordStatusItems";
      public const string SELECTED_RECORD_STATUS_CODE =
         "SelectedRecordStatusCode";

      public ComboBox ComboBoxControl { get; set; }

      private string m_Title;
      public string Title
      {
         get { return m_Title; }
         set
         {
            if (m_Title != value)
            {
               m_Title = value;
               OnPropertyChanged("Title");
            }
         }
      }

      private ObservableCollection<DataCodeInfo> m_RecordStatusCodes;
      public ObservableCollection<DataCodeInfo> Items
      {
         get { return m_RecordStatusCodes; }
         set
         {
            if (m_RecordStatusCodes != value)
            {
               m_RecordStatusCodes = value;
               OnPropertyChanged(RecordStatusControl.RECORD_STATUS_ITEMS);
            }
         }
      }

      private DataCodeInfo m_SelectedRecordStatusCode;
      public DataCodeInfo SelectedItem
      {
         get { return m_SelectedRecordStatusCode; }
         set
         {
            if (m_SelectedRecordStatusCode != value)
            {
               m_SelectedRecordStatusCode = value;
               OnPropertyChanged(RecordStatusControl.SELECTED_RECORD_STATUS_CODE);
            }
         }
      }

      public FrameworkElement GetControl(ModelColumnInfo column)
      {
         column.EditControlType = ModelColumnControlType.CodeValue;
         column.LookUpItems = Items;
         return ComboBoxControl;
      }

      /// <summary>
      /// Set Record Status Items...
      /// </summary>
      public static ObservableCollection<DataCodeInfo> GetRecordStatusItems(
         ObservableCollection<DataCodeInfo> items = null)
      {
         var l = CacheHelper.GetRecordStatusCodes();
         ObservableCollection<DataCodeInfo> codes =
            items ?? new ObservableCollection<DataCodeInfo>();
         foreach (var i in l)
         {
            codes.Add(i);
         }
         return codes;
      }

   }

}
