using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Common.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Core.Common.Mappings
{
  public static class MappingExtensions
  {
    public static Task<PaginatedResponse<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize, Guid? correlationId = null, string message = null)
        => PaginatedResponse<TDestination>.CreateAsync(queryable, pageNumber, pageSize, correlationId, message);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
        => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
  }
}
