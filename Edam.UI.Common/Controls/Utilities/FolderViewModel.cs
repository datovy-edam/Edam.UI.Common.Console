using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

// -----------------------------------------------------------------------------
using CommunityToolkit.Mvvm.ComponentModel;
using Edam.Data.AssetSchema;
using Edam.InOut;


namespace Edam.UI.Common.Controls.Utilities;


public class FolderViewModel : ObservableObject
{

    private string m_LastFolderName = null;

    private ObservableCollection<FileDetailInfo> m_Items;
    public ObservableCollection<FileDetailInfo> Items
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

    public FolderViewModel()
    {
        Items = new ObservableCollection<FileDetailInfo>();
    }

    public void GetFolderFiles(string folderName)
    {
        m_LastFolderName = folderName;
        ObservableCollection<FileDetailInfo> itms =
           new ObservableCollection<FileDetailInfo>();
        var item = FolderFileReader.GetFolderFileInfo(folderName);
        foreach (var i in item.Children)
        {
            var f = FileDetailInfo.GetDetails(i);
            itms.Add(f);
        }
        Items = itms;
    }

    public void RefreshFolderFiles()
    {
        if (String.IsNullOrWhiteSpace(m_LastFolderName))
        {
            return;
        }
        GetFolderFiles(m_LastFolderName);
    }
}
