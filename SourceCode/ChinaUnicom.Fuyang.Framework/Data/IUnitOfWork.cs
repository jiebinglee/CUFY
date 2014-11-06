using System;

namespace ChinaUnicom.Fuyang.Framework.Data
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsCommitted { get; }
        int Commit(bool validateOnSaveEnabled = true);
        void Rollback();
    }
}
