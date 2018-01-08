using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;


namespace ActivityTracker
{
  
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
      // [wip] finish this;
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

    // Add tag to database;
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

  }

}
