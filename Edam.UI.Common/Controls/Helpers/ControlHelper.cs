using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Uno.UI.Toolkit;

// -----------------------------------------------------------------------------
using Edam.DataObjects.Models;
using Edam.DataObjects.DataCodes;
using Edam.DataObjects.Objects;
using Edam.DataObjects.Dynamic;

namespace Edam.UI.Common.Controls.Helpers
{

   public class ControlHelper
   {

      #region -- 1.00 - Fields and Properties

      public const int DEFAULT_HEIGHT = 32;
      public const int EDITOR_WIDTH = 380;
      public const string UTC_FORMAT_SPECIFIER = "u";

      #endregion
      #region -- 4.00 - Prepare Controls

      //public static MarkdownTextBlock PrepareMarkdownTextBlock()
      //{
      //   MarkdownTextBlock tblock = new MarkdownTextBlock();
      //   tblock.HorizontalAlignment = HorizontalAlignment.Stretch;
      //   tblock.VerticalAlignment = VerticalAlignment.Stretch;
      //   return tblock;
      //}

      public static Thickness PrepareThickness(
         double? left = null, double? top = null,
         double? right = null, double? bottom = null)
      {
         Thickness t = new Thickness
         {
            Left = left ?? 0.0,
            Top = top ?? 0.0,
            Right = right ?? (left ?? 0.0),
            Bottom = bottom ?? (top ?? 0.0)
         };
         return t;
      }

      public static Button PrepareButton(
         string text, RoutedEventHandler onClick, bool setDimensions = true)
      {
         Button b = new Button();
         b.Content = text;
         if (setDimensions)
         {
            b.Margin = PrepareThickness(5, 10, 5, 0);
            b.Width = 70.0;
         }
         b.Click += onClick;
         return b;
      }

      public static void PrepareToolTip(
         UIElement control, ModelColumnInfo column)
      {
         string tip = column.Title;
         if (column.Element != null)
         {
            tip = String.IsNullOrEmpty(column.Element.Description) ?
               tip : column.Element.Description;
            if (!String.IsNullOrEmpty(tip))
            {
               ToolTip toolTip = new ToolTip
               {
                  Content = tip
               };
               ToolTipService.SetToolTip(control, toolTip);
            }
         }
      }

      public static TextBox PrepareTextBox(
         ModelColumnInfo column, bool setDimensions = true)
      {
         TextBox textBox = new TextBox();
         if (setDimensions)
         {
            textBox.Margin = PrepareThickness(5, 10, 0, 0);
            textBox.Width = EDITOR_WIDTH - 15;
         }

         PrepareToolTip(textBox, column);

         textBox.Name = column.ColumnName;
         textBox.Header = column.Title;
         if (column.Visible)
            textBox.PlaceholderText = "Enter " + column.Title; ;
         textBox.IsEnabled = column.Visible;
         textBox.IsTabStop = true;
         textBox.IsTapEnabled = true;
         textBox.HorizontalAlignment = HorizontalAlignment.Left;

         if (column.Element != null && column.Element.MaxLength > 100)
         {
            textBox.MinHeight = 100;
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.AcceptsReturn = false;
            textBox.MaxLength = column.Element.MaxLength;
         }
         column.EditControl = textBox;
         column.EditControlType = ModelColumnControlType.Text;
         return textBox;
      }

      public static PasswordBox PreparePasswordBox(
         ModelColumnInfo column, bool setDimensions = true)
      {
         PasswordBox passwordBox = new PasswordBox();
         if (setDimensions)
         {
            passwordBox.Margin = PrepareThickness(5, 10, 0, 0);
            passwordBox.Width = EDITOR_WIDTH - 15;
         }

         PrepareToolTip(passwordBox, column);

         passwordBox.Name = column.ColumnName;
         passwordBox.Header = column.Title;
         if (column.Visible)
            passwordBox.PlaceholderText = "Enter " + column.Title; ;
         passwordBox.IsEnabled = column.Visible;
         passwordBox.IsTabStop = true;
         passwordBox.IsTapEnabled = true;
         passwordBox.HorizontalAlignment = HorizontalAlignment.Left;

         column.EditControl = passwordBox;
         column.EditControlType = ModelColumnControlType.Password;
         return passwordBox;
      }

      public static DatePicker PrepareDatePicker(
         ModelColumnInfo column, bool setDimensions = true)
      {
         DatePicker uie = new DatePicker();
         if (setDimensions)
         {
            uie.Margin = PrepareThickness(5, 10, 0, 0);
         }
         uie.Header = column.Title;
         uie.HorizontalAlignment = HorizontalAlignment.Left;
         column.EditControl = uie;
         column.EditControlType = ModelColumnControlType.Date;
         return uie;
      }

