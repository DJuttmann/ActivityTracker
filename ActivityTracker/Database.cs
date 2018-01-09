using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;


namespace ActivityTracker
{
  
//========================================================================================
// Class DatabaseConnection
﻿//========================================================================================


  class DatabaseConnection
  {
    private string FileName;
    private SQLiteConnection Connection;

//========================================================================================
// Initialization, open and close

    // Constructor.
    public DatabaseConnection (string fileName)
    {
      if (fileName != null)
      {
        FileName = fileName;
        if (!File.Exists (FileName))
          if (!CreateDatabase ())
            throw new FileNotFoundException ("Database file not found: ", FileName);
      }
    }


    // Create a new database file.
    private bool CreateDatabase ()
    {
      try
      {
        SQLiteConnection.CreateFile (FileName);
      }
      catch (SQLiteException ex)
      {
        // [wip] show error message?
        return false;
      }
      Connection = new SQLiteConnection ("Data Source=" + FileName + ";Version=3;");
      if (Open ())
      {
        SQLiteCommand command = new SQLiteCommand (Connection);

        command.CommandText = "CREATE TABLE Users " +
          "(UserID INTEGER PRIMARY KEY, UserName VARCHAR(32), " +
          "PasswordHash VARCHAR(128), UserType VARCHAR(32))";
        command.ExecuteNonQuery ();

        command.CommandText = "CREATE TABLE Activities " +
          "(ActivityID INTEGER PRIMARY KEY, CreatorID INTEGER, " +
          "ActName VARCHAR(32), Description VARCHAR(1024))";
        command.ExecuteNonQuery ();

        command.CommandText = "CREATE TABLE Instances " +
          "(InstanceID INTEGER PRIMARY KEY, UserID INTEGER, ActivityID INTEGER, " +
          "TimeSpent INTEGER, PercentFinished INTEGER)";
        command.ExecuteNonQuery ();

        command.CommandText = "CREATE TABLE Sessions " +
          "(SessionID INTEGER PRIMARY KEY, InstanceID INTEGER, Date VARCHAR(128), " +
          "TimeSpent INTEGER, PercentFinished INTEGER)";
        command.ExecuteNonQuery ();

        command.CommandText = "CREATE TABLE Tags " +
          "(TagID INTEGER PRIMARY KEY, Name VARCHAR(32))";
        command.ExecuteNonQuery ();

        command.CommandText = "CREATE TABLE UserTags " +
          "(UserID INTEGER PRIMARY KEY, TagID INTEGER PRIMARY KEY)";
        command.ExecuteNonQuery ();

        command.CommandText = "CREATE TABLE ActivityTags " +
          "(ActID INTEGER PRIMARY KEY, TagID INTEGER PRIMARY KEY)";
        command.ExecuteNonQuery ();

        Close ();
        return true;
      }
      return false;
    }


    // Open a connection to the database.
    private bool Open ()
    {
      Connection = new SQLiteConnection ("Data Source=" + FileName + ";Version=3;");
      try
      {
        Connection.Open ();
        return true;
      }
      catch
      {
        return false;
      }
    }


    // Close a connection to the database.
    private void Close ()
    {
      Connection.Close ();
    }

//========================================================================================
// Reading and Writing

    public Int64 NewUserID ()
    {
      return DateTime.Now.Ticks;
    }


    public Int64 NewActivityID ()
    {
      return DateTime.Now.Ticks;
    }


    public Int64 NewInstanceID ()
    {
      return DateTime.Now.Ticks;
    }


    public Int64 NewSessionID ()
    {
      return DateTime.Now.Ticks;
    }


    public Int64 NewTagID ()
    {
      return DateTime.Now.Ticks;
    }

//----------------------------------------------------------------------------------------
// Users

    // Convert string to usertype.
    private UserType StringToUserType (string s)
    {
      if (s == "Student")
        return UserType.Student;
      if (s == "Teacher")
        return UserType.Teacher;

      return UserType.Student; // [wip] maybe return alternative?
    }


