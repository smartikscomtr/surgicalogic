using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Model.InputModel
{
    public class GridInputModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string SortBy { get; set; }
        public bool? Descending { get; set; }
   
    }
}
