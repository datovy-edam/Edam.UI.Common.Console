using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.Data.Books;
using Edam.UI.Controls.Booklets;
using Edam.UI.Controls.ViewModels;
using System.Collections.ObjectModel;

namespace Edam.UI.Controls.DataModels
{

   /// <summary>
   /// This class manage the inner Book and provide helpers to manage UI 
   /// resources and controls.
   /// </summary>
   public class BookModel : ObservableObject
   {

      #region -- 1.00 - Properties and Fields declarations

      private DataMapContext m_Context;
      public DataMapContext Context
      {
         get { return m_Context; }
      }

      /// <summary>
      /// Items show all added booklets and (code and text) cells...
      /// </summary>
      public ListView ListView
      {
         get { return m_Context.BookletViewList; }
      }

      private ObservableCollection<IBookCellView> m_Items;
      public ObservableCollection<IBookCellView> Items
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

      private BookInfo m_Book;
      public BookInfo Book
      {
         get { return m_Book; }
      }

      public BookletInfo SelectedBooklet
      {
         get { return Book.SelectedBooklet; }
         set 
         { 
            Book.SelectedBooklet = value;
         }
      }

      #endregion
      #region -- 1.50 - Constructure

      public BookModel(DataMapContext context)
      {
         if (context.UseCase.Book == null)
         {
            throw new Exception(
               "Expected an instance of a BookInfo null was found");
         }
         m_Book = context.UseCase.Book;
         m_Context = context;
         Items = new ObservableCollection<IBookCellView>();
      }

      #endregion
      #region -- 4.00 - Book Booklet and Cells support

      /// <summary>
      /// Find a booklet... by booklet ID
      /// </summary>
      /// <param name="bookletId">booklet ID to find</param>
      /// <returns>returns instance of BookletInfo if found, else null</returns>
      public BookletInfo FindBooklet(string bookletId)
      {
         return Book.Find(bookletId);
      }

      /// <summary>
      /// Clear all adquired resources including the ListView entries and Book/
      /// Booklet/Cell inner Map Items and Controls...
      /// </summary>
      public void ClearAll()
      {
         Items.Clear();
         if (ListView != null)
         {
            ListView.Items.Clear();
         }
         //foreach(var booklet in Book.Items)
         //{
         //   booklet.Items.Clear();
         //}
         //Book.Items.Clear();
      }

      /// <summary>
      /// Add booklet cell control to currently viewed map-item.
      /// </summary>
      /// <param name="model">instance of BookViewModel</param>
      /// <param name="cellType">cell type (code or text)</param>
      /// <param name="referenceId">parent map-item reference id</param>
      /// <param name="bookletCell">(optional) booklet cell</param>
      /// <returns>booklset cell as added</returns>
      public BookletCellInfo AddControl(
         BookViewModel model, BookletCellType cellType, string referenceId,
         BookletCellInfo bookletCell = null)
      {
         if (SelectedBooklet == null)
         {
            SelectedBooklet = new BookletInfo();
         }

         BookletCellInfo cell = bookletCell ?? new BookletCellInfo
         {
            BookletId = SelectedBooklet.BookletId,
            CellType = cellType,
            ReferenceId = referenceId,
            TextType = cellType == BookletCellType.Text ?
               BookletTextType.Markdown : BookletTextType.JSONata
         };

         SelectedBooklet.SelectedCell = cell;

         if (bookletCell == null)
         {
            SelectedBooklet.Items.Add(cell);
         }

         CellViewModel cellModel = new CellViewModel();
         cellModel.ViewModel = model;

         IBookCellView control = null;
         switch(cellType)
         {
            case BookletCellType.Code:
               var cctrl = new BookletCodeCellControl
               {
                  ViewModel = cellModel,
                  Tag = cell
               };
               cctrl.FramePanel.Tag = cell;
               cctrl.SetInputText(
                  Context.LanguageInstance.GetDefaultEmptyCodeText());
               cctrl.SetCell(cell);
               control = cctrl;
               cellModel.BaseControl = cctrl;
               break;
            case BookletCellType.Text:
            default:
               var tctrl = new BookletTextCellControl
               {
                  ViewModel = cellModel,
                  Tag = cell
               };
               tctrl.FramePanel.Tag = cell;
               tctrl.SetCell(cell);
               control = tctrl;
               cellModel.BaseControl = tctrl;
               break;
         }

         if (control != null)
         {
            ListView.Items.Add(control);
            //Items.Add(control);
            cell.Instance = control;
         }

         var itm = Book.Items.Find(
            (x) => x.BookletId == SelectedBooklet.BookletId);
         if (itm == null)
         {
            Book.Items.Add(SelectedBooklet);
         }

         return cell;
      }

      #endregion

   }

}
