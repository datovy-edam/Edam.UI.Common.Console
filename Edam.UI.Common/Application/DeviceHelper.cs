using System;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.Diagnostics;
using Edam.DataObjects.Devices;
using Edam.Data;
using Edam.Tasks;
using Edam.Application;

namespace Edam.UI.Common.Application
{

   public class DeviceDataHelper
   {
      public static readonly int DEFAULT_WAIT_TIMEOUT = 500; // milliseconds
      public static readonly string CONFIG_DEFAULT_ORGANIZATION_ID = 
         "DefaultOrganizationId";

      public static DeviceUserViewModel GetDeviceUser()
      {
         DeviceUserViewModel u = new DeviceUserViewModel();
         u.OrganizationId = AppSettings.GetString(
            CONFIG_DEFAULT_ORGANIZATION_ID);
         u.OrganizationName = "";
         u.UserEmail = "";
         u.UserName = "";
         
         return u;
      }

      /// <summary>
      /// Initialize device - user by fetching known info about both and 
      /// inserting the record in the local database.
      /// </summary>
      /// <param name="connection">Database connection.</param>
      /// <param name="results">(optional) results if you like to receive
      /// task result details back</param>
      public static async void InitializeDeviceUser(
         LDbConnection connection, 
         ResultsLog<List<DeviceUserViewModel>> results = null)
      {
         if (results == null)
            results = new ResultsLog<List<DeviceUserViewModel>>();

         DeviceUserViewModel u = GetDeviceUser();
         DeviceInfo d = new DeviceInfo();
         u.DeviceId = ""; // Acr.DeviceInfo.DeviceInfo.Hardware.DeviceId;
         u.DeviceName = u.DeviceId;
         u.IpAddress = ""; // Acr.DeviceInfo.DeviceInfo.Connectivity.IpAddress;

         results.Instance = new List<DeviceUserViewModel>();
         results.Instance.Add(u);
         results.Succeeded();

         // try to insert record...
         int count = await connection.InsertAsync<DeviceUserViewModel>(u);

         if (count == 0)
         {
            results.Failed(
               "DeviceHelper: Failed to initialize (DB Insert) Device User");
         }
      }

      /// <summary>
      /// Call to Initialize the Database for the first time
      /// </summary>
      /// <param name="worker">(optional) worker</param>
      public static async void InitializeDb(
         IWorker<List<DeviceUserViewModel>> worker = null)
      {
         // connect to the database
         LDbConnection.InitializeDb();
         ResultsLog<List<DeviceUserViewModel>> results = 
            new ResultsLog<List<DeviceUserViewModel>>();
         LDbConnection c = new LDbConnection();

         // try to fetch device (primary) user...
         SQLite.CreateTableResult tblResults;
         try
         {
            // try to create database (will be if not already there...
            tblResults = await c.CreateTableAsync<DeviceUserViewModel>();
            //if (tblResults.Count > 0)
            {
               // fetch all device users...
               List<DeviceUserViewModel> l = 
                  await c.GetItemsAsync<DeviceUserViewModel>();
               if (l != null)
               {
                  if (l.Count == 0)
                  {
                     InitializeDeviceUser(c, results);
                  }
                  else
                  {
                     results.Instance = l;
                     results.Succeeded();
                  }
               }
            }
            if (worker != null)
            {
               worker.Data = results.Instance;
               worker.Results = results;
               worker.Done();
            }
            else
               results.Succeeded();
         }
         catch(Exception ex)
         {
            results.Failed(ex);
         }
      }

      /// <summary>
      /// Initialize the Device...
      /// </summary>
      /// <remarks>this initialization may depend or use App.Config
      /// key/values therefore must be initialize in each indipendent </remarks>
      /// <param name="worker"></param>
      public static void InitializeDevice(
         IWorker<List<DeviceUserViewModel>> worker = null)
      {
         InitializeDb(worker);
         //Edam.DataObjects.Services.EchoService.GetEcho();  // test WebApi
      }

   }

}
