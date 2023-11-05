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

## ToDo
* Add role base Authorization (the authorization is already there i just need to prevent users from calling APIs that do functions that the user not suppose to do).
* Make [API CoreServices](AdaptEMS.API/Helpers/CoreServices.cs) and [Web CoreServices](AdaptEMS.Web/Helpers/CoreServices.cs) interface-oriented to facilitate unit testing.
* Make [Validators](AdaptEMS.API/Helpers/Validators.cs) interface-oriented to facilitate unit testing.
* Do small enhancements on the UI:
  - Make the Login and register pages vertically center.
  - Find a way to make the username on the top side menu seems nice.
* Create new Branches to Add Angular and Blazor Clients.
* ((Any advices))

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

use it to help your self with out harming any one.
