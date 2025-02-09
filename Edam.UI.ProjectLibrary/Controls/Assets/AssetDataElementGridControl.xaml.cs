using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
// -----------------------------------------------------------------------------
using Edam.UI.Controls.ViewModels;

namespace Edam.UI.Controls.Assets;


public sealed partial class AssetDataElementGridControl : UserControl
{
    private AssetViewerViewModel m_ViewModel;
    public AssetViewerViewModel ViewModel
    {
        get { return m_ViewModel; }
    }

    public AssetDataElementGridControl()
    {
        this.InitializeComponent();
        m_ViewModel = new AssetViewerViewModel();
        DataContext = m_ViewModel;
    }
}
