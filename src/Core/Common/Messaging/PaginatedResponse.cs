using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Core.Common.Messaging
{
  public class PaginatedResponse<T> : BaseResponse
  {
    public IEnumerable<T> Result { get; set; }
    public int? PageIndex { get; set; }
    public int? TotalPages { get; set; }
    public int? TotalCount { get; set; }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PaginatedResponse()
    {
    }

    public PaginatedResponse(IEnumerable<T> result, int count, int pageIndex, int pageSize, string message = null)
    {
      PageIndex = pageIndex;
      TotalPages = (int)Math.Ceiling(count / (double)pageSize);
      TotalCount = count;
      Result = result;
      Success(message);
    }

    public PaginatedResponse(IEnumerable<T> result, int count, int pageIndex, int pageSize, Guid correlationId, string message = null) : base(correlationId)
    {
      PageIndex = pageIndex;
      TotalPages = (int)Math.Ceiling(count / (double)pageSize);
      TotalCount = count;
      Result = result;
      Success(message);
    }

    public static async Task<PaginatedResponse<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, Guid? correlationId = null, string message = null)
    {
      var count = await source.CountAsync();
      var result = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

      if (correlationId == null)
      {
        return new PaginatedResponse<T>(result, count, pageIndex, pageSize, message);
      }
      else
      {
        return new PaginatedResponse<T>(result, count, pageIndex, pageSize, correlationId.Value, message);
      }
    }
  }
}
