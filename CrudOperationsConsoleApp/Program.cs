using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CrudOperationsConsoleApp
{
    public class Student
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public  required string LastName { get; set; }
        public int Class { get; set; }
        public required string SchoolName { get; set; }
        public required string HomeAddress { get; set; }
    }

    public class StudentOperations
    {
        private readonly string ConnStr = "Server=DELL\\SQLEXPRESS; Database=StudentDetails; Trusted_Connection=True; Encrypt=True; TrustServerCertificate=True;";

        // Method to Add Student to Database
        public void AddStudent(Student stu)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConnStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Student (FirstName, LastName, Class, SchoolName, HomeAddress) " +
                    "VALUES (@FirstName, @LastName, @Class, @SchoolName, @HomeAddress)", sqlconn);

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@FirstName", stu.FirstName);
                cmd.Parameters.AddWithValue("@LastName", stu.LastName);
                cmd.Parameters.AddWithValue("@Class", stu.Class);
                cmd.Parameters.AddWithValue("@SchoolName", stu.SchoolName);
                cmd.Parameters.AddWithValue("@HomeAddress", stu.HomeAddress);

                sqlconn.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Student record added successfully!");
            }
        }

        // Method to Display All Students
        public void DisplayStudents()
        {
            List<Student> studentlist = new List<Student>();

            using (SqlConnection sqlconn = new SqlConnection(ConnStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Student ORDER BY Id", sqlconn);
                cmd.CommandType = CommandType.Text;

                sqlconn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Student stu = new Student
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Class = Convert.ToInt32(reader["Class"]),
                        SchoolName = reader["SchoolName"].ToString(),
                        HomeAddress = reader["HomeAddress"].ToString(),
                    };

                    studentlist.Add(stu);
                }

                Console.WriteLine("\nStudent Details:");
                Console.WriteLine("--------------------------------------------------");
                foreach (Student stu in studentlist)
                {
                    Console.WriteLine($"ID: {stu.Id}");
                    Console.WriteLine($"First Name: {stu.FirstName}");
                    Console.WriteLine($"Last Name: {stu.LastName}");
                    Console.WriteLine($"Class: {stu.Class}");
                    Console.WriteLine($"School Name: {stu.SchoolName}");
                    Console.WriteLine($"Home Address: {stu.HomeAddress}");
                    Console.WriteLine("--------------------------------------------------");
                }

            }
        }

        // Method to Delete Student
        public void DeleteStudent(int Id)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConnStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Student WHERE Id=@Id", sqlconn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", Id);

                sqlconn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    Console.WriteLine($"Student with ID {Id} has been deleted successfully!");
                else
                    Console.WriteLine($"No student found with ID {Id}.");
            }
        }

        // Method to Update Student
        public void UpdateStudent(Student stu)
        {
            using (SqlConnection sqlconn = new SqlConnection(ConnStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Student SET FirstName = @FirstName, LastName = @LastName, Class = @Class, SchoolName = @SchoolName, HomeAddress = @HomeAddress WHERE Id = @Id", sqlconn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@Id", stu.Id);
                cmd.Parameters.AddWithValue("@FirstName", stu.FirstName);
                cmd.Parameters.AddWithValue("@LastName", stu.LastName);
                cmd.Parameters.AddWithValue("@Class", stu.Class);
                cmd.Parameters.AddWithValue("@SchoolName", stu.SchoolName);
                cmd.Parameters.AddWithValue("@HomeAddress", stu.HomeAddress);

                sqlconn.Open();
                cmd.ExecuteNonQuery();

                Console.WriteLine("Student record updated successfully!");
            }
        }

        // Method to Get Student by ID
        public Student GetStudentById(int Id)
        {
            Student stu = null;

            using (SqlConnection sqlconn = new SqlConnection(ConnStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Student WHERE Id=@Id", sqlconn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", Id);

                sqlconn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    stu = new Student
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Class = Convert.ToInt32(reader["Class"]),
                        SchoolName = reader["SchoolName"].ToString(),
                        HomeAddress = reader["HomeAddress"].ToString(),
                    };
                }
            }

            return stu;
        }

        // Method to Update Student Details
        public void UpdateStudentDetails()
        {
            Console.Write("Enter the Student ID you want to update: ");
            int Id = Convert.ToInt32(Console.ReadLine());

            Student stu = GetStudentById(Id);

            if (stu != null)
            {
                Console.WriteLine("\nExisting Student Details:");
                Console.WriteLine($"First Name: {stu.FirstName}");
                Console.WriteLine($"Last Name: {stu.LastName}");
                Console.WriteLine($"Class: {stu.Class}");
                Console.WriteLine($"School Name: {stu.SchoolName}");
                Console.WriteLine($"Home Address: {stu.HomeAddress}");

                Console.WriteLine("\nEnter New Details (Press Enter to keep existing values):");

                Console.Write("First Name: ");
                string firstName = Console.ReadLine();
                stu.FirstName = string.IsNullOrEmpty(firstName) ? stu.FirstName : firstName;

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine();
                stu.LastName = string.IsNullOrEmpty(lastName) ? stu.LastName : lastName;

                Console.Write("Class: ");
                string classInput = Console.ReadLine();
                stu.Class = string.IsNullOrEmpty(classInput) ? stu.Class : Convert.ToInt32(classInput);
                //Console.WriteLine("Invalid input. Please enter a valid integer for class.");
                    
                Console.Write("School Name: ");
                string schoolName = Console.ReadLine();
                stu.SchoolName = string.IsNullOrEmpty(schoolName) ? stu.SchoolName : schoolName;

                Console.Write("Home Address: ");
                string homeAddress = Console.ReadLine();
                stu.HomeAddress = string.IsNullOrEmpty(homeAddress) ? stu.HomeAddress : homeAddress;

                UpdateStudent(stu);
              }
            else
            {
                Console.WriteLine($"Student with ID {Id} not found.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            StudentOperations studentOperations = new StudentOperations();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nChoose an Option:");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Display All Students");
                Console.WriteLine("3. Delete Student by ID");
                Console.WriteLine("4. Update Student");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter First Name: ");
                        string firstName = Console.ReadLine();

                        Console.Write("Enter Last Name: ");
                        string lastName = Console.ReadLine();

                        Console.Write("Enter Class: ");
                        int studentClass = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Enter School Name: ");
                        string schoolName = Console.ReadLine();

                        Console.Write("Enter Home Address: ");
                        string homeAddress = Console.ReadLine();

                        // Create and add the student object
                        Student newStudent = new Student
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            Class = studentClass,
                            SchoolName = schoolName,
                            HomeAddress = homeAddress
                        };

                        studentOperations.AddStudent(newStudent);

                        break;

                    case 2:
                        studentOperations.DisplayStudents();
                        break;

                    case 3:
                        Console.Write("Enter Student ID to Delete: ");
                        int deleteId = Convert.ToInt32(Console.ReadLine());
                        studentOperations.DeleteStudent(deleteId);
                        break;

                    case 4:                   
                        studentOperations.UpdateStudentDetails();
                        break;

                    case 5:
                        exit = true;
                        Console.WriteLine("Exiting the application...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}
