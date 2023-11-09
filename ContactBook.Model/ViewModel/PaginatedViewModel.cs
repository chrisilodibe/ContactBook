using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Model.ViewModel
{
    public class PaginatedViewModel
    {
        public int TotalUsers { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public List<User> Users { get; set; }

    }
}
