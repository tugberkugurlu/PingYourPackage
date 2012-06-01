using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class PaginatedList<T> : List<T> {

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(IEnumerable<T> source, int pageIndex, int pageSize) {

            //goes to the database to get the total count
            TotalCount = source.Count(); 

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);

            this.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        public bool HasPreviousPage {

            get {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage {

            get {
                return (PageIndex + 1 < TotalPages);
            }
        }
    }
}