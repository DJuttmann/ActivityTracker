using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace ActivityTracker
{
  class Validation
  {
    private static readonly Regex NamePattern = new Regex ("^[ \t]*[^ \t'][^']*$");
    private static readonly Regex TextPattern = new Regex ("^[^']*$");

    public static readonly SolidColorBrush InvalidInput = 
      new SolidColorBrush (Color.FromArgb (0xFF, 0xFF, 0xA0, 0xA0));
    public static readonly SolidColorBrush ValidInput = 
      new SolidColorBrush (Color.FromArgb (0xFF, 0xFF, 0xFF, 0xFF));
   

    // Verify if string is a name (max 32 characters, no ' allowed,
    // contains at least one non-space character)
    public static bool IsName (string s)
    {
      return s.Length <= 32 && NamePattern.IsMatch (s);
    }


    // Verify if string is text (max 1024 characters, no ' alowed)
    public static bool IsText (string s)
    {
      return s.Length <= 1024 && TextPattern.IsMatch (s);
    }


    // Verify if string is a number.
    public static bool IsNumber (string s)
    {
      if (s.Length == 0)
        return false;
      for (int i = 0; i < s.Length; i++)
        if (s [i] < '0' || s [i] > '9')
          return false;
      return true;
    }


    // Verify if string is a year.
    public static bool IsYear (string s)
    {
      if (IsNumber (s) && s.Length <= 4)
        return true;
      return false;
    }


    // Verify if string is a Month.
    public static bool IsMonth (string s)
    {
      if (IsNumber (s) && s.Length <= 2)
      {
        int value = Convert.ToInt32 (s);
        if (value > 0 && value <= 12)
          return true;
      }
      return false;
    }


    // Verify if string is a Day.
    public static bool IsDay (string s)
    {
      if (IsNumber (s) && s.Length <= 2)
      {
        int value = Convert.ToInt32 (s);
        if (value > 0 && value <= 31)
          return true;
      }
      return false;
    }


    // Verify if string is time.
    public static bool IsTime (string s)
    {
      string [] segments = s.Split (new char [] {':'});
      switch (segments.Length)
      {
      case 0:
      default:
        return false;
      case 1:
        if (IsNumber (segments [0]))
          return true;
        return false;
      case 2:
        if (IsNumber (segments [0]) && IsNumber (segments [1]))
          return true;
        return false;
      }
    }


    // Convert time string (H:M or M) to int (minutes)
    public static Int64 TimeToInt (string s)
    {
      string [] segments = s.Split (new char [] {':'});
      switch (segments.Length)
      {
      case 0:
      default:
        return 0;
      case 1:
        return Convert.ToInt64 (segments [0]);
      case 2:
        return Convert.ToInt64 (segments [0]) * 60 + 
               Convert.ToInt64 (segments [1]);
      }
    }


    // Convert int (minutes) to time string (H:M)
    public static string FormatTime (Int64 minutes)
    {
      Int64 hours = minutes / 60;
      minutes %= 60;
      return hours.ToString () + ":" + (minutes < 10 ? "0" : "") + minutes.ToString ();
    }


    // Verify is string is percentage.
    public static bool IsPercent (string s)
    {
      if (IsNumber (s) && s.Length <= 3)
      {
        int value = Convert.ToInt32 (s);
        if (value >= 0 && value <= 100)
          return true;
      }
      return false;
    }


    // Check if password is not too short or too long, and if its characters are valid.
    public static bool ValidatePassword (string password)
    {
      if (password.Length < 8 || password.Length > 32)
        return false;
      for (int i = 0; i < password.Length; i++)
      {
        if (password [i] < ' ' || password [i] > '~')
          return false;
      }
      return true;
    }


    // Convert string to usertype.
    public static UserType StringToUserType (string s)
    {
      if (s == "Student")
        return UserType.Student;
      if (s == "Teacher")
        return UserType.Teacher;
      return UserType.None;
    }


    // Convert date to string.
    public static string DateToString (DateTime d)
    {
      return d.Year.ToString () + "-" + 
             d.Month.ToString () + "-" +
             d.Day.ToString ();
    }


    // Convert string to date.
    public static DateTime StringToDate (string s)
    {
      int year = 2000;
      int month = 1;
      int day = 1;
      string [] split = s.Split (new [] {'-'});
      if (split.Length == 3)
      {
        try
        {
          year = Convert.ToInt32 (split [0]);
          month = Convert.ToInt32 (split [1]);
          day = Convert.ToInt32 (split [2]);
        }
        catch
        {
          // Incorrect string format found.
          year = 1999;
          month = 12;
          day = 31;
        }
      }
      return new DateTime (year, month, day);
    }

  }

}
