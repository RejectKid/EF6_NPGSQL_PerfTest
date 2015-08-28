using System.ComponentModel;
using System.Configuration;

namespace EF6_NPGSQL_Perf_Test.DBHelpers
{
  interface IDbConn : INotifyPropertyChanged
  {
    ConnectionStringSettings GetConnection();
    bool ConnectedServer { get; }
    bool ConnectedLocal { get; }
    ConnectionStringSettings LocalConnectionString { get; }
    ConnectionStringSettings ServerConnectionString { get; }
    bool HasConnection { get; }
    bool HasDummyRan { get; set; }
  }
}
