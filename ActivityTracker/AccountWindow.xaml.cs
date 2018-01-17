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
  /// Interaction logic for AccountWindow.xaml
  /// </summary>
  public partial class AccountWindow : Window
  {
    private Project MyProject = null;  


    public AccountWindow (Project p)
    {
      InitializeComponent ();

      MyProject = p;
      foreach (string s in MyProject.ActiveUserTags)
        AccountTagList.Items.Add (s);
    }


    private void ClearValidation ()
    {
      AccountOldPassword.Background = Validation.ValidInput;
      AccountNewPassword.Background = Validation.ValidInput;
      AccountNewPassword2.Background = Validation.ValidInput;
    }


    // Validate new user tag input.
    public bool ValidateNewUserTag ()
    {
      if (!Validation.IsName (AccountNewTag.Text))
      {
        AccountNewTag.Background = Validation.InvalidInput;
        return false;
      }
      AccountNewTag.Background = Validation.ValidInput;
      return true;
    }


    // Add a tag to the list.
    private void AddTag_Click (object sender, RoutedEventArgs e)
    {
      if (ValidateNewUserTag ())
      {
        string newTag = AccountNewTag.Text.ToLower ();
        foreach (object obj in AccountTagList.Items)
        {
          if ((string) obj == newTag)
            return;
        }
        AccountTagList.Items.Add (newTag);
        AccountNewTag.Text = string.Empty;
      }
    }


    // Validate the old and new password input.
    private bool ValidatePassword ()
    {
      ClearValidation ();
      bool success = true;
      if (MyProject.ActiveUserPassword != Validation.CreateHash (AccountOldPassword.Password))
      {
        AccountOldPassword.Background = Validation.InvalidInput;
        success = false;
      }
      if (!Validation.ValidatePassword (AccountNewPassword.Password))
      {
        AccountNewPassword.Background = Validation.InvalidInput;
        success = false;
      }
      if (AccountNewPassword.Password != AccountNewPassword2.Password)
      {
        AccountNewPassword2.Background = Validation.InvalidInput;
        success = false;
      }
      return success;
    }


    // Save the edited data.
    private void SaveAccount_Click (object sender, RoutedEventArgs e)
    {
      // Validate password changes
      if (AccountOldPassword.Password.Length != 0 ||
          AccountNewPassword.Password.Length != 0 ||
          AccountNewPassword2.Password.Length != 0)
      {
        if (ValidatePassword ())
        {
          MyProject.SetActiveUserPassword (AccountNewPassword.Password);
        }
        else
          return; // cancel saving alltogether.
      }

      // Save tag changes
      List <string> tagList = new List <string> ();
      foreach (Object obj in AccountTagList.Items)
        tagList.Add ((string) obj);
      MyProject.SetActiveUserTags (tagList);

      DialogResult = true;
      this.Close ();
    }


    private void AccountTagList_KeyDown (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete || e.Key == Key.Back)
      {
        AccountTagList.Items.Remove (AccountTagList.SelectedItem);
      }
    }


    private void AccountNewTag_KeyDown (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
        AddTag_Click (null, null);
    }
  }
}
