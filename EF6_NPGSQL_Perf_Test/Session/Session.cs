using EF6_NPGSQL_Perf_Test.Objects;

namespace EF6_NPGSQL_Perf_Test.Session
{
  class Session
  {
    public Session()
    {
      User = new User();
      UserProfile = new UserProfile();
    }

    /// <summary>
    /// 
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UserProfile UserProfile { get; set; }
  }
}
