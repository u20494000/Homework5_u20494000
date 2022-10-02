using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework5_u20494000.Models;
using Homework5_u20494000.Models.ViewModels;
using Type = System.Type;

namespace Homework5_u20494000.Controllers
{
    public class HomeController : Controller


    {
        //Connection to Library Database:
        
        SqlConnection connectionstring = new SqlConnection("Data Source=.;Ininitial Catalog=Library;Intergrated Security=True;");

        //Display Book List:
        public static List<Books2> Books = new List<Books2>();
        //Borrowed book List:
        public static List<Borrowed> Borrows = new List<Borrowed>();
        //Books that are borrowed at the moment:
        public static List<BorrowedBooks> PresentlyBorrowedBooks = new List<BorrowedBooks>();
        //Student list:
        




        // GET: Home
        public ActionResult Index()
        {
            //Capture book count:
            //Book count if statement:
            //Catch error and finally close connectionstring:
            if(Books.Count==0)
            {
                try
                {
                    SqlCommand GetBooks = new SqlCommand("SELECT * book.[bookId] as BookID,book.[name] as Name,book.[pagecount] as Pages,book.[point] as Point,auth.[surname],type.[name] as TypeName FROM [Library].[dbo].[books]book JOIN [Library].[dbo].[authors] auth on book.authorId=auth.authorId JOIN [Library].[dbo].[types] type on book.typeId=type.typeId", connectionstring);
                    connectionstring.Open();
                    SqlDataReader TrackBooks = GetBooks.ExecuteReader();
                    while (TrackBooks.Read())
                    {
                        Books2 book = new Books2();
                        book.BookID = (int)TrackBooks["bookId"];
                        book.Name = (string)TrackBooks["name"];
                        book.Pages = (int)TrackBooks["pagecount"];
                        book.Point = (int)TrackBooks["point"];
                        book.AuthorSurname = (string)TrackBooks["authorSurname"];
                        book.TypeName = (string)TrackBooks["TypeName"];
                        book.Status = true;
                        Books.Add(book);
                        connectionstring.Close();
                        GetAllBorrowed();

                        //Manages where abouts of borrowed books:
                        //for loop that loops through all books:
                        for(int i=0; i < Borrows.Count; i++)
                        {
                            if (Borrows[i].BroughtDate == null)
                            {
                                PresentlyBorrowedBooks.Add(new BorrowedBooks(Borrows[i].BookID));
                            }


                        }
                        for (int i = 0; i < Borrows.Count; i++)
                        {
                            //for loop for individual borrowed books:
                            for(int j = 0; j < PresentlyBorrowedBooks.Count; j++)
                            {
                                if (Books[i].BookID == PresentlyBorrowedBooks[j].BookID)
                                {
                                    Books[i].Status = false;
                                }
                            }
                        }
                    }
                }
                catch(Exception error)
                {
                    ViewBag.Message = error.Message;
                }
                finally
                {
                    connectionstring.Close();
                }
            }
            return View();
        }

        //Functions and methods:
        private void GetAllBorrowed()
        {
            SqlCommand GetBorrowed = new SqlCommand("SELECT * FROM [Library].[dbo].[borrows]", connectionstring);
            connectionstring.Open();
            SqlDataReader TrackBorrowed = GetBorrowed.ExecuteReader();
            while (TrackBorrowed.Read())
            {
                Borrowed borrow = new Borrowed();
                borrow.BorrowID = (int)TrackBorrowed["borrowId"];
                borrow.StudentID = (int)TrackBorrowed["studentId"];
                borrow.BookID = (int)TrackBorrowed["bookId"];
                borrow.TakenDate= (DateTime)TrackBorrowed["takenDate"];
                borrow.BroughtDate = (DateTime)TrackBorrowed["broughtDate"];
                Borrows.Add(borrow);
            }
            connectionstring.Close();
           

        }
        private void GetStudent()
        {
            SqlCommand getAllStudents = new SqlCommand("SELECT * FROM [Library].[dbo].[students]", connectionstring);
            connectionstring.Open();
            SqlDataReader readStudents = getAllStudents.ExecuteReader();
            while (readStudents.Read())
            {
                Students student = new Students();
                student.StudentID = (int)readStudents["studentId"];
                student.Name = (string)readStudents["name"];
                student.Surname = (string)readStudents["surname"];
                student.Birthday = (DateTime)readStudents["birthdate"];
                student.Gender = (string)readStudents["gender"];
                student.Class = (string)readStudents["class"];
                student.Point = (int)readStudents["point"];
                Students.Add(student);
            }
            connectionstring.Close();
        }
        private SelectList GetAuthors()
        {
            List<Authors> authors = new List<Authors>();
            SqlCommand getAllAuthors = new SqlCommand("SELECT * FROM [Library].[dbo].[authors]", connectionstring);
            connectionstring.Open();
            SqlDataReader readAuthors = getAllAuthors.ExecuteReader();
            while (readAuthors.Read())
            {
                Authors author = new Authors();
                author.AuthorID = (int)readAuthors["authorId"];
                author.Name = (string)readAuthors["name"];
                authors.Add(author);
            }
            return new SelectList(authors, "authorId", "name");
        }
        private SelectList GetType()
        {
            List<Type> types = new List<Type>();
            SqlCommand getAllTypes = new SqlCommand("SELECT * FROM [Library].[dbo].[types]", connectionstring);
            connectionstring.Open();
            SqlDataReader readTypes = getAllTypes.ExecuteReader();
            while (readTypes.Read())
            {
                Type type = new Type();
                type.TypeID = (int)readTypes["typeId"];
                type.Name = (string)readTypes["name"];
                types.Add(type);
            }
            connectionstring.Close();
            return new SelectList(types, "typeId", "name");
        }
        public static List<Borrowed> Borrowed = new List<Borrowed>();
        public static List<Books2> Searched = null;
        public static int CheckingBook = 0;

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            List<Books2> returnBooks = null;
            try
            {
                Books.Clear();
                Borrows.Clear();
                if (Searched != null)
                {
                    returnBooks = Searched;
                }
                else
                {
                    Books = db.getAllBooks();
                    Students = db.getAllStudents();
                    Borrows = db.getAllBorrows();
                    for (int i = 0; i < Borrows.Count; i++)
                    {
                        if (Borrows[i].BroughtDate == null)
                        {
                            Books2 book = Books.Where(x => x.BookID == Borrows[i].BookID).FirstOrDefault();
                            book.Status = false;
                            book.studentId = (int)Borrows[i].StudentID;
                        }
                    }
                    returnBooks = Books;
                }


                ViewBag.Types = db.GetTypes();

                ViewBag.Authors = db.GetAuthors();

            }
            catch (Exception message)
            {
                ViewBag.Message = message.Message;
            }
            finally
            {
                db.closeConnection();
            }

