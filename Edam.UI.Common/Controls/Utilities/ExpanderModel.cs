using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

// -----------------------------------------------------------------------------

namespace Edam.UI.Common.Controls.Utilities;


public class ExpanderModel : ObservableObject
{
  private const string ChevronClose = "\xE96F";
  private const string ChevronOpen = "\xE970";

  private string m_SelectedChevron;
  public string SelectedChevron
  {
     get { return m_SelectedChevron; }
     set
     {
        if (m_SelectedChevron != value)
        {
           m_SelectedChevron = value;
           OnPropertyChanged(nameof(SelectedChevron));
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

  public ExpanderModel()
  {
     SelectedChevron = ChevronClose;
     PanelVisibility = Visibility.Collapsed;
  }

  public void TogglePanelVisibility()
  {
     SelectedChevron = PanelVisibility == Visibility.Visible ?
        ChevronClose : ChevronOpen;
     PanelVisibility = PanelVisibility == Visibility.Visible ?
        Visibility.Collapsed : Visibility.Visible;
  }

  /// <summary>
  /// Toogle panel visibility...
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void SidePanelToggle_Click(object sender, RoutedEventArgs e)
  {
     TogglePanelVisibility();
  }

}
