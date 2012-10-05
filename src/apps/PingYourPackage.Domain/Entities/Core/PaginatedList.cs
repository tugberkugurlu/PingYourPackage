using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {

    public class PaginatedList<T> : List<T> where T : IEntity {

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPageCount { get; private set; }

        public PaginatedList(
            int pageIndex, int pageSize, 
            int totalCount, IQueryable<T> source) {

            AddRange(source);

            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = 
                (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public bool HasPreviousPage {

            get {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage {

            get {
                return (PageIndex < TotalPageCount);
            }
        }
    }
}