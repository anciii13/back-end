﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        List<Tour> GetToursByAuthor(int id);
    }
}
