using System;
using System.Collections.Generic;
using BerghAdmin.Data;

namespace BerghAdmin.Services.Context
{
    public interface IContextService
    {
        Context Context { get; }
    }
}