using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EF6_NPGSQL_Perf_Test.DBHelpers;

namespace EF6_NPGSQL_Perf_Test.Objects
{
  public class UserProfile : IModHistory
  {
    public UserProfile()
    {
      CanvasLayout = new List<CanvasLayout>();
      Project = new Project();
      Scenario = new Scenario();
      TileLayout = new List<TileLayout>();
    }
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public User User { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public List<CanvasLayout> CanvasLayout { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public Project Project { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public Scenario Scenario { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public List<TileLayout> TileLayout { get; set; }

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
