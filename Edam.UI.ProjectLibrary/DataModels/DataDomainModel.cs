using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.UI.Common.Controls.Dialogs;
using Edam.InOut;
using Edam.UI.Controls.DataModels;
using Edam.Application;
using Edam.DataObjects.Models;
using Edam.DataObjects.Services;
using Edam.Data.Asset;
using Edam.Data.AssetManagement;
using Edam.Data.AssetConsole;
using Edam.DataObjects.Dynamic;
using Edam.Serialization;

namespace Edam.UI.Controls.DataModels
{

   public class DataDomainModel : ObservableObject
   {

      #region -- 1.00 - Singleton Support

      private static DataDomainModel m_Instance { get; set; }
      public DataDomainModel Instance
      {
         get { return m_Instance; }
      }

      #endregion
      #region -- 1.00 - Properties

      private const string DOMAIN_RECORD_FORM = "Domain.Record.Form";
      private const string DOMAIN = "Domain";
      private const string DOMAIN_DETAILS = "Domain Asset Editor";

      private ReferenceDataService m_DataService =
         ReferenceDataService.GetService();

      private DataDomain m_DataDomain = new DataDomain();

      private string m_DomainUri;
      public string DomainUri
      {
         get { return m_DomainUri; }
         set
         {
            if (m_DomainUri != value)
            {
               m_DomainUri = value;
               OnPropertyChanged(nameof(DomainUri));
            }
         }
      }

      private ObservableCollection<DataDomain> m_Items;
      public ObservableCollection<DataDomain> Items
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

      public DataDomain SelectedItem
      {
         get { return m_DataDomain; }
         set
         {
            if (m_DataDomain != value)
            {
               m_DataDomain = value;
               OnPropertyChanged(nameof(SelectedItem));
            }
         }
      }

      #endregion
      #region -- 1.50 - Constructors and Initialization

      public DataDomainModel() : base()
      {
         if (m_Instance == null)
         {
            m_Instance = this;
         }

         Items = new ObservableCollection<DataDomain>();
         DomainUri = String.Empty;
         GetItems(DomainUri);
      }

      public static DataDomainModel GetInstance()
      {
         if (m_Instance == null)
         {
            m_Instance = new DataDomainModel();
         }
         return m_Instance;
      }

      #endregion
      #region -- 4.00 - Register Domain from Arguments

      /// <summary>
      /// Register Domain with given info...
      /// </summary>
      /// <param name="item">Project Item to register</param>
      /// <param name="jsonText">json document as text</param>
      public static void RegisterDomain(ProjectItem item, string jsonText)
      {
         if (item == null)
         {
            return;
         }
         // now if this is an args json file then register domain
         if (item.Item.IsJson)
         {
            var results = AssetConsoleArgumentsInfo.TryFromJson(jsonText);
            if (results.Success)
            {
               var instance = GetInstance();
               foreach(var d in instance.Items)
               {
                  if (d.DomainUri == results.Instance.NamespaceUri)
                  {
                     return;
                  }
               }

               // not found, so try to register it...
               DataDomain dmain = results.Instance.GetDataDomain();
               if (dmain.DomainUri == "http://tempuri")
               {
                  return;
               }
               instance.ShowDomainEditor(dmain);
            }
         }
      }

      #endregion
      #region -- 4.00 - Services (GET, POST and DELETE) Methods

      public async void GetItems(string domainUri)
      {
         Items.Clear();
         var results = await m_DataService.GetDomainData(Session.SessionId,
            Session.OrganizationId, domainUri);
         if (results != null && results.Success)
         {
            foreach (var i in results.ResponseData.Domains)
            {
               Items.Add(i);
            }
         }
      }

      public void GetItems()
      {
         GetItems(m_DomainUri);
      }

      public void Add(DataDomain domain)
      {
         foreach(var item in Items)
         {
            if (item.DomainUri == domain.DomainUri)
            {
               return;
            }
         }
         Items.Add(domain);
      }

      public async void PostItem()
      {
         var results = await m_DataService.UpdateDomainData(Session.SessionId,
            Session.OrganizationId, m_DataDomain);
         if (results != null && results.Success)
         {
            Add(m_DataDomain);
         }
      }

      //public async void DeleteItem()
      //{

      //}

      #endregion
      #region -- 4.00 - Domain Add/New Dialog Support Methods

      /// <summary>
      /// Process domain dialog results...
      /// </summary>
      /// <param name="result">results</param>
      private void ProcessDomainDialogResult(IDialogObjectInfo result)
      {
         var rslt = result as IDialogObjectInfo;
         if (rslt == null || String.IsNullOrWhiteSpace(rslt.CommandText))
         {
            return;
         }

         ElementNodeInfo node = result.DataObject as ElementNodeInfo;
         if (rslt.CommandText == DialogBox.SUBMIT && node != null &&
            node.Description == DOMAIN_DETAILS)
         {
            // fetch Domain info from editor results
            ModelExpandoObject.GetData<DataDomain>(result.Result, m_DataDomain);

            try
            {
               NamespaceInfo nsi = new NamespaceInfo(
                  m_DataDomain.Prefix, m_DataDomain.DomainUri);
               m_DataDomain.Root = nsi.NamePath.Root;
               m_DataDomain.Domain = nsi.NamePath.Domain;

               PostItem();
            }
            catch (Exception ex)
            {
               Session.ShowMessageBox("Fail Request", ex.Message);
            }
         }
      }

      /// <summary>
      /// Add a new Data Domain...
      /// </summary>
      public void ShowDomainEditor(DataDomain domain = null)
      {
         m_DataDomain.ClearFields();
         ElementNodeInfo node = ApplicationElementInfo.GetElementNode(
            DOMAIN_RECORD_FORM, DOMAIN, DOMAIN_DETAILS);

         if (domain != null)
         {
            node.SetItemValue(
               nameof(DataDomain.OrganizationId), domain.OrganizationId);
            node.SetItemValue(
               nameof(DataDomain.DataOwnerId), domain.DataOwnerId);
            node.SetItemValue(
               nameof(DataDomain.DomainId), domain.DomainId);
            node.SetItemValue(
               nameof(DataDomain.DomainName), domain.DomainName);
            node.SetItemValue(
               nameof(DataDomain.Prefix), domain.Prefix);
            node.SetItemValue(
               nameof(DataDomain.DomainUri), domain.DomainUri);
            node.SetItemValue(
               nameof(DataDomain.Root), domain.Root);
            node.SetItemValue(
               nameof(DataDomain.Domain), domain.Domain);
            node.SetItemValue(
               nameof(DataDomain.Description), domain.Description);
            node.SetItemValue(
               nameof(DataDomain.TypeNo), domain.TypeNo.ToString());
         }

         DialogBox.ShowDialog(node, DOMAIN_DETAILS,
            ProcessDomainDialogResult, DialogBox.SUBMIT);
      }

      #endregion

   }

}
