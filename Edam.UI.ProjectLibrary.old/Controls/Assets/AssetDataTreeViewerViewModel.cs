using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------
using Edam.UI.Controls.DataModels;

namespace Edam.UI.Controls.Assets;


public class AssetDataTreeViewerViewModel : ObservableObject
{
    private DataMapContext m_Context;
    public DataMapContext Context
    {
        get { return m_Context; }
    }

    public void SetContext(DataMapContext context)
    {
        m_Context = context;
    }
}
