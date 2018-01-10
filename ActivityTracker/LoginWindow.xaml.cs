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
  /// Interaction logic for LoginWindow.xaml
  /// </summary>
  public partial class LoginWindow : Window
  {
    public Project MyProject;

    public LoginWindow()
    {
      InitializeComponent();

      MyProject = null;
    }

    private void ButtonLoginClick (object sender, RoutedEventArgs e)
    {
      if (MyProject != null)
      {
        if (MyProject.LoginUser (LoginName.Text, LoginPassword.Password))
        {
          DialogResult = true;
          this.Close ();
        }
        else
        {
          // [wip] Show some error message.
        }
      }
    }

    private void ButtonLoginCancelClick (object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      this.Close ();
    }
  }
}
