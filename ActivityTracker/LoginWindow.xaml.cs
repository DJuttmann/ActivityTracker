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
      ClearValidation ();
      LoginName.Focus ();
    }


    // Clear validation colours
    private void ClearValidation ()
    {
      LoginName.Background = Validation.ValidInput;
      LoginPassword.Background = Validation.ValidInput;
    }


    // Validate registration input
    private bool Validate ()
    {
      ClearValidation ();
      bool success = true;
      if (!Validation.IsName (LoginName.Text))
      {
        LoginName.Background = Validation.InvalidInput;
        success = false;
      }
//      if (!Validation.ValidatePassword (LoginPassword.Password))
//      {
//        LoginPassword.Background = Validation.InvalidInput;
//        success = false;
//      }
      return success;
    }


    private void ButtonLoginClick (object sender, RoutedEventArgs e)
    {
      if (MyProject != null && Validate ())
      {
        if (MyProject.LoginUser (LoginName.Text, LoginPassword.Password))
        {
          DialogResult = true;
          this.Close ();
        }
        else
        {
          WarningMessage.Content = "Error: incorrect username / password combination.";
        }
      }
    }

    private void ButtonLoginCancelClick (object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      this.Close ();
    }


    private void LoginPassword_KeyDown (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
        ButtonLoginClick (null, null);
    }


    private void LoginName_TextChanged (object sender, TextChangedEventArgs e)
    {
      WarningMessage.Content = "";
    }


    private void LoginPassword_TextChanged (object sender, RoutedEventArgs e)
    {
      LoginName_TextChanged (null, null);
    }
  }
}
