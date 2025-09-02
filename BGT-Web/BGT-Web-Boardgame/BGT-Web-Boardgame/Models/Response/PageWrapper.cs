using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BGT_Web_Boardgame.Models.Response
{
    public class PageWrapper<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

        public static PageWrapper<T> Ok(IReadOnlyList<T> Items, string? message = null)
        {
            return new PageWrapper<T>
            {
                IsSuccess = true,
                Message = message,
                Items = Items
            };

        }

        public static PageWrapper<T> Fail(T? result, string? message = null)
        {
            return new PageWrapper<T>
            {
                IsSuccess = false,
                Message = message
            };
        }
    }
}
