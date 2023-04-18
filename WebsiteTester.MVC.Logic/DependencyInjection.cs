﻿using Microsoft.Extensions.DependencyInjection;
using WebsiteTester.MVC.Logic.Services;

namespace WebsiteTester.MVC.Logic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMvcLogic(this IServiceCollection services)
        {
            services.AddScoped<ResultsSaverService>();
            services.AddScoped<ResultsReceiverService>();

            return services;
        }
    }
}