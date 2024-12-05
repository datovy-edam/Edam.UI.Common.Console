using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.DataObjects.References;
using Edam.UI.Common.Menus;

namespace Edam.UI.Common.Controls.ReferenceLists
{

   public class ReferenceListGroupModel : ObservableObject, IMenuView
   {

      #region -- 1.00 - Properties and definitions...

      public String DefaultReferenceId = String.Empty;
      private ReferenceListGroupInfo m_Group;
      public ReferenceListGroupInfo Record
      {
         get { return m_Group; }
         set { m_Group = value ?? new ReferenceListGroupInfo(); }
      }

      private Boolean hasReferrence
      {
         get { return !String.IsNullOrWhiteSpace(ReferenceId); }
      }

      public Int16 GroupNo
      {
         get
         {
            return m_Group.GroupNo.HasValue ? m_Group.GroupNo.Value : (Int16)0;
         }
         set
         {
            if (m_Group.GroupNo != value)
            {
               m_Group.GroupNo = value;
               OnPropertyChanged(nameof(GroupNo));
            }
         }
      }

      public String ReferenceId
      {
         get { return m_Group.ReferenceId; }
         set
         {
            if (m_Group.ReferenceId != value)
            {
               m_Group.ReferenceId = value;
               OnPropertyChanged(nameof(ReferenceId));
            }
         }
      }

      public bool ItemChecked
      {
         get { return !String.IsNullOrWhiteSpace(m_Group.ReferenceId); }
         set
         {
            m_Group.ReferenceId = value ? DefaultReferenceId : String.Empty;
            OnPropertyChanged("ItemChecked");
            OnPropertyChanged("TextColor");
         }
      }
      
      public Int16 ListNo
      {
         get
         {
            return m_Group.ListNo.HasValue ? m_Group.ListNo.Value : (Int16)0;
         }
         set
         {
            if (m_Group.ListNo != value)
            {
               m_Group.ListNo = value;
               OnPropertyChanged(nameof(ListNo));
            }
         }
      }

      public String Name
      {
         get { return m_Group.Name; }
         set
         {
            if (m_Group.Name != value)
            {
               m_Group.Name = value;
               OnPropertyChanged(nameof(Name));
            }
         }
      }

      public String CommentText
      {
         get { return m_Group.Comment; }
         set
         {
            if (m_Group.Comment != value)
            {
               m_Group.Comment = value;
               OnPropertyChanged(nameof(CommentText));
            }
         }
      }

      public String TextColor
      {
         get
         {
            return (hasReferrence) ? "#000000" : "#C7C7C7";
         }
         set
         {
            OnPropertyChanged("TextColor");
         }
      }

      public IMenuItemParent ParentMenu { get; set; }

      #endregion
      #region -- 1.50 - Initialize Resources

      public ReferenceListGroupModel(ReferenceListGroupInfo group = null)
      {
         m_Group = group ?? new ReferenceListGroupInfo();
      }

      #endregion
      #region -- 2.00 - MVVM Commands

      private void InitializeCommands()
      {
      }

      #endregion
      #region -- 4.00 - Support Menus

      public void SetState(object state)
      {

      }

      #endregion

   }

}
