using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using EF6_NPGSQL_Perf_Test.DBHelpers;
using EF6_NPGSQL_Perf_Test.Model;
using EF6_NPGSQL_Perf_Test.Objects;

namespace EF6_NPGSQL_Perf_Test.Session
{
  class SessionLogic
  {
    private readonly IDbConn _dbConn;

    public SessionLogic( IDbConn dbConn )
    {
      _dbConn = dbConn;
    }

    public void ChangePassword( Session session )
    {
      var curUser = session.User;
      curUser.Password = curUser.Password;
      curUser.PasswordResetFlag = false;
      var watch = new Stopwatch();
      watch.Start();
      using ( var db = new TestModel( _dbConn.GetConnection() ) )
      {
        db.Users.AddOrUpdate( curUser );
        db.SaveChanges();
      }
      Debug.WriteLine( "ChangePassword: " + watch.ElapsedMilliseconds + "ms" );
    }

    public void SaveUserProfile( Session session )
    {
      throw new System.NotImplementedException();
    }

    public bool TryLogin( Session session )
    {
      User queriedUser;
      var watch = new Stopwatch();
      watch.Start();
      using ( var db = new TestModel( _dbConn.GetConnection() ) )
      {
        queriedUser = db.Users.FirstOrDefault( o => o.UserName == session.User.UserName );
      }
      Debug.WriteLine( "TryLogin: " + watch.ElapsedMilliseconds + "ms" );

      //Set password if found and verify it
      string hashedPassword;
      if ( queriedUser != null )
        hashedPassword = queriedUser.Password;
      else
        return false;

      var returnMe = session.User.Password == hashedPassword;

      //If verified set session User to returned user
      if ( returnMe )
        session.User = queriedUser;

      return returnMe;
    }

    // Todo: need to refactor code to return list of profiles and display them accordingly
    /// <summary>
    /// Query Loads the most recent Profile
    /// </summary>
    /// <param name="session"></param>
    public void LoadProfile( Session session )
    {
      var watch = new Stopwatch();
      watch.Start();
      UserProfile queriedUserProfile;
      using ( var db = new TestModel( _dbConn.GetConnection() ) )
      {
        queriedUserProfile = db.UserProfiles
          .AsNoTracking()
          .OrderByDescending( o => o.DateModified )
          .Include( o => o.Scenario )
          .Include( o => o.Project )
          .Include( o => o.CanvasLayout
            .Select( op => op.Windows
            .Select( opq => opq.Canvas ) ) )
          .FirstOrDefault( o => o.User.Id == session.User.Id );
      }
      Debug.WriteLine( "LoadProfile: " + watch.ElapsedMilliseconds + "ms" );
      if ( queriedUserProfile == null )
        return;

      session.UserProfile = queriedUserProfile;
    }
  }
}
