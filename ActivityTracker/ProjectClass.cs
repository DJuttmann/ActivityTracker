//========================================================================================
// ActivityTracker by Daan Juttmann
// Created: 2017-12-19
// License: GNU General Public License 3.0 (https://www.gnu.org/licenses/gpl-3.0.en.html).
//========================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace ActivityTracker
{

//========================================================================================
// Class Project
﻿//========================================================================================

  public partial class Project
  {
    private List <User> Users;
    private List <Activity> Activities;
    private List <Tag> Tags; 
    private User ActiveUser; // The user who is logged in.
    private User SelectedUser; // The user whose data is being viewed.
    private Activity SelectedActivity;
    private Instance SelectedInstance;
    private Session SelectedSession;
    DatabaseConnection Database;
    private string DatabaseFileName;

    public Int64? ActiveUserID
    {
      get
      {
        return ActiveUser != null ? (Int64?) ActiveUser.ID : null;
      }
    }

    public UserType ActiveUserType
    {
      get
      {
        return ActiveUser != null ? ActiveUser.Type : UserType.None;
      }
    }

    public string ActiveUserName
    {
      get
      {
        return ActiveUser != null ? ActiveUser.Name : "";
      }
    }

    public string ActiveUserPassword
    {
      get
      {
        return ActiveUser != null ? ActiveUser.PasswordHash : "";
      }
    }

    public List <string> ActiveUserTags
    {
      get
      {
        return ActiveUser != null ? ActiveUser.TagNames
                                  : new List <string> ();
      }
    }

    public string SelectedUserName
    {
      get
      {
        return SelectedUser != null ? SelectedUser.Name : "";
      }
    }

    public UserType SelectedUserType
    {
      get
      {
        return SelectedUser != null ? SelectedUser.Type : UserType.None;
      }
    }

    public string SelectedActivityName
    {
      get
      {
        return SelectedActivity != null ? SelectedActivity.Name : "";
      }
    }

    public Int64? SelectedActivityCreatorID
    {
      get
      {
        return SelectedActivity != null ? 
               (Int64?) SelectedActivity.CreatorID : null;
      }
    }

    public string SelectedActivityDescription
    {
      get
      {
        return SelectedActivity != null ? SelectedActivity.Description : "";
      }
    }

    public List <string> SelectedActivityTags
    {
      get
      {
        return SelectedActivity != null ? SelectedActivity.TagNames
                                        : new List <string> ();
      }
    }

    public string SelectedInstanceName
    {
      get
      {
        if (SelectedInstance == null)
          return "";
        Activity a = Activities.Find (x => x.ID == SelectedInstance.ActivityID);
        return a != null ? a.Name : "[unknown activity]";
      }
    }

    public string SelectedInstanceTimeSpent
    {
      get
      {
        return SelectedInstance != null ? 
               Validation.FormatTime (SelectedInstance.TimeSpent) : "0:00";
      }
    }

    public Int64 SelectedInstancePercent
    {
      get
      {
        return SelectedInstance != null ? SelectedInstance.PercentFinished : 0;
      }
    }

    public DateTime SelectedSessionDate
    {
      get
      {
        return SelectedSession != null ? SelectedSession.Date : new DateTime (2000, 1, 1);
      }
    }

    public string SelectedSessionTimeSpent
    {
      get
      {
        return SelectedSession != null ?
               SelectedSession.TimeSpent.ToString () : "";
      }
    }

    public string SelectedSessionPercentFinished
    {
      get
      {
        return SelectedSession != null ?
               SelectedSession.PercentFinished.ToString () : "";
      }
    }

       
    // Constructor.
    public Project () {
      Users = new List <User> ();
      Activities = new List <Activity> ();
      Tags = new List <Tag> ();
      ActiveUser = null;
      SelectedUser = null;
      SelectedActivity = null;
      SelectedInstance = null;
      SelectedSession = null;
      Database = null;
      DatabaseFileName = null;
    }


    // Load a database file.
    public bool LoadDatabase (string fileName)
    {
      bool success = true;
      try
      {
        Database = new DatabaseConnection (fileName);
        if (!Database.LoadAllTags (Tags))
          success = false;
        if (!Database.LoadAllUsers (Users, Tags))
          success = false;
        if (!Database.LoadAllActivities (Activities, Users, Tags))
          success = false;
      }
      catch (Exception ex)
      {
        System.Windows.MessageBox.Show ("Failed to open database connection." +
                                        Environment.NewLine +
                                        ex.Message);
        return false;
      }
      DatabaseFileName = fileName;
      if (!success)
        System.Windows.MessageBox.Show ("Failed to load activity and/or tag lists.");
      return success;
    }


//========================================================================================
// Account management

    
    // Register a new user into the system.
    public bool RegisterUser (string userName, string password, UserType type)
    {
      if (Database == null || ActiveUser != null) // cannot register if someone is already logged in
        return false;
//      if (!Validation.ValidatePassword (password))
//        return false;

      // check if the username already exists.
      if (Users.Find (x => x.Name == userName) != null)
        return false;
      
      User newUser = new User (0, userName,
                               Validation.CreateHash (password), type);
      if (Database.AddUser (newUser))
      {
        Users.Add (newUser);
        ActiveUser = newUser; // new user is logged in
        SelectUser (ActiveUser);
        return true;
      }
      return false;
    }


    // Try to log in a user given username and password. Returns false on failure.
    public bool LoginUser (string userName, string password)
    {
//      if (!Validation.ValidatePassword (password))
//        return false;

      User tempUser = Users.Find (x => x.Name == userName);
      if (tempUser == null)
      {
        return false; // user not found.
      }
      if (tempUser.PasswordHash == Validation.CreateHash (password))
      {
        ActiveUser = tempUser;
        SelectUser (ActiveUser);
        return true;
      }
      return false;
    }


    // Log out currently active user.
    public void Logout ()
    {
      ActiveUser = null;
      SelectUser (null);
      SelectActivity (null);
      SelectInstance (null);
    }


    // Set the password for the active user.
    public bool SetActiveUserPassword (string password)
    {
      User tempUser = new User (ActiveUser.ID, ActiveUser.Name, 
                                Validation.CreateHash (password), ActiveUser.Type);
      if (Database.UpdateUser (tempUser))
      {
        ActiveUser.PasswordHash = tempUser.PasswordHash;
        return true;
      }
      return false;
    }


    // Set the tags for the active user.
    public bool SetActiveUserTags (List <string> tagList)
    {
      // Add new tags
      foreach (string tag in tagList)
      {
        Tag currentTag = Tags.Find (x => x.Name == tag);
        if (currentTag == null) // if tag does not exist yet, create it.
        {
          currentTag = new Tag (0, tag);
          if (!Database.AddTag (currentTag))
            continue;
          Tags.Add (currentTag);
        }
        if (Database.AddUserTag (ActiveUser.ID, currentTag.ID))
          ActiveUser.AddTag (currentTag);
      }

      // Remove deleted tags
      foreach (string tag in ActiveUser.TagNames)
      {
        if (!tagList.Contains (tag))
        {
          Tag currentTag = Tags.Find (x => x.Name == tag);
          if (Database.RemoveUserTag (ActiveUser.ID, currentTag.ID))
            ActiveUser.RemoveTag (currentTag);
        }
      }
      return true;
    }


    // Get activity by index in list;
    public User GetUser (int index)
    {
      if (index < 0 || index >= Users.Count)
        return null;
      return Users [index];
    }


    // Add tag to activity.
    public bool AddUserTag (User user, Tag tag)
    {
      if (user.HasTag (tag))
        return false;
      if (Database.AddUserTag (ActiveUser.ID, tag.ID))
      {
        user.AddTag (tag);
        return true;
      }
      return false;
    }
   
    
    // Remove tag from activity.
    public bool RemoveUserTag (User user, Tag tag)
    {
      if (Database.RemoveUserTag (user.ID, tag.ID))
      {
        user.RemoveTag (tag);
        return true;
      }
      return false;
    }


    // Select a user by ID.
    public bool SelectUser (Int64 id)
    {
      User user = Users.Find (x => x.ID == id);
      if (user == null)
        return false;
      SelectUser (user);
      return true;
    }


    // Select a user. May be set to null to select none.
    private void SelectUser (User newUser)
    {
      SelectedUser = newUser;
      if (SelectedUser != null && !SelectedUser.DataLoaded)
        SelectedUser.LoadDataFromDatabase (Database, Activities);

      // Selected instance must either belong to selected user, or be null.
      if (SelectedUser == null || 
          (SelectedInstance != null && !SelectInstance (SelectedInstance.ID)))
        SelectInstance (null);
    }

//========================================================================================
// Activity management

   
    // Get activity by index in list;
    public Activity GetActivity (int index)
    {
      if (index < 0 || index >= Activities.Count)
        return null;
      return Activities [index];
    }


    // Create new activity owned by SelectedUser.
    public bool CreateActivity (string name, string description, 
                                List <string> tagList)
    {
      Activity newActivity = new Activity (0, SelectedUser.ID, name, description,
                                           SelectedUser.Name);
      if (!Database.AddActivity (newActivity))
        return false;
      Activities.Add (newActivity);
      SelectedUser.AddActivity (newActivity);

      // Add tags
      foreach (string tag in tagList)
      {
        Tag currentTag = Tags.Find (x => x.Name == tag);
        if (currentTag == null) // if tag does not exist yet, create it.
        {
          currentTag = new Tag (0, tag);
          if (!Database.AddTag (currentTag))
            continue;
          Tags.Add (currentTag);
        }
        if (Database.AddActivityTag (newActivity.ID, currentTag.ID))
          newActivity.AddTag (currentTag);
      }
      return true;
    }


    // Edit the selected Activity.
    public bool EditActivity (string name, string description,
                              List <string> tagList)
    {
      if (SelectedActivity == null)
        return false;
      // Make sure SelectedActivity still exists?
      Activity activity = Activities.Find (x => x.ID == SelectedActivity.ID);
      if (activity == null)
        return false;
      Activity editActivity = new Activity (SelectedActivity.ID, activity.CreatorID,
                                            name, description, SelectedActivity.CreatorName);

      if (!Database.UpdateActivity (editActivity))
        return false;
      activity.Name = name;
      activity.Description = description;

      // Add new tags
      foreach (string tag in tagList)
      {
        Tag currentTag = Tags.Find (x => x.Name == tag);
        if (currentTag == null) // if tag does not exist yet, create it.
        {
          currentTag = new Tag (0, tag);
          if (!Database.AddTag (currentTag))
            continue;
          Tags.Add (currentTag);
        }
        if (Database.AddActivityTag (activity.ID, currentTag.ID))
          activity.AddTag (currentTag);
      }

      // Remove deleted tags
      foreach (string tag in activity.TagNames)
      {
        if (!tagList.Contains (tag))
        {
          Tag currentTag = Tags.Find (x => x.Name == tag);
          if (Database.RemoveActivityTag (activity.ID, currentTag.ID))
            activity.RemoveTag (currentTag);
        }
      }
      return true;
    }


    // Delete the selected activity.
    public bool DeleteActivity ()
    {
      if (SelectedActivity == null)
        return false;
      if (Database.DeleteActivity (SelectedActivity.ID))
      {
        Activities.Remove (SelectedActivity);
        User Creator = Users.Find (x => x.ID == SelectedActivity.CreatorID);
        if (Creator != null)
          Creator.DeleteActivity (SelectedActivity);
        SelectActivity (null);
        return true;
      }
      return false;
    }


    // Add tag to activity.
    public bool AddActivityTag (Activity activity, Tag tag)
    {
      if (activity.HasTag (tag))
        return false;
      if (Database.AddActivityTag (activity.ID, tag.ID))
      {
        activity.AddTag (tag);
        return true;
      }
      return false;
    }
   
    
    // Remove tag from activity.
    public bool RemoveActivityTag (Activity activity, Tag tag)
    {
      if (Database.RemoveActivityTag (activity.ID, tag.ID))
      {
        activity.RemoveTag (tag);
        return true;
      }
      return false;
    }


    // Select a activity by ID.
    public bool SelectActivity (Int64 id)
    {
      Activity activity = Activities.Find (x => x.ID == id);
      if (activity == null)
        return false;
      SelectActivity (activity);
      return true;
    }


    // Select an activity. May be set to null to select none.
    private void SelectActivity (Activity newActivity)
    {
      SelectedActivity = newActivity;
    }

//========================================================================================
// Instance management


    // Get instance for SelectedUser by index in list.
    public Instance GetInstance (int index)
    {
      if (SelectedUser == null)
        return null;
      return SelectedUser.GetInstance (index);
    }


    // Create new instance for SelectedUser given activity index.
    public bool CreateInstance (Int64 activityID)
    {
      Activity activity = Activities.Find (x => x.ID == activityID);
      if (activity == null)
        return false;

      Instance newInstance = new Instance (0, activity);
      if (Database.AddInstance (SelectedUser.ID, newInstance))
      {
        SelectedUser.AddInstance (newInstance);
        return true;
      }
      return false;
    }


    // Delete the selected instance.
    public bool DeleteInstance ()
    {
      if (SelectedUser == null || SelectedInstance == null)
        return false;

      if (Database.DeleteInstance (SelectedInstance.ID))
      {
        SelectedUser.DeleteInstance (SelectedInstance.ID);
        SelectInstance (null);
        return true;
      }
      return false;
    }


    // Select an instance based on id.
    public bool SelectInstance (Int64 id)
    {
      if (SelectedUser == null)
        return false;
      Instance instance = SelectedUser.GetInstance (id);
      if (instance == null)
        return false;
      SelectInstance (instance);

      return true;
    }


    // Select an instance. May be set to null to select none.
    private void SelectInstance (Instance newInstance)
    {
      SelectedInstance = newInstance;
      if (SelectedInstance != null && !SelectedInstance.DataLoaded)
        SelectedInstance.LoadDataFromDatabase (Database);

      // Selected session must either belong to selected instance, or be null.
      if (SelectedInstance == null || 
          (SelectedSession != null && !SelectSession (SelectedSession.ID)))
        SelectSession (null);
    }


//========================================================================================
// Session management


    // Get session for SelectedUser by index in list.
    public Session GetSession (int index)
    {
      return SelectedInstance.GetSession (index);
    }


    public bool CreateSession (DateTime date, Int64 timeSpent, 
                               Int64 percentFinished)
    {
      Session newSession = new Session (0, date, timeSpent, percentFinished);
      if (Database.AddSession (SelectedInstance.ID, newSession))
      {
        SelectedInstance.AddSession (newSession);
        return true;
      }
      return false;
    }


    // Edit selected session.
    public bool EditSession (DateTime date, Int64 timeSpent, Int64 percentFinished)
    {
      if (SelectedSession == null || SelectedInstance == null)
        return false;
      // Make sure Selectedsession still exists?
      Session session = SelectedInstance.GetSession (SelectedSession.ID);
      if (session == null)
        return false;
      Session editSession = new Session (SelectedSession.ID, date, 
                                         timeSpent, percentFinished);

      if (Database.UpdateSession (editSession))
      {
        session.Date = date;
        session.TimeSpent = timeSpent;
        session.PercentFinished = percentFinished;
        return true;
      }
      return false;
    }


    // Delete the selected session.
    public bool DeleteSession ()
    {
      if (SelectedInstance == null || SelectedSession == null)
        return false;

      if (Database.DeleteSession (SelectedSession.ID))
      {
        SelectedInstance.DeleteSession (SelectedSession.ID);
        SelectSession (null);
        return true;
      }
      return false;
    }


    // Select a session by ID.
    public bool SelectSession (Int64 id)
    {
      if (SelectedInstance == null)
        return false;
      Session session = SelectedInstance.GetSession (id);
      if (session == null)
        return false;
      SelectSession (session);
      return true;
    }


    // Select an activity. May be set to null to select none.
    private void SelectSession (Session newSession)
    {
      SelectedSession = newSession;
    }

//========================================================================================
// Tag management


    public bool CreateTag (string name)
    {
      Tag newTag = new Tag (0, name);
      if (Database.AddTag (newTag))
      {
        Tags.Add (newTag);
        return true;
      }
      return false;
    }
  }
}