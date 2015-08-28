using System;
using System.ComponentModel.DataAnnotations;
using EF6_NPGSQL_Perf_Test.DBHelpers;

namespace EF6_NPGSQL_Perf_Test.Objects
{
  public class Window : IModHistory
  {
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public Canvas Canvas { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public WindowSettings WindowSettings { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public OverlaySettings OverlaySettings { get; set; }
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
