using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EF6_NPGSQL_Perf_Test.DBHelpers;

namespace EF6_NPGSQL_Perf_Test.Objects
{
  public class User : IModHistory
  {
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Index( "IX_UserName", 1, IsUnique = true )]
    public string UserName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool PasswordResetFlag { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<UserProfile> UserProfiles { get; set; }

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
