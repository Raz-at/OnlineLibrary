Online Library ASP.NET Core 6 Web Application
This project is an Online Library web application built using ASP.NET Core 6 and Microsoft SQL Server for the database. The application supports two types of users: admin and regular users (students and teachers). Admins have the ability to manage books, view users, and perform administrative actions, while regular users can borrow books and manage their account.

Features
Admin Features:
  Add, edit, and remove books.
  View registered users.
  Deactivate user accounts if a book is not returned within 30 days.
  
User Features:
  Borrow books (3 books for students, 2 books for teachers) for 30 days.
  Pay charges (Rs 5 for students, Rs 10 for teachers) if books are not returned within 30 days.
  
Technology Stack
ASP.NET Core 6:
  Framework for building web applications and APIs using .NET.
Microsoft SQL Server:
  Relational database management system used for storing application data.


As an Admin:
  Log in with admin credentials.
    username: Admin
    password: Admin
  Manage books (add, edit, remove).
  View registered users and take actions.
  Deactivate user accounts for overdue books.

As a User (Student/Teacher):
  Log in with user credentials.
  Borrow books (2 for students, 3 for teachers) for 30 days.
  Return books within the due date to avoid charges.

Contributing
  Contributions are welcome! Please create an issue or pull request if you find any bugs or want to suggest enhancements.

License
  This project is licensed under the MIT License - see the LICENSE file for details.

Acknowledgments
  Special thanks to the ASP.NET Core and Microsoft SQL Server communities for their support and contributions to the development of this application.
