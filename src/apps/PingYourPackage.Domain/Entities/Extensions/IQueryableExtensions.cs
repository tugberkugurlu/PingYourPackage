using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.Domain.Entities {
    
    public static class IQueryableExtensions {

        public static PaginatedList<T> ToPaginatedList<T>(
            this IQueryable<T> query, 
            int pageIndex, 
            int pageSize) where T : IEntity {

            var baseQuery = query;
            query = query.Skip(
                (pageIndex - 1) * pageSize).Take(pageSize);

            var totalCount = baseQuery.Count();
            return new PaginatedList<T>(
                pageIndex, pageSize, totalCount, query);
        }
    }
}