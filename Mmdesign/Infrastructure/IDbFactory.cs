using Mmdesign.Models;
using System;

namespace Mmdesign.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        MyContextDb Init();
    }
}