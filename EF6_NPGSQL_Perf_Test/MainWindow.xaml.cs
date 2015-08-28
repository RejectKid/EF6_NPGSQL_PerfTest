
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using EF6_NPGSQL_Perf_Test.BindHouse;
using EF6_NPGSQL_Perf_Test.DBHelpers;
using EF6_NPGSQL_Perf_Test.Model;
using EF6_NPGSQL_Perf_Test.Notifier;
using EF6_NPGSQL_Perf_Test.Objects;
using EF6_NPGSQL_Perf_Test.Session;
using Ninject;
using Ninject.Modules;

namespace EF6_NPGSQL_Perf_Test
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    private IDbConn _dbConn;
    private StandardKernel _kernel;
    private readonly Session.Session _session;
    private readonly SessionLogic _sessionLogic;
    private readonly ObservableHandler<IDbConn> flagHandler;


    public MainWindow()
    {
      InitializeComponent();
      DataContext = this;
      var bindings = new Bindings();
      LoadKernel( bindings );
      _session = new Session.Session();
      _sessionLogic = new SessionLogic( _dbConn );

      flagHandler = new ObservableHandler<IDbConn>( _dbConn );
      flagHandler.Add( "HasDummyRan", OnDbConnHasDummyRanChanged );
    }

    private void OnDbConnHasDummyRanChanged( IDbConn obj )
    {
      _coldStartSwitch.IsChecked = _dbConn.HasDummyRan;
    }

    private void LoadKernel( INinjectModule bindings )
    {
      _kernel = new StandardKernel();
      _kernel.Load( bindings );
      _dbConn = _kernel.Get<IDbConn>();
    }

    private void _addButton_OnClick( object sender, RoutedEventArgs e )
    {
      var watch = new Stopwatch();
      for ( var i = 0; i < 100; i++ )
      {
        watch.Start();
        var user = AddUser( i );
        using ( var db = new TestModel( _dbConn.GetConnection() ) )
        {
          db.Users.Add( user );
          db.SaveChanges();
        }
      }
      _addSpeed.Text = "Add Speed: " + watch.ElapsedMilliseconds + "ms";
    }

    private void _loginButton_OnClick( object sender, RoutedEventArgs e )
    {
      var rnd = new Random();
      _session.User.UserName = "test" + rnd.Next( 100 );
      _session.User.Password = "a";
      var watch = new Stopwatch();
      watch.Start();
      _loadProfileButton.IsEnabled = _sessionLogic.TryLogin( _session );
      _loginSpeed.Text = "Login Speed: " + watch.ElapsedMilliseconds + "ms";
    }

    private void _loadProfileButton_OnClick( object sender, RoutedEventArgs e )
    {
      var watch = new Stopwatch();
      watch.Start();
      _sessionLogic.LoadProfile( _session );
      _loadProfileSpeed.Text = "Load Profile Speed: " + watch.ElapsedMilliseconds + "ms";
    }

    #region AddUserStuff
    public User AddUser( int i )
    {
      var rnd = new Random();
      var testAdd = new User();

      //testAdd.IsAdmin = true;
      //testAdd.Password = SimpleHash.ComputeHash( "a", "SHA512", null );
      //testAdd.PasswordResetFlag = true;
      //testAdd.UserName = "adam.buchanan";
      testAdd.IsAdmin = rnd.Next() % 2 == 0;
      testAdd.Password = "a";
      testAdd.PasswordResetFlag = rnd.Next() % 2 == 0;
      testAdd.UserName = "test" + i;
      testAdd.UserProfiles = new List<UserProfile>();
      testAdd.UserProfiles.Add( GimmeUserProfile( testAdd ) );
      return testAdd;
    }

    private UserProfile GimmeUserProfile( User add )
    {
      var testAdd = new UserProfile();
      testAdd.User = add;
      testAdd.CanvasLayout = new List<CanvasLayout>();
      testAdd.CanvasLayout.Add( GimmeCanvasLayout() );
      testAdd.Project = new Project { Name = "Impact" };
      testAdd.Scenario = new Scenario { ScenarioPath = "Impact" };
      testAdd.TileLayout = new List<TileLayout>();
      testAdd.TileLayout.Add( GimmeTileLayout() );
      return testAdd;
    }

    private CanvasLayout GimmeCanvasLayout()
    {
      var testAdd = new CanvasLayout();
      testAdd.Windows = new List<Objects.Window>();
      testAdd.Windows.Add( GimmeWindow() );
      testAdd.Name = "1 Screen - SharpEarth and Tiles";
      return testAdd;
    }

    private Objects.Window GimmeWindow()
    {
      var rnd = new Random();
      var resizeValues = Enum.GetValues( typeof( ResizeMode ) );
      var windowStateValues = Enum.GetValues( typeof( WindowState ) );
      var testAdd = new Objects.Window();
      testAdd.Canvas = new Canvas
      {
        IconSource = "Globe.png",
        Name = "Earth",
        ClassName = "EarthCanvasCustomizer"
      };
      testAdd.OverlaySettings = new OverlaySettings
      {
        Dpi = rnd.NextDouble(),
        IsFeedbackOn = rnd.Next() % 2 == 0,
        IsNotificationOn = rnd.Next() % 2 == 0,
        Offset = 0,
        Width = rnd.Next( 25 )
      };
      testAdd.WindowSettings = new WindowSettings
      {
        Height = rnd.Next( 25 ),
        IsSandboxed = rnd.Next() % 2 == 0,
        Left = rnd.Next( 25 ),
        ResizeMode = (ResizeMode)rnd.Next( resizeValues.Length ),
        ScreenSpan = rnd.Next( 2 ),
        ScreenIndex = 0,
        ScreenState = (WindowState)rnd.Next( windowStateValues.Length ),
        Top = rnd.Next( 25 ),
        Width = rnd.Next( 25 )
      };
      return testAdd;
    }

    private TileLayout GimmeTileLayout()
    {
      var testAdd = new TileLayout();
      testAdd.Name = "Default";
      testAdd.Tiles = new List<Tile>();
      testAdd.Tiles.Add( GimmeTile() );
      return testAdd;
    }

    private Tile GimmeTile()
    {
      var testAdd = new Tile();
      var rnd = new Random();
      testAdd.Settings = new TileSetting
      {
        Row = rnd.Next( 30 ),
        Column = rnd.Next( 30 ),
        Opacity = rnd.NextDouble(),
        IsLocked = rnd.Next() % 2 == 0,
        RowSpan = rnd.Next( 30 ),
        ColSpan = rnd.Next( 30 ),
        Size = rnd.Next( 30 ),
        AssociatedObjectName = ""
      };
      testAdd.TileName = "Fusion.Impact.PlayCalling.Views.PlayCallingTile";
      return testAdd;
    }
    #endregion


  }
}
