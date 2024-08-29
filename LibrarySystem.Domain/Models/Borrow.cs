using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibrarySystem.Domain.Models;

public partial class Borrow
{
    [Key]
    public int IdBorrow { get; set; }

    public int IdUser { get; set; }

    public int IdBook { get; set; }

    public DateOnly? DateBorrow { get; set; }

    public DateOnly? DateReturn { get; set; }

    public decimal? Penalty { get; set; }

    [JsonIgnore]
    public virtual Book IdBookNavigation { get; set; } = null!;
    [JsonIgnore]
    public virtual User IdUserNavigation { get; set; } = null!;
}
