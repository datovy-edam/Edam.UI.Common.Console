using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// -----------------------------------------------------------------------------
using Edam.Data.AssetConsole;
using Edam.Data.AssetSchema;
using Edam.Data.AssetProject;
using Edam.InOut;
using System.Collections.ObjectModel;
using Edam.Data.Asset;
using Edam.UI.Common.Controls.Utilities;

namespace Edam.UI.Controls.DataModels;

public class ProjectItem : TextEditorViewModel
{
  public const string EXECUTABLE_NAME = "arguments";
  public AssetConsoleArgumentsInfo CurrentArguments { get; set; }

  public ItemBaseInfo Item { get; set; }

  public FolderFileItemInfo Parent
  {
     get { return Item.Parent; }
  }

  public string Name
  {
     get { return Item.Name; }
  }

  public string NameFull
  {
     get { return Item.NameFull; }
  }

  public NamespaceInfo Namespace
  {
     get { return CurrentArguments.Namespace; }
  }

  public ObservableCollection<ProjectItem> Children { get; set; }
  public string ItemFolderPath
  {
     get
     {
        if (Item == null ||
           (Item.Type != ItemType.Folder && Item.Type != ItemType.File))
        {
           return null;
        }
        return Item.Type == ItemType.Folder ? Item.Full : Item.Path;
     }
  }

  public bool CanExecute
  {
     get
     {
        string name = GetFolderName().ToLower();
        return name == EXECUTABLE_NAME;
     }
  }

  public ProjectItem(ItemBaseInfo item)
  {
     if (item == null)
     {
        return;
     }

     InitializeVisibility(false);
     IconTypeNo = (int)item.Type;
     SelectedText = item.NameFull;

     Children = new ObservableCollection<ProjectItem>();
     Item = item;
  }

  /// <summary>
  /// Get a Project Item given a full path.
  /// </summary>
  /// <param name="fullPath">full path</param>
  /// <returns>instance of ProjectItem is returned</returns>
  public ProjectItem GetProjectItem(string fullPath)
  {
     ItemBaseInfo item = new ItemBaseInfo();
     item.FromFullPath(fullPath, null);
     ProjectItem projectItem = new ProjectItem(item);

     return projectItem;
  }

  public string GetFolderName()
  {
     if (String.IsNullOrWhiteSpace(ItemFolderPath))
     {
        return String.Empty;
     }
     string[] l = ItemFolderPath.Split('\\');
     if (l.Length == 0)
     {
        return String.Empty;
     }
     return l[l.Length - 1];
  }

  public string GetProjectsFolder()
  {
     System.Text.StringBuilder sb = new System.Text.StringBuilder();
     string[] l = Item.Path.Split("\\");
     int cnt = 0;
     foreach (string s in l)
     {
        if (s == Project.PROJECTS)
        {
           sb.Append(s);
           break;
        }
        sb.Append(s + "\\");
        cnt++;
     }
     return sb.ToString();
  }

  public string GetProjectFolder()
  {
     System.Text.StringBuilder sb = new System.Text.StringBuilder();
     string[] l = Item.Path.Split("\\");
     int cnt = 0;
     foreach (string s in l)
     {
        if (s == Project.PROJECTS)
        {
           sb.Append(
              s + (l.Length > cnt + 1 ? "\\" + l[cnt + 1] : String.Empty));
           break;
        }
        sb.Append(s + "\\");
        cnt++;
     }
     return sb.ToString();
  }

  public string GetDocumentsFolder()
  {
     var pfolder = GetProjectFolder();
     return pfolder + "\\" + Project.DOCUMENTS;
  }

  public override void TextBlock_DoubleTapped()
  {
     TextBlockSetText(Item.NameFull);
  }
}
