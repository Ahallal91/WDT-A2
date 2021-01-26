using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Model.Repository
{
    /*
     * Reference: Week 9 WebAPI
     */
    public interface IDataRepository<TEntity, TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAll(TKey id);
        TKey Update(TKey id, TEntity item);
    }
}
