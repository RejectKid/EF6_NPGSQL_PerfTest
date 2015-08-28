using System;
using EF6_NPGSQL_Perf_Test.DBHelpers;

namespace EF6_NPGSQL_Perf_Test.Objects
{
  public class OverlaySettings : IModHistory
  {
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the top docked control's visibility.
    /// </summary>
    public bool IsNotificationOn { get; set; }

    /// <summary>
    /// Gets or sets the bottom docked control's visibility.
    /// </summary>
    public bool IsFeedbackOn { get; set; }

    /// <summary>
    /// Gets or sets the notification and feedback bar's 
    /// horizontal offset on the window. The default is 0.
    /// </summary>
    public double Offset { get; set; }

    /// <summary>
    /// Gets or sets the notification and feedback bar's 
    /// horizontal size on the window. The default, 0,
    /// will span the entire length of the window.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the inital DPI (scale) of the overlay.
    /// Where 1 = 100% scale and values less than one are smaller
    /// than normal scale and larger are bigger than normal scale.
    /// </summary>
    public double Dpi { get; set; }
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
