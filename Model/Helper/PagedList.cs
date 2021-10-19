using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Model.Helper
{
    public class PagedList<T> : List<T>
    {
        public PagedList(List<T> collection, int currentPage, int totalItems, int itemsPerPage)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling(totalItems / (itemsPerPage * 1.0));
            this.AddRange(collection);
        }

        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public static async Task<PagedList<T>> CreatePagingListAsync(IQueryable<T> source, int currentPage, int itemsPerPage)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
            return new PagedList<T>(items, currentPage, count, itemsPerPage);
        }
    }
}