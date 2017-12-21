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

    Int64 NewUserID ()
    {
      return DateTime.Now.Ticks;
    }


    Int64 NewActivityID ()
    {
      return DateTime.Now.Ticks;
    }


    Int64 NewInstanceID ()
    {
      return DateTime.Now.Ticks;
    }


    Int64 NewSessionID ()
    {
      return DateTime.Now.Ticks;
    }


    Int64 NewTagID ()
    {
      return DateTime.Now.Ticks;
    }


  }


  


}
