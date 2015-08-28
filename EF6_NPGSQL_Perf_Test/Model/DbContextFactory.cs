using System.Configuration;
using System.Data.Entity.Infrastructure;

namespace EF6_NPGSQL_Perf_Test.Model
{
  class DbContextFactory : IDbContextFactory<TestModel>
  {
    public TestModel Create()
    {
      return new TestModel( ConfigurationManager.ConnectionStrings["localNPGSQL"] );
    }
  }
}
