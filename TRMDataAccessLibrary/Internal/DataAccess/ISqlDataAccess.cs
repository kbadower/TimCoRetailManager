using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace TRMDataAccessLibrary.Internal.DataAccess
{
    public interface ISqlDataAccess
    {
        IConfiguration Configuration { get; }

        void CommitTransaction();
        string GetConnectionString(string name);
        List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);
        void RollbackTransatction();
        void SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
        void SaveDataInTransaction<T>(string storedProcedure, T parameters);
        void StartTransaction(string connectionStringName);

        void Dispose();
    }
}