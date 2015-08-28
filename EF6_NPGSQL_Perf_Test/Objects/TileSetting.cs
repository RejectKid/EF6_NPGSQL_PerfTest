using System;
using EF6_NPGSQL_Perf_Test.DBHelpers;

namespace EF6_NPGSQL_Perf_Test.Objects
{
  public class TileSetting : IModHistory
  {
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Row { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Column { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public double Opacity { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsLocked { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int RowSpan { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int ColSpan { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Size { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string AssociatedObjectName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int ScreenIndex { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime DateModified { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime DateCreated { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsDirty { get; set; }
  }
}
