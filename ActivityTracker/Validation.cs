using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ActivityTracker
{
  class Validation
  {
    private static readonly Regex NamePattern = new Regex ("^[ \t]*[^ \t].*$");

    
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


    public static bool IsName (string s)
    {
      return NamePattern.IsMatch (s);
    }

  }

}
