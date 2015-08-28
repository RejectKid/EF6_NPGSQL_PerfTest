using EF6_NPGSQL_Perf_Test.DBHelpers;
using EF6_NPGSQL_Perf_Test.DBHelpers.Impl;
using Ninject.Modules;

namespace EF6_NPGSQL_Perf_Test.BindHouse
{
  class Bindings : NinjectModule
  {
    public override void Load()
    {
      Bind<IDbConn>().To<DbConn>().InSingletonScope();
    }
  }
}
