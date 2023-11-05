# EMS

Employees Management System is a simple application for managing employees.

## Purpose
Is self learning project to enhance structural and UI skills.
  
## Roles
There are two roles:
* Admin
* Employee

## Functionality
The project now have those functions:
* Create Employee (Admin)
* Confirm Registered Employee(Admin)
* Register Employee(Employee)
* Create leave request(Employee)
* Approve or Reject leave request(Admin)

## Installation

Clone the project and make sure you have SQL server on your machine and have a (create database) permission or change the connection string according to your environment then run it.

## Technical Details
### Used Technologies
* .NET Core 7
* SQL
* HTML
* CSS
* Javascript
* Bootstrap

### Overall structure
There are projects inside the solution:
* API: web api project contains all business logic, database communication and Validations.
* WEB web project contains the UI.
* Entities Contains the models for the solution.

any code that implements the business should be inside CoreService class or validator class for validation process.

## ToDo
* Add role base Authorization (the authorization is already there i just need to prevent users from calling APIs that do functions that the user not suppose to do).
* Make [API CoreServices](AdaptEMS.API/Helpers/CoreServices.cs) and [Web CoreServices](AdaptEMS.Web/Helpers/CoreServices.cs) interface-oriented to facilitate unit testing.
* Make [Validators](AdaptEMS.API/Helpers/Validators.cs) interface-oriented to facilitate unit testing.
* Do small enhancements on the UI:
  - Make the Login and register pages vertically center.
  - Find a way to make the username on the top side menu seems nice.
* Add Comments to eazily create clear documentation.
* Add Test Project to put Automated Tests inside it.
* Create new Branches to Add Angular and Blazor Clients.
* ((Any advices))

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

use it to help your self with out harming any one.
