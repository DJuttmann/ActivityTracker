//========================================================================================
// ActivityTracker by Daan Juttmann
// Created: 2017-12-19
// License: GNU General Public License 3.0 (https://www.gnu.org/licenses/gpl-3.0.en.html).
//========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ActivityTracker
{
  /// <summary>
  /// Interaction logic for SettingsWindow.xaml
  /// </summary>
  public partial class SettingsWindow: Window
  {
    public string FileName
    {
      get {return DatabaseFileName.Text;}
    }


    // Constructor.
    public SettingsWindow (string databaseFile)
    {
      InitializeComponent ();

      DatabaseFileName.Text = databaseFile;
    }


    // Select file button.
    private void SelectFile_Click (object sender, RoutedEventArgs e)
    {
      Microsoft.Win32.OpenFileDialog fileSelect = new Microsoft.Win32.OpenFileDialog ();
      if (fileSelect.ShowDialog () == true)
        DatabaseFileName.Text = fileSelect.FileName;
    }


    // Save button.
    private void Save_Click (object sender, RoutedEventArgs e)
    {
      DialogResult = true;
      Close ();
    }


    // Cancel button.
    private void Cancel_Click (object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close ();
    }
  }
}