      public static ComboBox PrepareComboBox(
         ModelColumnInfo column, List<DataCodeInfo> codes, 
         bool setDimensions = true)
      {
         if (column.EditControl != null)
         {
            return column.EditControl as ComboBox;
         }

         ComboBox control = new ComboBox
         {
            Header = column.Title,
            IsEditable = true
         };
         column.LookUpItems = new ObservableCollection<DataCodeInfo>();
         foreach (DataCodeInfo i in codes)
         {
            column.LookUpItems.Add(i);
            control.Items.Add(i);
         }

         if (setDimensions)
         {
            control.Margin = PrepareThickness(5, 10, 0, 0);
            control.Width = EDITOR_WIDTH - 15;
         }

         control.DisplayMemberPath = DataCodeInfo.DESCRIPTION;
         control.HorizontalAlignment = HorizontalAlignment.Left;

         column.EditControl = control;
         column.EditControlType = ModelColumnControlType.CodeValue;
         return control;
      }

      public static FrameworkElement PrepareStackControl(ModelData model)
      {
         StackPanel sp = new StackPanel();
         foreach (var i in model.Columns)
         {
            if (i.Visible)
            {
               sp.Children.Add((FrameworkElement)i.EditControl);
            }
         }
         return sp;
      }

      #endregion
      #region -- 4.00 - Setup Controls Data

      /// <summary>
      /// Set ComboBox Value.
      /// </summary>
      /// <param name="column">Column information</param>
      /// <param name="data">data to set (if any)</param>
      public static void SetComboBox(ModelColumnInfo column, object data)
      {
         ComboBox control = column.EditControl as ComboBox;
         if (control != null)
         {
            if (data == null || column.LookUpItems == null)
            {
               control.SelectedItem = null;
               return;
            }

            DataCodeInfo item = null;
            string value = data.ToString();
            foreach (var i in column.LookUpItems)
            {
               if (i.CodeId == value)
               {
                  item = i;
                  break;
               }
            }
            control.SelectedItem = item == null ? null : item;
         }
      }

      /// <summary>
      /// Set DatePicker Value.
      /// </summary>
      /// <param name="column">Column information</param>
      /// <param name="data">data to set (if any)</param>
      public static void SetDatePicker(ModelColumnInfo column, object data)
      {
         if (column.EditControl is DatePicker control)
         {
            string dtString = data?.ToString();
            DateTime? dt = null;

            if (!String.IsNullOrWhiteSpace(dtString) &&
               DateTime.TryParse(dtString, out DateTime outDt))
               dt = outDt;

            control.SelectedDate = dt.HasValue ?
               new DateTimeOffset?(dt.Value) : null;
         }
      }

      /// <summary>
      /// Set TextBox Value.
      /// </summary>
      /// <param name="column">Column information</param>
      /// <param name="data">data to set (if any)</param>
      public static void SetTextBox(ModelColumnInfo column, object data)
      {
         TextBox control = column.EditControl as TextBox;
         if (control != null)
         {
            control.Text = data == null ? String.Empty : data.ToString();
         }
      }

      /// <summary>
      /// Setup Editor Control based on given (dynamic) row usually be the same
      /// as the last "SelectedRow"...
      /// </summary>
      /// <remarks>
      /// The ElementItemInfo values will be use to set the controls values that
      /// will be presented.  See GetControlsData countepart.
      /// </remarks>
      /// <param name="row">ExpandoObject as a dynamic instance</param>
      public static void SetControlsData(
         dynamic row, List<ModelColumnInfo> items)
      {
         ModelExpandoObject rowData = new ModelExpandoObject(row);
         foreach (var c in items)
         {
            var data = rowData.GetValue(c.ColumnName);
            switch (c.EditControlType)
            {
               case ModelColumnControlType.CodeValue:
                  ControlHelper.SetComboBox(c, data);
                  break;
               case ModelColumnControlType.Date:
                  ControlHelper.SetDatePicker(c, data);
                  break;
               default:
                  ControlHelper.SetTextBox(c, data);
                  break;
            }
         }
      }

      public static void SetControlsDataValue(
         dynamic row, List<ModelColumnInfo> items)
      {
         ModelExpandoObject rowData = new ModelExpandoObject(row);
         foreach (var c in items)
         {
            switch (c.EditControlType)
            {
               case ModelColumnControlType.CodeValue:
                  ControlHelper.SetComboBox(c, c.ElementValue.ValueText);
                  break;
               case ModelColumnControlType.Date:
                  ControlHelper.SetDatePicker(c, c.ElementValue.ValueText);
                  break;
               default:
                  ControlHelper.SetTextBox(c, c.ElementValue.ValueText);
                  break;
            }
         }
      }

      #endregion
      #region -- 4.00 - Get Data

