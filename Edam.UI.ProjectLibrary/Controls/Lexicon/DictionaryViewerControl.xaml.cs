using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236
using Edam.UI.Controls.DataModels;

namespace Edam.UI.Controls.Lexicon;


public sealed partial class DictionaryViewerControl : UserControl
{
    private DictionariesModel m_ViewModel = new DictionariesModel();
    public DictionariesModel ViewModel
    {
        get { return m_ViewModel; }
        set { m_ViewModel = value; }
    }

    public DictionaryViewerControl()
    {
        this.InitializeComponent();
        DataContext = m_ViewModel;
    }

}
