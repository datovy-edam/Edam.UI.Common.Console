using System;
using System.Collections.Generic;

// -----------------------------------------------------------------------------
using Edam.DataObjects.ReferenceData;
using Edam.Data.Asset.Services;
using Edam.Data.AssetConsole;
using Edam.Data.AssetSchema;
using Edam.Data.Asset;
using Edam.Data.AssetManagement.Resources;
using Edam.Data.AssetConsole.Services;
using Edam.Diagnostics;
using Edam.Data.AssetProject;
using Edam.Application;
using Edam.UI.Common.DataModels;
using Edam.Data.AssetReference.ReferenceData;

namespace Edam.UI.Controls.DataModels;


public class ProjectHelper
{
  public const string DEFAULT_TEXTMAP_FOLDER = "DefaultTextMapFolder";
  public const string DEFAULT_TEXTMAP_FILENAME = "TextMap.json";

  private static int m_OutputCounter = 0;

  /// <summary>
  /// Get Text Map file path...
  /// </summary>
  /// <param name="args"></param>
  /// <returns></returns>
  public static string GetTextMapPath(AssetConsoleArgumentsInfo args)
  {
     string folder = null;
     string path = null;

     // see if path is specified in args...
     if (args == null)
     {
        folder = args.TextMapFolderPath;
        if (folder.IndexOf("{0}") != -1)
        {
           path = folder.Replace("{0}", args.OutFileExtension);
        }
        else
        {
           folder = null;
        }
     }

     // see if path is specified in app-settings... should end with '/'...
     if (folder == null)
     {
        folder = AppSettings.GetSectionString(DEFAULT_TEXTMAP_FOLDER);
        if (folder != null)
        {
           path = folder + args.OutFileExtension + DEFAULT_TEXTMAP_FILENAME;
        }
     }

     return path;
  }

  /// <summary>
  /// Prepare Excel File
  /// </summary>
  /// <param name="item"></param>
  public static void PrepareOutputFile(
     ProjectItem item, AssetConsoleProcedure procedure)
  {
     var aresults = ProjectConsole.GetArgsContext(item.Item);
     if (!aresults.Success)
     {
        // TODO: output issues found to the app log here
        return;
     }

     // make sure to get a copy of args since we will change some stuff...
     var args = AssetConsoleArgumentsInfo.Duplicate(
        aresults.Instance.Arguments);

     args.Procedure = AssetConsoleProcedure.ToFile;
     args.AssetDataItems = ProjectContext.AssetDataSet;

     var file = args.OutputFile;
     var fileName = file.Name;
     var extension = "";

     string defaultName = "assets";
     switch(procedure)
     {
        case AssetConsoleProcedure.AssetsToOpenXml:
           file.Extension = InOut.FileExtension.OPEN_XML;
           defaultName = "dam-dictionary";
           fileName = defaultName;
           extension = InOut.FileExtension.OPEN_XML;
           break;
        case AssetConsoleProcedure.AssetsToLexiconDatabase:
           file.Extension = String.Empty;
           defaultName = "dam-lexicon-db";
           fileName = defaultName;
           extension = String.Empty;
           args.Procedure = AssetConsoleProcedure.AssetsToLexiconDatabase;
           break;
        case AssetConsoleProcedure.AssetsToLexiconWorkbook:
           file.Extension = InOut.FileExtension.OPEN_XML;
           defaultName = "dam-lexicon";
           fileName = defaultName;
           extension = InOut.FileExtension.OPEN_XML;
           args.Procedure = AssetConsoleProcedure.AssetsToLexiconWorkbook;
           break;
        case AssetConsoleProcedure.AssetsToJsd:
           defaultName = "dam-json";
           fileName = defaultName;
           extension = InOut.FileExtension.JSD;
           args.Procedure = AssetConsoleProcedure.AssetsToJsd;
           break;
        case AssetConsoleProcedure.AssetsToJld:
           defaultName = "dam-json-ld";
           fileName = defaultName;
           extension = InOut.FileExtension.JSONLD;
           args.Procedure = AssetConsoleProcedure.AssetsToJld;
           break;
        case AssetConsoleProcedure.AssetsToGql:
           defaultName = "dam-gql";
           fileName = defaultName;
           extension = InOut.FileExtension.GQL;
           args.Procedure = AssetConsoleProcedure.AssetsToGql;
           break;
        case AssetConsoleProcedure.AssetsToDdl:
           defaultName = "dam-ddl";
           fileName = defaultName;
           extension = InOut.FileExtension.DDL;
           args.Procedure = AssetConsoleProcedure.AssetsToDdl;
           break;
        case AssetConsoleProcedure.AssetsToXsd:
        case AssetConsoleProcedure.ToXsd:
           defaultName = "dam-xml";
           fileName = defaultName;
           extension = InOut.FileExtension.XSD;
           break;
        default:
           break;
     }

     if (String.IsNullOrWhiteSpace(file.Path))
     {
        file.Path = "./" + Project.DOCUMENTS;
     }

     file.Name = args.Namespace.Prefix + "." + fileName;
     file.Extension = extension;

     var fName = String.IsNullOrWhiteSpace(fileName) ? 
        defaultName : fileName;

     args.OutFileExtension = extension;
     //args.TextMapFilePath = GetTextMapPath(args);

     var filePath = file.Path + "/" + fileName + "." + extension;
     //args.AssetDataItems = aresults.Data.
     var results = ProjectConsole.Execute(item.Item, args, filePath);

     // TODO: manage results here...
     // TODO: prepare templates based on needed processing...
  }

