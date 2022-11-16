using System.Data;
using SQLHelper.Entities;

namespace SQLHelper.TableManagers
{
    public class DbRelationalTableManager<T1, T2, TRegistryModel> : DbTableOperationManager<TRegistryModel>, ITableOperationManager<TRegistryModel>
    where T1 : BaseEntity, new()
    where T2 : BaseEntity, new()
    where TRegistryModel : BaseRelationalEntity, new()
    {
        public DbRelationalTableManager(SqlDbContext context, DataRow row) : base(context, row)
        {

        }
    }
}