      /// <summary>
      /// Prepare dynamic row using controls data... fetching the updated values
      /// from the controls.
      /// </summary>
      /// <remarks>
      /// See SetControlsData countepart.
      /// </remarks>
      /// <returns>true if any data element has changed</returns>
      public static dynamic GetControlsData(dynamic currentRow, 
         List<ModelColumnInfo> items, out int changedCount)
      {
         changedCount = 0;
         if (currentRow == null)
         {
            return null;
         }

         ModelExpandoObject row = new ModelExpandoObject(currentRow);
         foreach (ModelColumnInfo c in items)
         {
            if (c.Element == null)
            {
               continue;
            }

            switch (c.EditControlType)
            {
               case ModelColumnControlType.CodeValue:
                  if (c.EditControl is ComboBox comboBox &&
                     comboBox.SelectedItem != null)
                  {
                     DataCodeInfo codeInfo = 
                        comboBox.SelectedItem as DataCodeInfo;
                     string codeValue = codeInfo?.CodeId.ToString();
                     row.SetValue(c.ColumnName, codeValue);
                     c.ElementValue.ValueText = codeValue;
                  }
                  break;
               case ModelColumnControlType.Date:
                  if (c.EditControl is DatePicker datePicker &&
                     datePicker.SelectedDate != null)
                  {
                     // TODO: replate hardcoded "u" ...
                     string dtValue = datePicker.SelectedDate.HasValue ?
                        string.Empty :
                        datePicker.SelectedDate.Value.ToString(
                           UTC_FORMAT_SPECIFIER);
                     row.SetValue(c.ColumnName, dtValue);
                     c.ElementValue.ValueText = dtValue;
                  }
                  break;
               case ModelColumnControlType.Password:
                  string secretValue = 
                     !(c.EditControl is PasswordBox passwordBox) ?
                     string.Empty : (passwordBox.Password ?? String.Empty);
                  row.SetValue(c.ColumnName, secretValue);
                  c.ElementValue.ValueText = secretValue;
                  break;
               default:
                  string textValue = !(c.EditControl is TextBox textBox) ?
                     string.Empty : (textBox.Text ?? String.Empty);
                  row.SetValue(c.ColumnName, textValue);
                  c.ElementValue.ValueText = textValue;
                  break;
            }
            if (c.ElementValue.ChangedIndicator)
            {
               changedCount++;
            }
         }
         return row.Instance;
      }

      #endregion
      #region -- 4.00 - Manage Control

      /// <summary>
      /// Get Control base on the column characteristics...
      /// </summary>
      /// <param name="column">Column to base control from</param>
      /// <returns>returns the control needed for the column</returns>
      public static FrameworkElement GetControl(
         ModelColumnInfo column, MapInfo map, bool setDimensions = true)
      {
         column.EditControlType = ModelColumnControlType.Text;

         // is this a combo box?  if so prepare it and return
         if (map != null && map.Link.Count > 0)
         {
            return PrepareComboBox(column, map.LinkCodes, setDimensions);
         }
         else if (column.Element != null && column.Element.LinkCodes != null &&
            column.Element.LinkCodes.Count > 0)
         {
            return PrepareComboBox(
               column, column.Element.LinkCodes, setDimensions);
         }

         // manage other column types...
         if (column == null || column.Element == null)
         {
            return PrepareTextBox(column, setDimensions);
         }
         switch (column.ElementValue.ValueType)
         {
            case ObjectValueType.Date:
               column.EditControlType = ModelColumnControlType.Date;
               return PrepareDatePicker(column, setDimensions);
            case ObjectValueType.SecretText:
               return PreparePasswordBox(column, setDimensions);
            default:
               return PrepareTextBox(column, setDimensions);
         }
      }

      #endregion
      #region -- 4.00 - Manage ModelData 

      /// <summary>
      /// Initialize Model Data... using model columns adding associated 
      /// controls and setting values.
      /// </summary>
      /// <param name="model">instance of model data</param>
      /// <returns>a StackPanel control is returned containing all child 
      /// controls that implements the model editor</returns>
      public static object GetEditorContent(ModelData model)
      {
         // add Dialog Context with needed controls defined in the ModelData
         model.ModelObject = ModelExpandoObject.New(model.Columns);
         ControlHelper.PrepareControls(model);

         // set the values in the model data into the controls
         ControlHelper.SetControlsData(model.ModelObject, model.Columns);

         return ControlHelper.PrepareStackControl(model);
      }

      /// <summary>
      /// Get Controls...
      /// </summary>
      /// <param name="model"></param>
      public static void PrepareControls(ModelData model)
      {
         foreach(var i in model.Columns)
         {
           i.EditControl = GetControl(i, null);
         }
      }

      /// <summary>
      /// Release resources...
      /// </summary>
      /// <param name="model">instance of ModelData</param>
      public static void Dispose(ModelData model)
      {
         if (model != null)
         {
            foreach(var i in model.Columns)
            {
               i.Dispose();
            }
         }
      }

      #endregion

   }

}
