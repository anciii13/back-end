﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class ReportedIssuesDatabaseRepository : IReportedIssueRepository
    {
        private readonly ToursContext _dbContext;

        public ReportedIssuesDatabaseRepository(ToursContext dbContext)
        {
            _dbContext = dbContext;
        }


        public ReportedIssue Get(long id)
        {
            var equipment = _dbContext.ReportedIssues.Find(id);
            if (equipment == null) throw new KeyNotFoundException("Not found: " + id);
            return equipment;
        }

        public ReportedIssue Resolve(long id)
        {
            var equipment = Get(id);
            try
            {
                equipment.Resolve();
                _dbContext.ReportedIssues.Update(equipment);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new KeyNotFoundException(e.Message);
            }
            return equipment;
        }
    }
}