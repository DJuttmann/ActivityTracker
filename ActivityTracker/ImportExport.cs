//========================================================================================
// ActivityTracker by Daan Juttmann
// Created: 2017-12-19
// License: GNU General Public License 3.0 (https://www.gnu.org/licenses/gpl-3.0.en.html).
//========================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace ActivityTracker
{

  class ImportExport
  {
    private static StreamWriter StreamOut;
    private static StreamReader StreamIn;


﻿//========================================================================================
// Reading


    // Try to open a file for writing data.
    public static bool OpenFileRead (string filename)
    {
      try
      {
        StreamIn = new StreamReader (filename);
      }
      catch
      {
        return false;
      }
      return true;
    }


    // Close the input file.
    public static void CloseFileRead ()
    {
      StreamIn.Close ();
    }


    // Split a line of data into separate values.
    private static List <string> SplitLine (string data)
    {
      List <string> output = new List <string> ();
      StringBuilder entry = new StringBuilder ();
      bool quoted = false;

      for (int i = 0; i < data.Length; i++)
      {
        char c = data [i];
        switch (c)
        {
        case '\'':
          if (quoted)
          {
            output.Add (entry.ToString ());
            entry.Clear ();
          }
          quoted = !quoted;
          break;
        case ' ':
        case '\t':
          if (!quoted && entry.Length > 0)
          {
            output.Add (entry.ToString ());
            entry.Clear ();
          }
          else
            entry.Append (c);
          break;
        default:
          entry.Append (c);
          break;
        }
      }
      if (entry.Length > 0)
        output.Add (entry.ToString ());
      return output;
    }


﻿//========================================================================================
// Writing

    // Try to open a file for writing data.
    public static bool OpenFileWrite (string filename)
    {
      try
      {
        StreamOut = new StreamWriter (filename);
      }
      catch
      {
        return false;
      }
      return true;
    }
    

    // Close the output file.
    public static void CloseFileWrite ()
    {
      StreamOut.Close ();
    }


    // Write any data.
    public static void WriteData (object obj)
    {
      switch (obj)
      {
      case User user:
        WriteUser (user);
        break;
      case Activity activity:
        WriteActivity (activity);
        break;
      case Instance instance:
        WriteInstance (instance);
        break;
      case Session session:
        WriteSession (session);
        break;
      default:
        break;
      }
    }

    
    // Write user data
    private static void WriteUser (User user)
    {
      StreamOut.WriteLine ("User " + 
                           user.ID.ToString () + " '" +
                           user.Name + "' '" +
                           user.PasswordHash + "' '" +
                           user.Type.ToString () + "'");
      foreach (string tag in user.TagNames)
      {
        StreamOut.WriteLine ("\tUserTag " +
                             user.ID.ToString () + " '" +
                             tag + "'");
      }

    }


    // Write activity data
    private static void WriteActivity (Activity activity)
    {
      StreamOut.WriteLine ("Activity " + 
                           activity.ID.ToString () + " '" +
                           activity.CreatorID.ToString () + " '" +
                           activity.Name + "' '" +
                           activity.Description + "'");
      foreach (string tag in activity.TagNames)
      {
        StreamOut.WriteLine ("\tActivityTag " +
                             activity.ID.ToString () + " '" +
                             tag + "'");
      }
    }

    
    // Write instance data
    private static void WriteInstance (Instance instance)
    {
      StreamOut.WriteLine ("Instance " + 
                           instance.ID.ToString () + " '" +
                           instance.ActivityID.ToString () + "'");
    }


    // Write session data
    private static void WriteSession (Session session)
    {
      StreamOut.WriteLine ("Session " + 
                           session.ID.ToString () + " " +
                           Validation.DateToString (session.Date) + " " +
                           session.TimeSpent.ToString () + " " +
                           session.PercentFinished.ToString ());
    }

  }




  public partial class Project
  {

    public void ExportData (string filename, DataType type)
    {
      object data;
      switch (type)
      {
      case DataType.User:
        data = SelectedUser;
        break;
      case DataType.Activity:
        data = SelectedActivity;
        break;
      case DataType.Instance:
        data = SelectedInstance;
        break;
      case DataType.Session:
        data = SelectedSession;
        break;
      default:
        data = null;
        break;
      }

      if (!ImportExport.OpenFileWrite (filename))
        return;
      ImportExport.WriteData (data);
      ImportExport.CloseFileWrite ();
    }

  }

}