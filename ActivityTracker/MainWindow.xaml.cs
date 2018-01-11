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
    private List <UIActivityItem> ActivityItems;
    private List <UIInstanceItem> InstanceItems;
    private List <UISessionItem> SessionItems;
    private View ActiveView;

    // Constructor.
    public MainWindow ()
    {
      InitializeComponent ();

      MainProject = new Project ();
      MainProject.LoadDatabase ("ActivityDatabase.sqlite");
      ActivityItems = new List <UIActivityItem> ();
      InstanceItems = new List <UIInstanceItem> ();
      SessionItems = new List <UISessionItem> ();
      ShowNoView ();

      // [wip] Login admin -- for testing purposes, remove later.
      MainProject.LoginUser ("Admin", "12345678");
      LoginSetup ();
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
      EditActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Visible;

      InstanceView.Visibility = Visibility.Hidden;
      SelectUser.Visibility = Visibility.Hidden;

      SessionView.Visibility = Visibility.Hidden;
      NewSession.Visibility = Visibility.Hidden;
      EditSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "Activity List";
      LoadActivityList ();
      ShowActivityList ();
      ActiveView = View.Activities;
    }


    // Show the list of availabe activities.
    private void ShowActivityList ()
    {
      ActivityList.Children.Clear ();
      for (int i = 0; i < ActivityItems.Count; i++)
        ActivityList.Children.Add (ActivityItems [i].Element);
    }


    // Show the instances view.
    private void ShowInstanceView ()
    {
      ActivityView.Visibility = Visibility.Hidden;
      FindActivity.Visibility = Visibility.Hidden;
      NewActivity.Visibility = Visibility.Hidden;
      EditActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Hidden;

      InstanceView.Visibility = Visibility.Visible;
      SelectUser.Visibility = Visibility.Visible;

      SessionView.Visibility = Visibility.Hidden;
      NewSession.Visibility = Visibility.Hidden;
      EditSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "User Data";
      InstancesText.Content = "Activities for " + MainProject.SelectedUserName;
      LoadInstanceList ();
      ShowInstanceList ();
      ActiveView = View.Instances;
    }


    // Show the list of activity instances for the selected user.
    private void ShowInstanceList ()
    {
      InstanceList.Children.Clear ();
      for (int i = 0; i < InstanceItems.Count; i++)
        InstanceList.Children.Add (InstanceItems [i].Element);
    }


    // Show the session view.
    private void ShowSessionView ()
    {
      ActivityView.Visibility = Visibility.Hidden;
      FindActivity.Visibility = Visibility.Hidden;
      NewActivity.Visibility = Visibility.Hidden;
      EditActivity.Visibility = Visibility.Hidden;
      StartButton.Visibility = Visibility.Hidden;

      InstanceView.Visibility = Visibility.Hidden;
      SelectUser.Visibility = Visibility.Hidden;

      SessionView.Visibility = Visibility.Visible;
      NewSession.Visibility = Visibility.Hidden;
      EditSession.Visibility = Visibility.Hidden;

      ViewTitle.Content = "Session Log";
      SessionsText.Content = MainProject.SelectedActivityName + " - " + 
                             MainProject.SelectedUserName +
                             "'s sessions";
      LoadSessionList ();
      ShowSessionList ();
      ActiveView = View.Sessions;
    }


    // Show the list of sessions for the selected instance.
    private void ShowSessionList ()
    {
      SessionList.Children.Clear ();
      for (int i = 0; i < SessionItems.Count; i++)
        SessionList.Children.Add (SessionItems [i].Element);
    }


    private void ShowNewActivity ()
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


    private void HideNewActivity ()
    {
      if (ActiveView == View.Activities)
      {
        FindActivity.Visibility = Visibility.Visible;
        NewActivity.Visibility = Visibility.Hidden;
      }
    }


    private void ShowEditActivity ()
    {
      if (ActiveView == View.Activities)
      {
        FindActivity.Visibility = Visibility.Hidden;
        EditActivity.Visibility = Visibility.Visible;

        NewActivityName.Text = MainProject.SelectedActivityName;
        NewActivityDescription.Text = MainProject.SelectedActivityDescription;
        NewActivityTagInput.Text = "";
      }
    }


    private void HideEditActivity ()
    {
      if (ActiveView == View.Activities)
      {
        FindActivity.Visibility = Visibility.Visible;
        EditActivity.Visibility = Visibility.Hidden;
      }
    }


    private void ShowNewSession ()
    {
      if (ActiveView == View.Sessions)
      {
        NewSession.Visibility = Visibility.Visible;

        NewSessionDate.Text = "";
        NewSessionTimeSpent.Text = "";
        NewSessionPercentFinished.Text = "";
      }
    }


    private void HideNewSession ()
    {
      if (ActiveView == View.Sessions)
      {
        NewSession.Visibility = Visibility.Hidden;
      }
    }


    private void ShowEditSession ()
    {
      if (ActiveView == View.Sessions)
      {
        EditSession.Visibility = Visibility.Visible;

        EditSessionDate.Text = "";
        EditSessionTimeSpent.Text = "";
        EditSessionPercentFinished.Text = "";
      }
    }


    private void HideEditSession ()
    {
      if (ActiveView == View.Sessions)
      {
        EditSession.Visibility = Visibility.Hidden;
      }
    }


