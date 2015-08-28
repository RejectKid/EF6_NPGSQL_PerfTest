using System;
using System.Windows;
using EF6_NPGSQL_Perf_Test.DBHelpers;

namespace EF6_NPGSQL_Perf_Test.Objects
{
  public class WindowSettings : IModHistory
  {
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int ScreenIndex { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public WindowState ScreenState { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public ResizeMode ResizeMode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Width { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Height { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Left { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Top { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int ScreenSpan { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsSandboxed { get; set; }
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