            return View(returnBooks);
        }

        [HttpGet]
        public ActionResult Clear()
        {
            Searched.Clear();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Search(string name, int? typeId, int? authorId)
        {
            try
            {
                if (Searched != null)
                {
                    Searched.Clear();
                }
                if (name != "" && typeId != null && authorId != null)
                {
                    //search has to be on all 3 parameters
                    GetAllBooks();
                    Searched = Books.Where(x => x.Name == name && x.TypeID == typeId && x.AuthorID == authorId).ToList();
                }
                else if (name != "" && typeId != null && authorId == null)
                {
                    // search on name and type 
                }
                else if (name != "" && typeId == null && authorId != null)
                {
                    // search on name and author
                }
                else if (name == "" && typeId != null && authorId != null)
                {
                    //search on type and author
                }
                else if (name == "" && typeId == null && authorId != null)
                {
                    //search on author
                }
                else if (name == "" && typeId != null && authorId == null)
                {
                    // search on type
                    Searched = Books.Where(x => x.TypeID == typeId).ToList();
                }
                else if (name != "" && typeId == null && authorId == null)
                {
                    // search on name 
                }
                else
                {
                    TempData["Message"] = "You Didnt Search For Anything Stop Wasting my Time !!!";
                }
            }
            catch (Exception message)
            {
                TempData["Message"] = message;
            }

            return RedirectToAction("Index");
        }

        private void GetAllBooks()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Details(int bookId)
        {
            Books2 bookInList = Books.Where(x => x.BookID == bookId).FirstOrDefault();
            if (bookInList != null)
            {
                var AllRecordsOfBookInBorrows = Borrows.Where(x => x.BookID == bookId).ToList();
                bookInList.totalBorrows = AllRecordsOfBookInBorrows.Count();
                List<Borrowed2> RecordOfBorrowed = new List<Borrowed2>();
                for (int i = 0; i < AllRecordsOfBookInBorrows.Count(); i++)
                {
                    Borrowed2 record = new Borrowed2();
                    record.BorrowID = AllRecordsOfBookInBorrows[i].BorrowID;
                    record.StudentName = Students.Where(x => x.StudentID== AllRecordsOfBookInBorrows[i].StudentID).FirstOrDefault().name;
                    record.TakenDate = AllRecordsOfBookInBorrows[i].TakenDate;
                    record.BroughtDate = AllRecordsOfBookInBorrows[i].BroughtDate;
                    RecordOfBorrowed.Add(record);
                }
                bookInList.borrowedRecords = RecordOfBorrowed;
            }
            else
            {
                ViewBag.Message = "Book Not Found";
            }
            return View(bookInList);
        }

        [HttpGet]
        public ActionResult ViewStudents(int bookId)
        {
            Books2 book = Books.Where(x => x.BookID == bookId).FirstOrDefault();
            ViewBag.Status = book.Status;
            if (book.studentId != 0)
            {
                ViewBag.studentId = book.studentId;
            }
            else
            {
                ViewBag.studentId = 0;
            }
            CheckingBook = 0;
            CheckingBook = bookId;
            return View(Students);
        }
    }
}