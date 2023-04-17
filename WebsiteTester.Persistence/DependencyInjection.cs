﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebsiteTester.Persistenсe
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebsiteTesterPersistenсe(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WebsiteTesterDbContext>(options =>
                           options.UseSqlServer(connectionString));

            return services;
        }
    }
}
