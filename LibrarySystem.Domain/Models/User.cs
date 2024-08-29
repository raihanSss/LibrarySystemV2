using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibrarySystem.Domain.Models;

public partial class User
{
    [Key]
    public int IdUser { get; set; }

    public string Fname { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Librarycard { get; set; }

    public DateOnly Cardexp { get; set; }

    public int? Notreturnbook { get; set; }

    public string? Penalty { get; set; }

    public string? Position { get; set; }


    [JsonIgnore]
    public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
}

