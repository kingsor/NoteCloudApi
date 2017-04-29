# Welcome to the NoteCloud API

This project is made to create a RESTful API for any applications I would like to make in the future;

## API Definitions

### No login required (starts with /public)

*   [GET] /users - Gets all users
*   [POST] /users - Register a users
*   [POST] /users/login - Login a user
*   [GET] /notes/user/{userId} - Get public notes for a user
*   [GET] /notegroups/user/{userId} - Get user notegroups
*   [GET] /followers/{userId} - Get a users followers

### Require Login (starts with /private)

*   [GET] /notes/user/{userId} - Get private notes for a user
*   [POST] /notes - Add a note
*   [PUT] /notes/{noteId} - Update a note
*   [DELETE] /notes/{noteId} - Delete a note
*   [POST] /notegroups - Create a notegroup
*   [PUT] /notegroups/{notegroupId} - Update a notegroup
*   [DELETE] /notegroups/{notegroupId} - Delete a notegroup
*   [POST] /followers/{userId} - Follow a user
*   [DELETE] /followers/{userId} - Unfollow a user
