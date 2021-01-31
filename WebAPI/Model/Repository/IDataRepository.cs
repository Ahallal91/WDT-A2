using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Model.Repository
{
    /*
     * Reference: Week 9 WebAPI
     * Simple Data repository interface which supports GetAll of a type as a list, GetAll by that objects ID and Updating 
     * a specific Object by its ID.
     * These three methods allow admin features of getting all Transactions, Users and billpays. Selecting a specific/Multiple
     * Object of that ID type where an edit may occur or to display id's and then the update method supports
     * updating accounts and billpays for blocking feature.
     */
    public interface IDataRepository<TEntity, TKey> where TEntity : class
    {
        List<TEntity> GetAll();
        List<TEntity> GetAllByID(TKey id);
        List<TEntity> GetAllByIDWithDate(TKey id, DateTime startDate, DateTime endDate);
        TKey Update(TKey id, TEntity item);
    }
}
