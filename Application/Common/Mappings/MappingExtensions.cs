using Application.Common.Models;
using System.Linq;

namespace Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static PaginatedList<TDestination> MappedPaginatedList<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) => PaginatedList<TDestination>.Create(queryable, pageNumber, pageSize);//Expression-bodied members

    }
}


