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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ActivityTracker
{
  public enum View
  {
    None,
    Activities,
    Instances,
    Sessions
  }


  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow: Window
  {
    private Project MainProject;
    private View ActiveView;


    // Constructor.
    public MainWindow ()
    {
      InitializeComponent ();

      MainProject = new Project ();
      MainProject.LoadDatabase ("ActivityDatabase.sqlite");
      ShowNoView ();
    }


    // Loads the window for user after logging in.
    private void LoginSetup ()
    {
      ShowActivityView ();
    }

﻿//========================================================================================
// Manage visibility of UI elements

    // Show no view.
    private void ShowNoView ()
    {
      ActivityView.Visibility = Visibility.Hidden;
      FindActivity.Visibility = Visibility.Hidden;
      NewActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Visible;

      InstanceView.Visibility = Visibility.Hidden;
      SelectUser.Visibility = Visibility.Hidden;

      SessionView.Visibility = Visibility.Hidden;
      NewSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "";
      ActiveView = View.None;
    }


    // Show the activities view.
    private void ShowActivityView ()
    {
      ActivityView.Visibility = Visibility.Visible;
      FindActivity.Visibility = Visibility.Visible;
      NewActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Visible;

      InstanceView.Visibility = Visibility.Hidden;
      SelectUser.Visibility = Visibility.Hidden;

      SessionView.Visibility = Visibility.Hidden;
      NewSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "Activity List";
      ReloadActivityList ();
      ActiveView = View.Activities;
    }


    // Reload the list of activities from the selected user.
    private void ReloadActivityList ()
    {
      ActivityList.Children.Clear ();
      int i = 0;
      Activity activity = MainProject.GetActivity (i);
      while (activity != null)
      {
        var item = new UIActivityItem (activity.CreatorID, activity.Name,
                                       "Unknown", activity.Description);
        ActivityList.Children.Add (item.Element);
        i++;
        activity = MainProject.GetActivity (i);
      }
    }


    // Show the instances view.
    private void ShowInstanceView ()
    {
      ActivityView.Visibility = Visibility.Hidden;
      FindActivity.Visibility = Visibility.Hidden;
      NewActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Hidden;

      InstanceView.Visibility = Visibility.Visible;
      SelectUser.Visibility = Visibility.Visible;

      SessionView.Visibility = Visibility.Hidden;
      NewSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "User Activities";
      InstancesText.Content = "Activities for " + MainProject.SelectedUserName;
      ActiveView = View.Instances;
    }


    // Show the session view.
    private void ShowSessionView ()
    {
      ActivityView.Visibility = Visibility.Hidden;
      FindActivity.Visibility = Visibility.Hidden;
      NewActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Hidden;

      InstanceView.Visibility = Visibility.Hidden;
      SelectUser.Visibility = Visibility.Hidden;

      SessionView.Visibility = Visibility.Visible;
      NewSession.Visibility = Visibility.Visible;

      ViewTitle.Content = "Session Log";
      SessionsText.Content = MainProject.SelectedActivityName + " - " + 
                             MainProject.SelectedUserName +
                             "'s sessions";
      ActiveView = View.Sessions;
    }


    private void OpenNewActivity ()
    {
      if (ActiveView == View.Activities)
      {
        FindActivity.Visibility = Visibility.Hidden;
        NewActivity.Visibility = Visibility.Visible;

        NewActivityName.Text = "";
        NewActivityDescription.Text = "";
        NewActivityTagInput.Text = "";
      }
    }


    private void CloseNewActivity ()
    {
      if (ActiveView == View.Activities)
      {
        FindActivity.Visibility = Visibility.Visible;
        NewActivity.Visibility = Visibility.Hidden;
      }
    }

﻿//========================================================================================
// Event handlers

    private void MenuRegisterClick (object sender, RoutedEventArgs e)
    {
      // Launch the registration window.
      var register = new RegisterWindow ();
      register.MyProject = MainProject;
      bool? success = register.ShowDialog ();
      if (success is bool s)
        if (s)
          LoginSetup ();
    }


    private void MenuLoginClick (object sender, RoutedEventArgs e)
    {
      // Launch the login Window
      var login = new LoginWindow ();
      login.MyProject = MainProject;
      bool? success = login.ShowDialog ();
      if (success is bool s)
        if (s)
          LoginSetup ();
    }


    private void MenuLogoutClick (object sender, RoutedEventArgs e)
    {
      // [wip] Log out.
    }


    private void TextBox_TextChanged (object sender, TextChangedEventArgs e)
    {
      // [wip] Can this be removed?
    }


    private void MenuInstancesClick (object sender, RoutedEventArgs e)
    {
      ShowInstanceView ();
    }


    private void MenuActivitiesClick (object sender, RoutedEventArgs e)
    {
      ShowActivityView ();
    }


    private void AddButton_Click (object sender, RoutedEventArgs e)
    {
      switch (ActiveView)
      {
      case View.Activities:
        OpenNewActivity ();
        break;
      case View.Instances:
        break;
      case View.Sessions:
        break;
      default:
        break;
      }
    }


    private void EditButton_Click (object sender, RoutedEventArgs e)
    {
      switch (ActiveView)
      {
      case View.Activities:
        break;
      case View.Instances:
        break;
      case View.Sessions:
        break;
      default:
        break;
      }
    }


    private void DeleteButton_Click (object sender, RoutedEventArgs e)
    {
      switch (ActiveView)
      {
      case View.Activities:
        break;
      case View.Instances:
        break;
      case View.Sessions:
        break;
      default:
        break;
      }
    }


    private void ButtonCeateActivity_Click (object sender, RoutedEventArgs e)
    {
      MainProject.CreateActivity (NewActivityName.Text, NewActivityDescription.Text);
      ReloadActivityList ();
      CloseNewActivity ();
    }


    private void ButtonCancelActivity_Click (object sender, RoutedEventArgs e)
    {
      CloseNewActivity ();
    }

  }
}