    // Load
    public bool LoadUser (string userName, ref Int64 userID, ref string passwordHash,
                          ref UserType type)
    {
      SQLiteCommand command = new SQLiteCommand (Connection);
      bool success = false;
      if (Open ())
      {
        command.CommandText = "SELECT * FROM Users WHERE UserName = " + userName;
        SQLiteDataReader reader = command.ExecuteReader ();
        if (reader.Read ())
        {
          userID = (Int64) reader ["UserID"];
          passwordHash = (string) reader ["PasswordHash"];
          type = StringToUserType ((string) reader ["UserType"]);
          success = true;
        }
        if (reader.Read ())
        {
          // [wip] handle case when there are multiple users with same name 
          // (this should never happen!)
        }
        Close ();
      }
      return success;
    }


    // Add user to database;
    public bool AddUser (User user)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "INSERT INTO Users " +
          "(UserID, UserName, PasswordHash, UserType) values (" +
          user.ID.ToString () + ", '" +
          user.Name + "', '" +
          user.PasswordHash + "', '" +
          user.Type.ToString () + "')";
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }


    // Update user information in database.
    public bool UpdateUser (User user)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "UPDATE Users SET " +
          "UserName = " + user.Name +
          ", PasswordHash = " + user.PasswordHash +
          ", UserType = " + user.Type.ToString () +
          "WHERE UserID = " + user.ID.ToString (); 
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }

//----------------------------------------------------------------------------------------
// Activities

    // Save all activities from database in list. List allTags must already be filled.
    public bool LoadAllActivities (List <Activity> activities, List <Tag> allTags)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        activities.Clear ();
        Activity newActivity = null;
        command.CommandText = "SELECT * FROM Activities";
        SQLiteDataReader ActivityReader = command.ExecuteReader ();
        SQLiteDataReader TagReader;
        while (ActivityReader.Read ())
        {
          newActivity = new Activity (
            (Int64) ActivityReader ["ActivityID"], 
            (Int64) ActivityReader ["CreatorID"],
            (string) ActivityReader ["ActName"],
            (string) ActivityReader ["Description"]);
          command.CommandText = "SELECT * FROM ActivityTags WHERE ActID = " +
                                newActivity.ID.ToString ();
          TagReader = command.ExecuteReader ();
          while (TagReader.Read ())
          {
            newActivity.AddTag ((Int64) TagReader ["TagID"], allTags);
          }
          activities.Add (newActivity);
        }
        Close ();
      }
      return success;
    }


    // Add activity to database;
    public bool AddActivity (Activity activity)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "INSERT INTO Activities " +
          "(ActivityID, CreatorID, ActName, Description) values (" +
          activity.ID.ToString () + ", '" +
          activity.CreatorID.ToString () + ", '" +
          activity.Name + "', '" +
          activity.Description + "')";
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }


    // Update activity information in database.
    public bool UpdateActivity (Activity activity)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "UPDATE Activities SET " +
          "CreatorID = " + activity.CreatorID +
          ", ActName = " + activity.Name +
          ", Description = " + activity.Description +
          "WHERE ActivityID = " + activity.ID.ToString (); 
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }


    // Remove activity from Database ()
    public bool DeleteActivity (Activity activity)
    {
      // [wip]
      return false;
    }

//----------------------------------------------------------------------------------------
// Instances
      
    // Add an instance to the database.
    public bool AddInstance (Int64 userID, Instance instance)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "INSERT INTO Instances " +
          "(InstanceID, UserID, ActivityID, TimeSpent, PercentFinished) values (" +
          instance.ID.ToString () + ", " +
          userID.ToString () + ", " +
          instance.ActivityID.ToString () + ", " +
          instance.TimeSpent.ToString () + ", " +
          instance.PercentFinished.ToString () + ")";
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }


    // Update instance information in database.
    public bool UpdateInstance (Instance instance)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "UPDATE Instances SET " +
          "TimeSpent = " + instance.TimeSpent.ToString () +
          ", PercentFinished = " + instance.PercentFinished.ToString () +
          "WHERE InstanceID = " + instance.ID.ToString (); 
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }


    // Remove an instance from the Database.
    public bool DeleteInstance (Int64 instanceID)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "DELETE FROM Instances WHERE " +
          "InstanceID = " + instanceID.ToString ();
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }

