using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

namespace Edam.UI.Controls.Utilities;


public class LableToggleViewModel : ObservableObject
{

    public const string EXPANDER_UP = "\uE96D";
    public const string EXPANDER_DOWN = "\uE96E";

    private string m_Text;
    public string Text
    {
        get { return m_Text; }
        set
        {
            if (m_Text != value)
            {
                m_Text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
    }

    private string m_Icon;
    public string Icon
    {
        get { return m_Icon; }
        set
        {
            if (m_Icon != value)
            {
                m_Icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
    }

    private Visibility m_PanelVisibility;
    public Visibility PanelVisibility
    {
        get { return m_PanelVisibility; }
        set
        {
            if (m_PanelVisibility != value)
            {
                m_PanelVisibility = value;
                OnPropertyChanged(nameof(PanelVisibility));
            }
        }
    }

    public LableToggleViewModel(string lableText)
    {
        Text = lableText;
        Icon = EXPANDER_DOWN;
        PanelVisibility = Visibility.Collapsed;
    }

    public void Open()
    {
        if (PanelVisibility == Visibility.Collapsed)
        {
            ToggleClick(this, null);
        }
    }

    public void ToggleClick(object sender, RoutedEventArgs e)
    {
        Icon = Icon == EXPANDER_DOWN ? EXPANDER_UP : EXPANDER_DOWN;
        PanelVisibility = Icon == EXPANDER_UP ?
           Visibility.Visible : Visibility.Collapsed;
    }

}