  /// <summary>
  /// Process Item... runing the project with inner arguments to fetch the 
  /// Asset Data Set and prepare a collection of the set elements.
  /// </summary>
  /// <remarks>
  /// The static ProjectContext will contain the ProjectItem if it is 
  /// successfully processed.
  /// </remarks>
  /// <param name="item">project item to execute</param>
  /// <returns>instance or ResultsLog is returned with Data as the prepared
  /// collection of asset data elements</returns>
  public static ResultsLog<List<AssetData>> Execute(ProjectItem item)
  {
     ResultsLog<List<AssetData>> results = 
        new ResultsLog<List<AssetData>>();

     ResultsLog<object> presults = ProjectConsole.ProcessItem(item.Item);

     if (!presults.Success)
     {
        results.Copy(presults);
        return results;
     }

     // set project runtime arguments...
     AssetConsoleArgumentsInfo pargs =
        presults.ResultValueObject as AssetConsoleArgumentsInfo;
     item.CurrentArguments = pargs;

     ProjectContext.SetAssetDataSet(pargs.AssetDataItems);
     ProjectContext.SetProjectAndResetArguments(item);

     // manage found project asset data items...
     List<AssetData> items = pargs == null ?
        null : pargs.AssetDataItems as List<AssetData>;

     AssetData.ReconcileUseCases(items, pargs.UseCases);

     results.Instance = items;
     results.ResultValueObject = items;
     results.Succeeded();

     return results;
  }

  /// <summary>
  /// Save given text into a file in the current project documents folder.
  /// </summary>
  /// <param name="text">data to save</param>
  public static void ToFile(string fileName, string text)
  {
     var item = ProjectContext.CurrentProject == null ? 
        null : ProjectContext.CurrentProject;
     if (item == null)
     {
        return;
     }
     var pfolder = item.GetDocumentsFolder();
     if (System.IO.Directory.Exists(pfolder))
     {
        // TODO: Remove hardcoded ReferenceData name...
        // TODO: Add newly added file in the Visual TreeItem side panel
        System.IO.File.WriteAllText(
           pfolder + "\\" + fileName, text);
     }
  }

  /// <summary>
  /// Save Reference Data Template as a File into the Project - Document
  /// folder.
  /// </summary>
  /// <param name="assetData"></param>
  public static void SaveReferenceDataTemplateFile(AssetData assetData)
  {
     var template = ReferenceDataTemplate.
        FromAssetData(null, assetData);
     var json = ReferenceDataTemplateBaseInfo.ToJson(template);

     string fileName = "ReferenceData.Template.json";
     ToFile(fileName, json);

     // add file to Project TreeView...
     var docFolder = ProjectContext.CurrentProject.GetDocumentsFolder();
     var fullPath = docFolder + "\\" + fileName;
     InOut.ItemBaseInfo bitem = new InOut.ItemBaseInfo();
     bitem.FromFullPath(fullPath, null);
     ProjectItem citem = new ProjectItem(bitem);
     bitem.Type = InOut.ItemType.File;

     AddItem(ProjectContext.TreeView, citem);
  }

