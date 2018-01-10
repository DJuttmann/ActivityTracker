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
    }


    // Register button click handler.
    private void ButtonRegisterClick (object sender, RoutedEventArgs e)
    {
      if (MyProject != null && RegisterPassword.Password == RegisterPassword2.Password)
      {
        if (MyProject.RegisterUser (RegisterName.Text, RegisterPassword.Password, 
              DatabaseConnection.StringToUserType (RegisterUserType.Text)))
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


    // Cancel button click handler.
    private void ButtonRegisterCancelClick (object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      this.Close ();
    }
  }
}
