﻿using WebsiteTester.Domain.Models;

namespace WebsiteTester.Application.Common.Interfaces
{
    public interface IWebsiteTesterDbContext
    {
        IQueryable<Link> Links { get; }
        IQueryable<LinkTestResult> LinkTestResults { get; }

        void Add<T>(T link) where T : class;
        void AddRange<T>(IEnumerable<T> links) where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