//----------------------------------------------------------------------------------------
// Sessions
      
    // Add a session to the database.
    public bool AddSession (Int64 instanceID, Session session)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "INSERT INTO Sessions " +
          "(SessionID, InstanceID, ActivityID, Date, TimeSpent, PercentFinished) values (" +
          session.ID.ToString () + ", " +
          instanceID.ToString () + ", '" +
          session.Date.ToString () + "', " +
          session.TimeSpent.ToString () + ", " +
          session.PercentFinished.ToString () + ")";
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }


    // Update session information in database.
    public bool UpdateSession (Session session)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "UPDATE Sessions SET " +
          "Date = '" + session.Date.ToString () +
          "', TimeSpent = " + session.TimeSpent.ToString () +
          ", PercentFinished = " + session.PercentFinished.ToString () +
          "WHERE SessionID = " + session.ID.ToString (); 
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }


    // Remove an session from the Database.
    public bool DeleteSession (Int64 sessionID)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "DELETE FROM Sessions WHERE " +
          "SessionID = " + sessionID.ToString ();
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }

//----------------------------------------------------------------------------------------
// Tags

    // Load all tags from Database.
    public bool LoadAllTags (List <Tag> tags)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        tags.Clear ();
        Tag newTag = null;
        command.CommandText = "SELECT * FROM Tags";
        SQLiteDataReader Reader = command.ExecuteReader ();
        while (Reader.Read ())
        {
          newTag = new Tag (
            (Int64) Reader ["TagID"], 
            (string) Reader ["Name"]);
          tags.Add (newTag);
        }
        Close ();
      }
      return success;
    }


    // Add tag to database.
    public bool AddTag (Tag newTag)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "INSERT INTO Tags (TagId, Name) values (" +
          newTag.ID.ToString () + ", '" +
          newTag.Name + "')";
        command.ExecuteNonQuery ();
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }


    // Add a tag to a user in the database.
    public bool AddUserTag (Int64 UserID, Int64 TagID)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "INSERT INTO UserTags (UserID, TagId) values (" +
          UserID.ToString () + ", " +
          TagID.ToString () + ")";
        command.ExecuteNonQuery ();
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }


    // Delete tag from user in the database.
    public bool RemoveUserTag (Int64 UserID, Int64 TagID)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "DELETE FROM UserTags WHERE " +
          "UserID = " + UserID.ToString () +
          ", TagID = " + TagID.ToString ();
        success = command.ExecuteNonQuery () > 0;
        Close ();
      }
      return success;
    }


    // Add a tag to a user in the database.
    public bool AddActivityTag (Int64 ActivityID, Int64 TagID)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "INSERT INTO ActivityTags (ActivityID, TagId) values (" +
          ActivityID.ToString () + ", " +
          TagID.ToString () + ")";
        success = command.ExecuteNonQuery () == 1;
        Close ();
      }
      return success;
    }


    // Delete tag from activity in the database.
    public bool RemoveActivityTag (Int64 ActivityID, Int64 TagID)
    {
      bool success = false;
      SQLiteCommand command = new SQLiteCommand (Connection);
      if (Open ())
      {
        command.CommandText = "DELETE FROM ActivityTags WHERE " +
          "ActID = " + ActivityID.ToString () +
          ", TagID = " + TagID.ToString ();
        success = command.ExecuteNonQuery () > 0;
        Close ();
      }
      return success;
    }
  }

}
