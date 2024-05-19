# School Management API Documentation

This document provides an overview of the endpoints available in the School Management API along with their functionalities and payload structures. The API is written with C#.

## Summary

The **School Management API** is designed to meet the requirements of the assessment. This project employs **WebSockets** to facilitate real-time communication between clients and the server, ensuring that updates and notifications are delivered instantly. The WebSocket URL for this API is `https://localhost:7299/index.html`.

### Key Features

- **User Authentication and Registration:** Secure user registration and login processes with JWT token-based authentication to protect access to the system.
- **Course Management:** Comprehensive CRUD operations for managing courses, including creating, updating, retrieving, and deleting course information.
- **Student Management:** Efficiently handle student data with endpoints for creating, retrieving, updating, and deleting student records.
- **Real-Time Updates:** Leveraging WebSockets to provide immediate updates and notifications to users, ensuring they are always informed of the latest changes.
- **Input Validation:** Input validations are implemented to guard against XML, XSS and SQL injection attacks.

## Getting Started

### Prerequisites

Before you begin, ensure you have met the following requirements:

- [Install .NET Core](https://dotnet.microsoft.com/download)

### Installation

1. Clone this repository:
   ```shell
   git clone https://github.com/Meekdavid/Niyo-Assesment
   ```
2. Navigate to the Project Directory:
   ```shell
   cd Niyo-Assesment
   ```
3. Restore Dependencies:
   ```shell
   dotnet restore
   ```
4. Build Projects:
   ```shell
   dotnet build
   ```
5. Run the API locally:
   ```shell
   dotnet run
   ```
   The API will be available at `http://localhost:5085/swagger/index.html` (HTTP) and `https://localhost:7299/swagger/index.html` (HTTPS) by default. Alternatively, load the project/solution on Visual Studio and run.

## Authentication Controller

### Register a New User

Endpoint: `POST /api/Authentication/Register`

Register a new user in the system.
For the payload below, TypeId 1 for admin, 2 for teacher and 3 for student.

#### Request Payload

```json
{
  "firstName": "string",
  "lastName": "string",
  "userName": "string",
  "password": "string",
  "phoneNumber": "string",
  "locationId": "string",
  "typeId": "string"
}
```

#### Response Payload

```json
{
  "ResponseCode": "string",
  "ResponseMessage": "string"
}
```

### Login User

Endpoint: `POST /api/Authentication/Login`

Login an existing user and receive a JWT token for authentication.

#### Request Payload

```json
{
  "userName": "string",
  "password": "string"
}
```

#### Response Payload

```json
{
  "JwtToken": "string",
  "ResponseCode": "string",
  "ResponseMessage": "string"
}
```

**Note:** To access other endpoints, clients must include the JWT token in the header with the key 'Authorization' and value 'Bearer 'token''.

## Courses Controller

### Create a New Course

Endpoint: `POST /api/Courses/Create`

Create a new course in the system.

#### Request Payload

```json
{
  "Title": "string",
  "Description": "string",
  "Credits": "string",
  "Instructor": "string",
  "Department": "string",
  "StartDate": "string",
  "EndDate": "string"
}
```

#### Response Payload

```json
{
  "ResponseCode": "string",
  "ResponseMessage": "string"
}
```

### Retrieve All Courses

Endpoint: `GET /api/Courses/RetrieveAll`

Retrieve all courses available in the system.

#### Response Payload

```json
[
  {
    "Id": 0,
    "Title": "string",
    "Description": "string",
    "Credits": "string",
    "Instructor": "string",
    "Department": "string",
    "StartDate": "string",
    "EndDate": "string"
  }
]
```

### Retrieve a Course by ID

Endpoint: `GET /api/Courses/{courseId}`

Retrieve a course by its unique ID.

#### Response Payload

```json
{
  "Id": 0,
  "Title": "string",
  "Description": "string",
  "Credits": "string",
  "Instructor": "string",
  "Department": "string",
  "StartDate": "string",
  "EndDate": "string"
}
```

### Update a Course by ID

Endpoint: `PUT /api/Courses/{courseId}`

Update details of a course identified by its ID.

#### Request Payload

```json
{
  "Title": "string",
  "Description": "string",
  "Credits": "string",
  "Instructor": "string",
  "Department": "string",
  "StartDate": "string",
  "EndDate": "string"
}
```

#### Response Payload

```json
{
  "ResponseCode": "string",
  "ResponseMessage": "string"
}
```

### Delete a Course by ID

Endpoint: `DELETE /api/Courses/{courseId}`

Delete a course from the system using its unique ID.

#### Response Payload

```json
{
  "ResponseCode": "string",
  "ResponseMessage": "string"
}
```

## Students Controller

### Create a New Student

Endpoint: `POST /api/Students/CreateStudent`

Register a new student in the system.

#### Request Payload

```json
{
  "FirstName": "string",
  "LastName": "string",
  "DateOfBirth": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "Address": "string",
  "EnrollmentDate": "string",
  "GPA": "string"
}
```

#### Response Payload

```json
{
  "ResponseCode": "string",
  "ResponseMessage": "string"
}
```

### Retrieve All Students

Endpoint: `GET /api/Students`

Retrieve all students registered in the system.

#### Response Payload

```json
[
  {
    "Id": "string",
    "FirstName": "string",
    "LastName": "string",
    "DateOfBirth": "string",
    "Email": "string",
    "PhoneNumber": "string",
    "Address": "string",
    "EnrollmentDate": "string",
    "GPA": "string"
  }
]
```

### Retrieve a Student by ID

Endpoint: `GET /api/Students/{studentId}`

Retrieve details of a student by their unique ID.

#### Response Payload

```json
{
  "Id": "string",
  "FirstName": "string",
  "LastName": "string",
  "DateOfBirth": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "Address": "string",
  "EnrollmentDate": "string",
  "GPA": "string"
}
```

### Update a Student by ID

Endpoint: `PUT /api/Students/{studentId}`

Update details of a student identified by their ID.

#### Request Payload

```json
{
  "FirstName": "string",
  "LastName": "string",
  "DateOfBirth": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "Address": "string",
  "EnrollmentDate": "string",
  "GPA": "string"
}
```

#### Response Payload

```json
{
  "ResponseCode": "string",
  "ResponseMessage": "string"
}
```

### Delete a Student by ID

Endpoint: `DELETE /api/Students/{studentId}`

Delete a student from the system using their unique ID.

#### Response Payload

```json
{
  "ResponseCode": "string",
  "ResponseMessage": "string"
}
```
