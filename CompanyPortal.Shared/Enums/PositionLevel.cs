using System.ComponentModel.DataAnnotations;

namespace CompanyPortal.Shared.Enums;

public enum PositionLevel
{
    [Display(Name = "Kmenový zaměstnanec")]
    Member = 1,

    [Display(Name = "Vedoucí skupiny")]
    GroupLeader = 2,

    [Display(Name = "Ředitel")]
    Director = 3,

    [Display(Name = "Člen vedení")]
    Board = 4
}
