using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
using Edam.UI.Controls.ViewModels;
using Edam.UI.Controls.DataModels;
using Edam.UI.Controls.Lexicon;
using Edam.UI.Common;

namespace Edam.UI.Controls.Booklets;


[ContentProperty(Name = "FrameContent")]
public sealed partial class BookletPanelControl : UserControl
{

    private BookViewModel m_ViewModel = new BookViewModel();
    public BookViewModel ViewModel
    {
        get { return m_ViewModel; }
    }

    public static DependencyProperty FrameContentProperty =
        DependencyProperty.Register("FrameContent", typeof(object),
           typeof(FramePanelControl), null);

    public object FrameContent
    {
        get => GetValue(FrameContentProperty);
        set => SetValue(FrameContentProperty, value);
    }

    public BookletPanelControl()
    {
        this.InitializeComponent();
        DataContext = m_ViewModel;
        m_ViewModel.ManageEvent = ManageNotification;
        TextSimilarityScoreViewer.Visibility = Visibility.Collapsed;
    }

    public ListView GetListView()
    {
        return BookletList;
    }

    /// <summary>
    /// Capture events comming from child controls associated with this
    /// BookViewModel...
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args">event arguments</param>
    public void ManageNotification(object sender, NotificationArgs args)
    {
        if (args.Type == NotificationType.AddItem)
        {
            if (args.MessageText == "TEXT")
            {
                m_ViewModel.AddTextCell();
            }
            else
            {
                m_ViewModel.AddCodeCell();
            }
        }
        else if (args.Type == NotificationType.RemoveItem)
        {
            m_ViewModel.DeleteCell(args.EventData);
        }
        else if (args.Type == NotificationType.ExecuteItem &&
           args.MessageText == LexiconDataModel.TEXT_SIMILARITY)
        {
            TextSimilarityScoreViewer.Visibility = Visibility.Visible;
        }
    }

    public void ManageNotification(object sender, DataTreeEventArgs args)
    {
        MapSidePanel.ViewModel.ManageNotification(sender, args);
        TextSimilarityScoreViewer.Visibility = Visibility.Collapsed;
    }

    public void SetContext(DataMapContext context)
    {
        context.BookletViewList = BookletList;
        context.LexiconModel.ManageNotification =
           ManageNotification;
        TextSimilarityScoreViewer.ViewModel.ManageNotification =
           ManageNotification;

        ViewModel.SetContext(context);
        MapSidePanel.ViewModel.Context = context;
    }

    private void AddCodeCell_Click(object sender, RoutedEventArgs e)
    {
        m_ViewModel.AddCodeCell();
    }

    private void AddTextCell_Click(object sender, RoutedEventArgs e)
    {
        m_ViewModel.AddTextCell();
    }

    private void DeleteCell_Click(object sender, RoutedEventArgs e)
    {
        m_ViewModel.DeleteCell();
    }

    private void BookletList_SelectionChanged(
       object sender, SelectionChangedEventArgs e)
    {

    }

    private void ExecuteBooklet_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Context.Execute(ViewModel.Model.SelectedBooklet);
    }

    private void SemanticSimilarityCompare_Click(
       object sender, RoutedEventArgs e)
    {
        if (TextSimilarityScoreViewer.Visibility == Visibility.Visible)
        {
            TextSimilarityScoreViewer.Visibility = Visibility.Collapsed;
            return;
        }

        ViewModel.Context.LexiconSemanticTextCompare(
           ViewModel.Model.SelectedBooklet,
           TextSimilarityScoreViewer.ViewModel);
    }

}
