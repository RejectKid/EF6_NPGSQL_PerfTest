using System;

namespace EF6_NPGSQL_Perf_Test.DBHelpers
{
  interface IModHistory
  {
    DateTime DateModified { get; set; }
    DateTime DateCreated { get; set; }
    bool IsDirty { get; set; }
  }
}
