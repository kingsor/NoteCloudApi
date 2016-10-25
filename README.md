# Welcome to the NoteCloud API

This project is made to create a RESTful API for any applications I would like to make in the future;
All API Definitions are available via my website http://www.itderrickh.com

## API Definitions

*   [GET] /users - Gets all users
*   [POST] /users - Register a users
*   [POST] /users/login - Login a user
*   [GET] /notes/user/{userId} - Get notes for a user
*   [POST] /notes - Add a note
*   [PUT] /notes/{noteId} - Update a note
*   [DELETE] /notes/{noteId} - Delete a note
*   [GET] /notegroups/user/{userId} - Get user notegroups
*   [POST] /notegroups - Create a notegroup
*   [PUT] /notegroups/{notegroupId} - Update a notegroup
*   [DELETE] /notegroups/{notegroupId} - Delete a notegroup
*   [GET] /followers/{userId} - Get a users followers
*   [POST] /followers/{userId} - Follow a user
*   [DELETE] /followers/{userId} - Unfollow a user
