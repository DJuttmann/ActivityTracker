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
  /// Interaction logic for RegisterWindow.xaml
  /// </summary>
  public partial class RegisterWindow : Window
  {
    public Project MyProject;


    // Constructor.
    public RegisterWindow ()
    {
      InitializeComponent ();

      MyProject = null;
      ClearValidation ();
    }


    // Clear validation colours
    private void ClearValidation ()
    {
      RegisterName.Background = Validation.ValidInput;
      RegisterPassword.Background = Validation.ValidInput;
      RegisterPassword2.Background = Validation.ValidInput;
    }


    // Validate registration input
    private bool Validate ()
    {
      ClearValidation ();
      bool success = true;
      if (!Validation.IsName (RegisterName.Text))
      {
        RegisterName.Background = Validation.InvalidInput;
        success = false;
      }
      if (!Validation.ValidatePassword (RegisterPassword.Password))
      {
        RegisterPassword.Background = Validation.InvalidInput;
        success = false;
      }
      if (RegisterPassword2.Password != RegisterPassword.Password)
      {
        RegisterPassword2.Background = Validation.InvalidInput;
        success = false;
      }
      return success;
    }


    // Register button click handler.
    private void ButtonRegisterClick (object sender, RoutedEventArgs e)
    {
      if (MyProject != null && Validate ())
      {
        if (MyProject.RegisterUser (RegisterName.Text, RegisterPassword.Password, 
              Validation.StringToUserType (RegisterUserType.Text)))
        {
          DialogResult = true;
          this.Close ();
        }
        else
        {
          MessageBox.Show ("Could not register user");
        }
      }
    }


    // Cancel button click handler.
    private void ButtonRegisterCancelClick (object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      this.Close ();
    }


    private void RegisterPassword2_KeyDown (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
        ButtonRegisterClick (null, null);
    }
  }
}