﻿//========================================================================================
// Activity view


    // Load the list of activities.
    private void LoadActivityList ()
    {
      ActivityItems.Clear ();
      int i = 0;
      Activity activity = MainProject.GetActivity (i);
      while (activity != null)
      {
        var item = new UIActivityItem (this, activity.ID, activity.Name,
                                       "Unknown", activity.Description);
        ActivityItems.Add (item);
        i++;
        activity = MainProject.GetActivity (i);
      }
    }


    // Select none of the activities.
    public void ActivitiesSelectNone ()
    {
      foreach (UIActivityItem activity in ActivityItems)
        activity.Selected = false;
    }


﻿//========================================================================================
// Instance view


    // Load the list of instances for the selected user and activity.
    private void LoadInstanceList ()
    {
      InstanceItems.Clear ();
      int i = 0;
      Instance instance = MainProject.GetInstance (i);
      while (instance != null)
      {
        var item = new UIInstanceItem (this, instance.ID, instance.Name,
                                       instance.TimeSpent, instance.PercentFinished);
        InstanceItems.Add (item);
        i++;
        instance = MainProject.GetInstance (i);
      }
    }


    // Select none of the instances.
    public void InstancesSelectNone ()
    {
      foreach (UIInstanceItem instance in InstanceItems)
        instance.Selected = false;
    }


﻿//========================================================================================
// Session view


    // Load the list of sessions for the selected instance.
    private void LoadSessionList ()
    {
      SessionItems.Clear ();
      int i = 0;
      Session session = MainProject.GetSession (i);
      while (session != null)
      {
        var item = new UISessionItem (this, session.ID, session.Date.ToShortDateString (),
                                      session.TimeSpent, session.PercentFinished);
        SessionItems.Add (item);
        i++;
        session = MainProject.GetSession (i);
      }
    }


    // Select none of the sessions.
    public void SessionsSelectNone ()
    {
      foreach (UISessionItem instance in SessionItems)
        instance.Selected = false;
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
        ShowNewActivity ();
        break;
      case View.Instances:
        ShowActivityView ();
        break;
      case View.Sessions:
        ShowNewSession ();
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
        ShowEditActivity ();
        break;
      case View.Instances:
        UIInstanceItem item = InstanceItems.Find (x => x.Selected);
        if (item != null && MainProject.SelectInstance (item.ID))
          ShowSessionView ();
        break;
      case View.Sessions:
        ShowEditSession ();
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


    private void StartButtonClick (object sender, RoutedEventArgs e)
    {
      bool started = false;
      foreach (UIActivityItem item in ActivityItems)
      {
        if (item.Selected && MainProject.CreateInstance (item.ID))
          started = true;
      }
      if (started)
        ShowInstanceView ();
    }


    private void ButtonCeateActivity_Click (object sender, RoutedEventArgs e)
    {
      MainProject.CreateActivity (NewActivityName.Text, NewActivityDescription.Text);
      ShowActivityList ();
      HideNewActivity ();
    }


    private void ButtonCancelActivity_Click (object sender, RoutedEventArgs e)
    {
      HideNewActivity ();
    }


    private void ButtonSaveEditActivity_Click (object sender, RoutedEventArgs e)
    {
      // [wip] MainProject.EditActivity (
      ShowActivityList ();
      HideEditActivity ();
    }


    private void ButtonCancelEditActivity_Click (object sender, RoutedEventArgs e)
    {
      HideEditActivity ();
    }


    private void ButtonCeateSession_Click (object sender, RoutedEventArgs e)
    {
      MainProject.CreateSession (new DateTime (2000, 1, 1), 
                                 Convert.ToInt64 (NewSessionTimeSpent.Text),
                                 Convert.ToInt64 (NewSessionPercentFinished.Text));
      ShowSessionList ();
      HideNewSession ();
    }


    private void ButtonCancelSession_Click (object sender, RoutedEventArgs e)
    {
      HideNewSession ();
    }

    private void ButtonEditSaveSession_Click (object sender, RoutedEventArgs e)
    {
      // [wip] MainProject.EditSession (
      ShowSessionList ();
      HideEditSession ();
    }

    private void ButtonEditCancelSession_Click (object sender, RoutedEventArgs e)
    {
      HideEditSession ();
    }
  }
}