  /// <summary>
  /// Save Asset... in the case that the AssetType is UseCase then all of
  /// those will be saved as individual assets.
  /// </summary>
  /// <param name="data">Asset Data</param>
  /// <param name="type">target Asset Type</param>
  public static async void SaveAsset(AssetData data, AssetType type)
  {
     AssetDataElementList items = data.Items;

     // save each Use-Case individually...
     IResultsLog results;
     if (type == AssetType.UseCase)
     {
        int cnt = 0;
        foreach(var uc in data.UseCases)
        {
           cnt++;
           string ucn = "uc." + uc.Name.ToLower();
           string uriText =
              data.DefaultNamespace.Uri.OriginalString + "/" + ucn;
           string prfText = data.DefaultNamespace.Prefix +
              ".uc" + cnt.ToString();
           NamespaceInfo auri = new NamespaceInfo(prfText, uriText, 
              data.DefaultNamespace.OrganizationDomainId,
              data.DefaultNamespace.Extension);

           results = await ResourceContext.SaveAssetAsync(
              items, auri, data.DefaultNamespace.OrganizationDomainId, 
              type);

           // TODO: manage failed results ASAP...
        }
        return;
     }

     // save the asset with given info...
     results = await ResourceContext.SaveAssetAsync(
        data.Items, data.DefaultNamespace, 
        data.DefaultNamespace.OrganizationDomainId, type);

     // TODO: manage failed results ASAP...
  }

  /// <summary>
  /// Process Save Option...
  /// </summary>
  /// <remarks>
  /// AssetData used must be available in the static ProjectContext.
  /// </remarks>
  /// <param name="optionName">option name to process</param>
  public static void ProcessSaveOption(string optionName)
  {
     AssetData data = ProjectContext.AssetData;
     AssetType type = ProjectContext.AssetType;

     switch (optionName)
     {
        case SaveOptionInfo.DATABASE:
           SaveAsset(data, type);
           break;
        case SaveOptionInfo.LEXICON_DATABASE:
           PrepareOutputFile(ProjectContext.CurrentProject,
              AssetConsoleProcedure.AssetsToLexiconDatabase);
           break;
        case SaveOptionInfo.LEXICON_WORKBOOK:
           PrepareOutputFile(ProjectContext.CurrentProject,
              AssetConsoleProcedure.AssetsToLexiconWorkbook);
           break;
        case SaveOptionInfo.EXCEL:
           PrepareOutputFile(ProjectContext.CurrentProject, 
              AssetConsoleProcedure.AssetsToOpenXml);
           break;
        case SaveOptionInfo.DATA_TEMPLATE_FILE:
           SaveReferenceDataTemplateFile(data);
           break;
        case SaveOptionInfo.XSD:
           PrepareOutputFile(ProjectContext.CurrentProject,
              AssetConsoleProcedure.AssetsToXsd);
           break;
        case SaveOptionInfo.JSD:
           PrepareOutputFile(ProjectContext.CurrentProject,
              AssetConsoleProcedure.AssetsToJsd);
           break;
        case SaveOptionInfo.JLD:
           PrepareOutputFile(ProjectContext.CurrentProject,
              AssetConsoleProcedure.AssetsToJld);
           break;
        case SaveOptionInfo.GQL:
           PrepareOutputFile(ProjectContext.CurrentProject,
              AssetConsoleProcedure.AssetsToGql);
           break;
        case SaveOptionInfo.DDL:
           PrepareOutputFile(ProjectContext.CurrentProject,
              AssetConsoleProcedure.AssetsToDdl);
           break;
        default:
           break;
     }
  }

  /// <summary>
  /// Add Project Item...  in given project item
  /// </summary>
  /// <param name="item">item information</param>
  /// <param name="folder">target folder full path</param>
  /// <param name="fileName">file name</param>
  /// <returns>true if added</returns>
  public static bool AddItem(
     ProjectItem item, ProjectItem newItem)
  {
     bool done = false;
     foreach (var i in item.Children)
     {
        if (i.Item.Full == newItem.ItemFolderPath)
        {
           // check if item is already in the children's list...
           foreach(var c in i.Children)
           {
              if (c.Item.Name == newItem.Item.Name)
              {
                 done = true;
                 break;
              }
           }

           // if not found in Children then add it...
           if (!done)
           {
              i.Children.Add(newItem);
           }
           return true;
        }
        done = AddItem(i, newItem);
     }
     return done;
  }

  /// <summary>
  /// Delete Project Item...
  /// </summary>
  /// <param name="item">item information</param>
  /// <param name="itemFullName">item full name</param>
  /// <returns>true if deleted</returns>
  public static bool DeleteItem(ProjectItem item, string itemFullName)
  {
     bool done = false;
     foreach (var i in item.Children)
     {
        if (i.Item.Full == itemFullName)
        {
           return item.Children.Remove(i);
        }
        done = DeleteItem(i, itemFullName);
     }
     return done;
  }


}
