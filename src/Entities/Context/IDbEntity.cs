using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQLHelper.Entities.Context
{
    public interface IDbEntity
    {
        public int Id { get; set; }
    }
}