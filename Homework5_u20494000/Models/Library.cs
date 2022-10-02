using Homework5_u20494000.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework5_u20494000.Models
{
    public class Library
    {
        private static Library instance;
        public static Library GetLibraryService()
        {
            if (instance == null)
            {
                instance = new Library();
            }
            return instance;
        }
        private SqlConnection buildConnection()
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            sqlConnectionStringBuilder["Data Source"] = ".";
            sqlConnectionStringBuilder["Initial Catalog"] = "Library";
            sqlConnectionStringBuilder["IntergratedSecurity"] = ".";
            return new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
        }

        public bool openConnection()
        {
            using (SqlConnection conn = buildConnection())
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool closeConnection()
        {
            using (SqlConnection conn = buildConnection())
            {
                try
                {
                    conn.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public List<Books2> GetBooks()
        {
            List<Books2> bookList = new List<Books2>();
            String command = "SELECT book.[bookId] as bookId ,book.[name] as name ,book.[pagecount] as pagecount ,book.[point] as point, auth.[surname] as authorSurname ,type.[name] typeName,  book.[authorId],book.[typeId] " +
                            "FROM [Library].[dbo].[books] book " +
                            "JOIN [Library].[dbo].[authors] auth on book.authorId = auth.authorId " +
                            "JOIN [Library].[dbo].[types] type on book.typeId = type.typeId";
            using (SqlConnection conn = buildConnection())
            {
                try
                {
                    OpenConnection();
                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        SqlDataReader readBooks = cmd.ExecuteReader();
                        while (readBooks.Read())
                        {
                            Books2 book = new Books2();
                            book.BookID = (int)readBooks["bookId"];
                            book.Name = (string)readBooks["name"];
                            book.Pages = (int)readBooks["pagecount"];
                            book.Point = (int)readBooks["point"];
                            book.AuthorID = (int)readBooks["authorId"];
                            book.TypeID = (int)readBooks["typeId"];
                            book.AuthorSurname = (string)readBooks["authorSurname"];
                            book.TypeName = (string)readBooks["typeName"];
                            book.Status = true;
                            bookList.Add(book);
                        }
                    }
                    closeConnection();
                }
                catch
                {

                }
            }
            return bookList;
        }

        private void OpenConnection()
        {
            throw new NotImplementedException();
        }

        public List<Students> getAllStudents()
        {
            List<Students> studentList = new List<Students>();
            String command = "SELECT * FROM [Library].[dbo].[students]";
            using (SqlConnection conn = buildConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    SqlDataReader readStudents = cmd.ExecuteReader();
                    while (readStudents.Read())
                    {
                        Students student = new Students();
                        student.StudentID = (int)readStudents["studentId"];
                        student.Name = (string)readStudents["name"];
                        student.Surname = (string)readStudents["surname"];
                        student.Birthday= (DateTime)readStudents["birthdate"];
                        student.Gender = (string)readStudents["gender"];
                        student.Class = (string)readStudents["class"];
                        student.Point = (int)readStudents["point"];
                        studentList.Add(student);
                    }
                }
            }
            return studentList;
        }

        public List<Borrowed> GetAllBorrows()
        {
            List<Borrowed> borrowList = new List<Borrowed>();
            String command = "SELECT * FROM [Library].[dbo].[borrows]";
            using (SqlConnection conn = buildConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    SqlDataReader readBorrows = cmd.ExecuteReader();
                    while (readBorrows.Read())
                    {
                        Borrowed borrow = new Borrowed();
                        borrow.BorrowID = (int)readBorrows["borrowId"];
                        borrow.StudentID = (int)readBorrows["studentId"];
                        borrow.BookID = (int)readBorrows["bookId"];
                        borrow.TakenDate = Convert.ToDateTime(readBorrows["takenDate"]);
                        var broughtDate = readBorrows["broughtDate"].ToString();
                        if (broughtDate != "")
                        {
                            borrow.BroughtDate = Convert.ToDateTime(readBorrows["broughtDate"]);
                        }
                        else
                        {
                            borrow.BroughtDate = null;
                        }

                        borrowList.Add(borrow);
                    }
                }
            }
            return borrowList;
        }

        public SelectList GetTypes()
        {
            List<Type> typesList = new List<Type>();
            String command = "SELECT * FROM [Library].[dbo].[types]";
            using (SqlConnection conn = buildConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    SqlDataReader readTypes = cmd.ExecuteReader();
                    while (readTypes.Read())
                    {
                        Type type = new Type();
                        type.TypeID = (int)readTypes["typeId"];
                        type.Name = (string)readTypes["name"];
                        typesList.Add(type);
                    }
                }
            }
            return new SelectList(typesList, "typeId", "name");
        }

        public SelectList GetAuthors()
        {
            List<Authors> authorsList = new List<Authors>();
            String command = "SELECT * FROM [Library].[dbo].[authors]";
            using (SqlConnection conn = buildConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    SqlDataReader readAuthors = cmd.ExecuteReader();
                    while (readAuthors.Read())
                    {
                        Authors author = new Authors();
                        author.AuthorID = (int)readAuthors["authorId"];
                        author.Name = (string)readAuthors["name"];
                        authorsList.Add(author);
                    }
                }
            }
            return new SelectList(authorsList, "typeId", "name");
        }

    }
}