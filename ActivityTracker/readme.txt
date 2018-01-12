﻿==========================================================================================
ActivityTracker by Daan Juttmann
Version: 0.1 alpha
Created: 2017-12-21
License: GNU General Public License 3.0 (https://www.gnu.org/licenses/gpl-3.0.en.html).
==========================================================================================

-- DESCRIPTION --

A tool for students and teachers to track activities. The program keeps a list of
activities, which users can start tracking data for. For each started activity, the user
can add session, which log the amount of time spent on the activity, as well as what
percentage of the activity was finished. Users can create their own activities as well.

The basic user type is a Student. Teacher accounts have the additional ability to view 
the activity progress of students.


-- USAGE --

- Activities
You can view the activities by selecting 'Activity List' from the 'View' menu.
Alternatively, if you are on the 'User Data' screen, you can click 'Add' to go to the
activity screen. The left side of the screen shows a list of available activities.
Select an activity to edit or delete it using the buttons at the top of the screen,
or press 'add' to create a new activity. Select an activity and click 'Start Activity'
to start tracking progress on it.

- User data
Select 'User Data' in the view menu to see all the activities you have started. Again
you can add, edit and delete items shown in the list using the corresponding buttons.
Clicking the 'Edit' button when an item is selected brings you to the Sessions Log.

- Session Log
The Session Log shows a list of all sessions for a specific task, including for each
session a date, duration, and percentage finished. Use the 'Add', 'Edit' and 'Delete'
buttons to Update the list accordingly.


-- IMPLEMENTED FEATURES FOR ALPHA VERSION 0.1 --

Several features have not yet been implemented. This is the current state of the program:

- Only one account can be used for testing purposes. The program automatically loads the
  'Admin' account. There user role has no efect on functionality yet.
- A sample database is included, if none is present the program automatically sets up a
  new one.
- You can view, add, edit and delete new activities, instances and sessions.
- A lot of data is not properly validated! entering incorrect values may lead to crashes.
- The tag system has not been implemented yet.
- No search is available yet.
- Any settings screens are still missing.

==========================================================================================