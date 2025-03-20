# Guide: How to Use
## Login Window
Student Logins:
- Username: jake Password: student123
- Username: jonathan Password: jonathan123
Teacher Login:
- Username: sarah Password: teacher123


## Main Features 
In both Student & Teacher Dashboards:
- **Data persistence: ** Actions performed are automatically saved in a JSON file named UserData.json.
- **Search for a subject:** Select the search bar by left-clicking on it and begin typing a partial phrase and click on the blue search button. Note that the search is not case-sensitive. To turn off the search, delete the phrase from the search box and click on the blue search button again.
- **View subject details:** Hover over a subject to see its description.
- **Logout:** Left-click on the "Log Out" button in the top-right corner of the window to return to the login window.

Student Dashboard:
- **Enroll in a subject:** First left-click on the subject to select it. Then left click on the "Enroll in Selected Subject" button. 
- **Drop a subject:** First left-click on the subject to select it. Then left click on the "Drop Selected Subject" button. 

Teacher Dashboard:
- **Create new subjects:** First left-click on the subject. In the bottom menu, enter the name and description in the text boxes and left click on the "Create" button. 
- **Edit a subject:** First left-click on the subject and then click on "Update Details" button. The bottom menu will then automatically change to accommodate editing. Modify the name and description and left click on the "Save" button. 


 # Contributors 
Genius Engineers Group
Azzam: Created the student and teacher dashboard UI. Created the extra feature of search bar.
Jonathan: Created the login screen UI and authentication. Created the teachers "create and edit" functions.
Daniils: Performed all of the testing (unit, functional, and headless).



# A. Unit Testing

# B. Functional Testing
## Student Functionality Tests:
✅ **Enrollment Test:** When a student enrolls in an available subject, it appears in the "Enrolled Subjects" section. Once the action is performed, the student receives a popup window containing a confirmation message.

✅ **Drop Subject Test:** When a student drops a subject in which he/she is enrolled in, it appears in the "Available Subjects" section. Once the action is performed, the student receives a popup window containing a confirmation message.


## Teacher Functionality Tests:
✅ **Create Subject Test:** When a teacher creates a new subject, it will appear in the Teachers subject section as well as in the "Available Subjects" sections for all students.

✅ **Delete Subject Test:** When a teacher deletes a selected subject, the subject will be removed from the Teachers subject section. The deletion will reflect as well in the "Available Subjects" sections for all students.



## System-Level Tests:
✅ **Login Test:** When a user enters a valid student or teacher username and password, the user will be directed to student or teacher dashboard, depending on the user's role. It is not possible for a student to log in to the teacher’s dashboard and vice-versa. If the user enters an incorrect username or password a small message is displayed.
✅ **Data Persistence Test:** Any actions performed by students (enroll or drop a subject) or teachers (create or edit a subject) are saved in UserData.json.


# C. User Interface Testing
