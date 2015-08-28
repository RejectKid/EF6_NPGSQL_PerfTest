
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EF6_NPGSQL_Perf_Test.Model;

namespace EF6_NPGSQL_Perf_Test.DBHelpers.Impl
{
  class DbConn : Notifier.Notifier, IDbConn
  {
    private bool _hasDummyRan;

    /// <summary>
    /// 
    /// </summary>
    public DbConn()
    {
      var watch = new Stopwatch();
      watch.Start();
      CheckConnectionStrings();
      var context = TaskScheduler.FromCurrentSynchronizationContext();
      if ( !HasDummyRan )
      {
        Task.Factory.StartNew( DummyQuery ).ContinueWith( delegate
        {
          Debug.WriteLine( "DbConn() -- from task: " + watch.ElapsedMilliseconds + "ms" );
          HasDummyRan = true;
        }, context );
      }

      Debug.WriteLine( "DbConn(): " + watch.ElapsedMilliseconds + "ms" );
    }

    public void DummyQuery()
    {
      using ( var db = new TestModel( GetConnection() ) )
      {
        db.Users.FirstAsync();
      }
    }

    /// <summary>
    /// Returns the valid connection needed to query DB.  This method
    /// Prioritizes the server conections over the local connection
    /// </summary>
    /// <returns>connection string</returns>
    public ConnectionStringSettings GetConnection()
    {
      //return ConnectedServer ? ServerConnectionString : LocalConnectionString;
      return LocalConnectionString;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool ConnectedServer { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public bool ConnectedLocal { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public ConnectionStringSettings LocalConnectionString { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public ConnectionStringSettings ServerConnectionString { get; private set; }

    public bool HasConnection
    {
      get { return ConnectedLocal || ConnectedServer; }
    }

    public bool HasDummyRan
    {
      get { return _hasDummyRan; }
      set { SetField( ref _hasDummyRan, value ); }
    }

    private void CheckConnectionStrings()
    {
      var allConnections = ConfigurationManager.ConnectionStrings;
      foreach ( ConnectionStringSettings connection in allConnections )
      {
        if ( connection.ProviderName != "Npgsql" )
          continue;

        var factory = DbProviderFactories.GetFactory( connection.ProviderName );
        var conn = factory.CreateConnection();
        conn.ConnectionString = connection.ConnectionString;
        try
        {
          var tokenSource = new CancellationTokenSource( 4000 );
          var connTask = conn.OpenAsync( tokenSource.Token );
          connTask.Wait();
        }
        catch ( Exception )
        {
        }

        SetDbProperties( connection, conn.State == ConnectionState.Open );
        conn.Close();
      }
    }

    private void SetDbProperties( ConnectionStringSettings connection, bool isConnected )
    {
      if ( connection.Name.StartsWith( "local" ) )
      {
        ConnectedLocal = isConnected;
        LocalConnectionString = connection;
      }
      else if ( connection.Name.StartsWith( "server" ) )
      {
        ConnectedServer = isConnected;
        ServerConnectionString = connection;
      }
    }
  }
}
