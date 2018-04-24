using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using ChemistShopSite.Models;

namespace ChemistShopSite
{
    public class DbCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;

        public DbCacheMiddleware(RequestDelegate next, IMemoryCache memCache)
        {
            this._next = next;
            this._memoryCache = memCache;
        }

        public async Task Invoke(HttpContext context, MedicamentsContext db)
        {
            string path = context.Request.Path.Value.ToLower();
            object model = null;
            switch (path)
            {
                case "/":
                    model = db.Medicaments.Last();
                    break;
                case "/home/index":
                    model = db.Medicaments.Last();
                    break;
                case "/home/reception":
                    model = db.Receptions.Last();
                    break;
                case "/home/consumption":
                    model = db.Consumptions.Last();
                    break;
            }
            var modelTemp = model;
            if (!_memoryCache.TryGetValue(path, out modelTemp))
            {
                _memoryCache.Set(path, model,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            await _next.Invoke(context);
        }
    }

    public static class DbCacheExtensions
    {
        public static IApplicationBuilder UseLastElementCache(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbCacheMiddleware>();
        }
    }
}
