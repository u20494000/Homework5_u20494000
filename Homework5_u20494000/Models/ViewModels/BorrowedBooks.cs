using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework5_u20494000.Models.ViewModels
{
    public class BorrowedBooks
    {
        public int BookID { get; set; }

        public BorrowedBooks(int bookId)
        {
            this.BookID = bookId;
        }
    }
}