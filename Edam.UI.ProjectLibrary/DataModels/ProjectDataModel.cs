using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

// -----------------------------------------------------------------------------
using Edam.InOut;

namespace Edam.UI.Controls.DataModels;


public class ProjectDataModel
{

    public static ProjectItem? ToObservable(FolderFileItemInfo item)
    {
        if (item == null) return null;

        ProjectItem? p = new ProjectItem(item);
        foreach (var c in item.Children)
        {
            p.Children.Add(ToObservable(c));
        }
        return p;
    }

}
