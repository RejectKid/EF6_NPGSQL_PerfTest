using System;
using System.Configuration;
using System.Linq;
using EF6_NPGSQL_Perf_Test.DBHelpers;
using EF6_NPGSQL_Perf_Test.Objects;

namespace EF6_NPGSQL_Perf_Test.Model
{
  using System.Data.Entity;

  public class TestModel : DbContext
  {
    // Your context has been configured to use a 'TestModel' connection string from your application's 
    // configuration file (App.config or Web.config). By default, this connection string targets the 
    // 'EF6_NPGSQL_Perf_Test.TestModel' database on your LocalDb instance. 
    // 
    // If you wish to target a different database and/or database provider, modify the 'TestModel' 
    // connection string in the application configuration file.
    public TestModel( ConnectionStringSettings getConnectionString )
      : base( getConnectionString.Name )
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating( DbModelBuilder modelBuilder )
    {
      modelBuilder.Types().Configure( o => o.Ignore( "IsDirty" ) );
      modelBuilder.Entity<Scenario>().Ignore( o => o.FullScenarioPath );

      base.OnModelCreating( modelBuilder );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override int SaveChanges()
    {
      var result = SaveModificationHistory();
      return result;
    }

    /// <summary>
    /// This is to automatically update/add "dateModified" and "dateCreated" columns to keep history of all rows
    /// </summary>
    /// <returns></returns>
    private int SaveModificationHistory()
    {
      var changesMade = ChangeTracker
        .Entries()
        .Where( e => e.Entity is IModHistory && ( e.State == EntityState.Added || e.State == EntityState.Modified ) )
        .Select( e => e.Entity as IModHistory );

      var allModHistories = ChangeTracker
        .Entries()
        .Where( e => e.Entity is IModHistory )
        .Select( e => e.Entity as IModHistory );

      foreach ( var history in changesMade )
      {
        history.DateModified = DateTime.Now;
        if ( history.DateCreated == DateTime.MinValue )
          history.DateCreated = DateTime.Now;
      }

      var result = base.SaveChanges();

      foreach ( var history in allModHistories )
      {
        history.IsDirty = false;
      }
      return result;
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<Canvas> Canvases { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<CanvasLayout> CanvasLayouts { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<OverlaySettings> OverlaySettings { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<Project> Projects { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<Scenario> Scenarios { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<Tile> Tiles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<TileLayout> TileLayouts { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<TileSetting> TileSettings { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<Window> Windows { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual DbSet<WindowSettings> WindowSettings { get; set; }
  }
}