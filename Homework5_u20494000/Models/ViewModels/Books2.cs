using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework5_u20494000.Models.ViewModels
{
    public class Books2
    {
        internal int TypeID;
        internal int AuthorID;

        public int BookID { get; set; }
        public string Name { get; set; }
        public int Pages { get; set; }
        public int Point { get; set; }
        public string AuthorSurname { get; set; }
        public string TypeName { get; set; }

        public bool Status { get; set; }
        public int totalBorrows { get; set; }
        public List<Borrowed2> borrowedRecords { get; set; }
        public int studentId { get; set; }
    }
}