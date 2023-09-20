<h2>Online Library ASP.NET Core 6 Web Application</h2>
<p>This project is an Online Library web application built using ASP.NET Core 6 and Microsoft SQL Server for the database. The application supports two types of users: admin and regular users (students and teachers). Admins have the ability to manage books, view users, and perform administrative actions, while regular users can borrow books and manage their account.
</p>

<p>Features</p>
Admin Features:
<ul>
  <li>Add, edit, and remove books.</li>
  <li>View registered users.</li>
  <li>Deactivate user accounts if a book is not returned within 30 days.</li>
</ul> 
  
User Features:
<ul>
  <li>Borrow books (3 books for students, 2 books for teachers) for 30 days.</li>
  <li>Pay charges (Rs 5 for students, Rs 10 for teachers) if books are not returned within 30 days.</li>
</ul>
<br/>

<h2>Technology Stack:</h2><br/>
<strong>ASP.NET Core 6:</strong>
 <li>Framework for building web applications and APIs using .NET.</li> 
 <br/>
<strong>Microsoft SQL Server:</strong>
  <li>Relational database management system used for storing application data.</li>

<br/>
As an Admin:
<ul>
  <li>Log in with admin credentials.</li>
  <li>username: Admin</li>
  <li>password: Admin</li>
  <li>Manage books (add, edit, remove).</li>
  <li>View registered users and take actions.</li>
  <li>Deactivate user accounts for overdue books.</li>
</ul> 
  

As a User (Student/Teacher):
<ul>  
  <li>Creat user account</li>
  <li> Log in with user credentials.</li>
  <li>Borrow books (2 for students, 3 for teachers) for 30 days.</li>
  <li> Return books within the due date to avoid charges.</li>
</ul> 
 

Contributing
  <li>Contributions are welcome! Please create an issue or pull request if you find any bugs or want to suggest enhancements.</li>

License
 <li>This project is licensed under the MIT License - see the LICENSE file for details.</li>

Acknowledgments
 <li>Special thanks to the ASP.NET Core and Microsoft SQL Server communities for their support and contributions to the development of this application.</li>
