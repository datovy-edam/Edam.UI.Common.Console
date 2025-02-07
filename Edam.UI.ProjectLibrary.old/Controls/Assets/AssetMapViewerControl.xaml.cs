using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
using Edam.UI.Controls.ViewModels;
using Edam.UI.Controls.DataModels;
using Edam.UI.Common;

namespace Edam.UI.Controls.Assets
{

   /// <summary>
   /// Map Viewer provide support to define mappings, definitions, instructions,
   /// or transformations within a Book/Booklet format.
   /// </summary>
   public sealed partial class AssetMapViewerControl : UserControl
   {

      private AssetMapViewerModel m_ViewModel = new AssetMapViewerModel();
      public AssetMapViewerModel ViewModel
      {
         get { return m_ViewModel; }
      }

      public AssetMapViewerControl()
      {
         this.InitializeComponent();
         DataContext = m_ViewModel;
      }

      /// <summary>
      /// Manage Map Viewing prividing a map context.
      /// </summary>
      /// <remarks>
      /// See AssetSidePanelControl view-model notification.
      /// </remarks>
      /// <param name="sender">notification sender</param>
      /// <param name="args">notification arguments whose EventData should had
      /// an object specifying the data Map Context</param>
      public void ManageNotification(
         object sender, NotificationArgs args)
      {
         if (args.MessageText != AssetViewOption.DataMapView.ToString())
         {
            return;
         }

         DataMapContext context =
            args.EventData as DataMapContext;
         if (context == null)
         {
            return;
         }

         if (m_ViewModel.MapContext != null &&
            context.ContextId != m_ViewModel.MapContext.ContextId)
         {
            m_ViewModel.MapContext.ClearAll();
         }

         m_ViewModel.SetUpMapping(context);
         AssetDataTreeViewer.ManageNotification(this, args);
         MapPlayControl.SetMapContext(context);
         MapPanelControl.SetContext(context);
      }

      /// <summary>
      /// Tab selection changed
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void TabViewer_SelectionChanged(
         object sender, SelectionChangedEventArgs e)
      {
         if (e.AddedItems.Count == 0)
         {
            return;
         }
         var tabItem = e.AddedItems[0] as TabViewItem;
         //ViewModel.NotifyTabChanged(tabItem.Name, tabItem);
      }

   }

}
