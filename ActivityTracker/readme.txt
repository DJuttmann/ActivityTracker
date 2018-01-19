﻿==========================================================================================
ActivityTracker by Daan Juttmann
Version: 1.0
Created: 2017-12-21
License: GNU General Public License 3.0 (https://www.gnu.org/licenses/gpl-3.0.en.html).
==========================================================================================

-- DESCRIPTION --

A tool for students and teachers to track activities. The program keeps a list of
activities, which users can start tracking data for. For each started activity, the user
can add session, which log the amount of time spent on the activity, as well as what
percentage of the activity was finished. Users can create their own activities as well.

The basic user type is a Student. Teacher accounts have the additional ability to view 
the activity progress of other students.


-- USAGE --

- Regisitration, log in, log out, account settings
All these options can be found in the 'User' menu.

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

- Import and export data
To Import or export data, choose the appropriate option in the 'File' menu. Take note
the Importing is not supported yet in this version!

- Settings
Choosing the settings menu, you can select which database file to load when the program
starts. If the file does not exist, an empty database will be created with that name.


-- NOTES FOR RELEASE VERSION 1.0 --

- Importing files not implemented yet.
Due to some concerns with potential database collisions, the import function has not been
implemented yet.

==========================================================================================