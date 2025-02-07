using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.Data.AssetConsole;
using Edam.Data.AssetSchema;
using Edam.InOut;
using Edam.Data.Asset;

namespace Edam.UI.Controls.DataModels
{

   public static class ProjectContext
   {

      #region -- 1.00 - fields and properties

      public static ProjectItem TreeView { get; set; }

      private static AssetConsoleArgumentsInfo m_Arguments = null;
      public static AssetConsoleArgumentsInfo Arguments
      {
         get { return m_Arguments; }
      }

      private static AssetDataList m_AssetDataSet = null;
      public static AssetDataList AssetDataSet
      {
         get { return m_AssetDataSet; }
      }

      // Current Asset
      public static AssetType AssetType { get; set; }
      private static AssetData m_AssetData = null;
      public static AssetData AssetData
      {
         get { return m_AssetData; }
      }

      private static ProjectItem m_CurrentProject = null;
      public static ProjectItem CurrentProject
      {
         get { return m_CurrentProject; }
      }

      public static string ProjectFolderPath
      {
         get { return m_CurrentProject.GetProjectFolder(); }
      }

      public static NamespaceInfo CurrentNamespace
      {
         get { return CurrentProject.Namespace; }
      }

      public static AssetConsoleArgumentsInfo GetArguments()
      {
         if (m_CurrentProject == null)
         {
            return null;
         }
         return m_CurrentProject.CurrentArguments;
      }

      public static void SetAssetDataSet(AssetDataList dataSet)
      {
         m_AssetDataSet = dataSet;
      }

      /// <summary>
      /// Set Project and Reset Arguments...
      /// </summary>
      /// <param name="item">project item to be set</param>
      public static void SetProjectAndResetArguments(ProjectItem item)
      {
         m_CurrentProject = item;
         if (m_CurrentProject != null)
         {
            m_Arguments = m_CurrentProject.CurrentArguments;
            if (m_Arguments.AssetDataItems == null)
            {
               m_Arguments.AssetDataItems = new AssetDataList();
            }

            if (m_Arguments != null && m_Arguments.AssetDataItems.Count > 0)
            {
               m_AssetData = m_Arguments.AssetDataItems[0];
            }
         }
      }

      /// <summary>
      /// A project has been selected, set it as current project...
      /// </summary>
      /// <param name="item">project item to be set</param>
      public static void SetSelectedProject(ProjectItem item)
      {

      }

      #endregion
      #region -- 4.00 - Get / Find and Prepare Arguments 

      public static void PrepareArguments(AssetConsoleArgumentsInfo args)
      {
         if (args == null || args.HasAssets)
         {
            return;
         }

         var results = ProjectConsole.Execute(CurrentProject.Item, args, null);
         if (!results.Success)
         {

         }
      }

      /// <summary>
      /// Get (JSON) Arguments from file.
      /// </summary>
      /// <param name="filePath">file path name</param>
      /// <returns>instance of found args for process is returned if found
      /// </returns>
      private static AssetConsoleArgumentsInfo GetArguments(string filePath)
      {
         if (filePath.EndsWith(".Args.json"))
         {
            return AssetConsoleArgumentsInfo.FromJsonFilePath(filePath);
         }
         return null;
      }

      /// <summary>
      /// Get Arguments by Process name.
      /// </summary>
      /// <param name="item">folder item to investigate</param>
      /// <param name="name">name of process name to find</param>
      /// <returns>instance of found args for process is returned if found
      /// </returns>
      public static AssetConsoleArgumentsInfo GetArgumentsByProcessName(
         ItemBaseInfo item, string name)
      {
         if (item.Children != null && item.Children.Count > 0)
         {
            foreach (var f in item.Children)
            {
               if (f.Name == "Arguments")
               {
                  if (f.Children != null && f.Children.Count > 0)
                  {
                     foreach (var i in f.Children)
                     {
                        var args = GetArguments(i.Full);
                        if (args != null)
                        {
                           if (args.Process.Name == name)
                           {
                              return args;
                           }
                        }
                     }
                  }
               }
            }
         }
         return null;
      }

      /// <summary>
      /// Get Arguments by Process Name
      /// </summary>
      /// <param name="name">process name</param>
      /// <returns>instance of found args for process is returned if found
      /// </returns>
      public static AssetConsoleArgumentsInfo GetArgumentsByProcessName(
         string name)
      {
         AssetConsoleArgumentsInfo args = null;

         // try to find given name in current project
         if (CurrentProject.Parent != null)
         {
            args = GetArgumentsByProcessName(CurrentProject.Parent, name);
            if (args != null)
            {
               PrepareArguments(args);
               return args;
            }
         }

         // try to find given name in all projects
         ProjectItem item = TreeView;
         if (item.Item.Name != "Projects")
         {
            return null;
         }
         foreach(var c in item.Children)
         {
            args = GetArgumentsByProcessName(c.Item, name);
            if (args != null)
            {
               break;
            }
         }

         PrepareArguments(args);
         return args;
      }

      #endregion

   }

}
