using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibrarySystem.Domain.Models;

public partial class Book
{
    [Key]
    public int IdBook { get; set; }

    public string Title { get; set; } 

    public string? Category { get; set; }

    public string? Isbn { get; set; }

    public string? Publisher { get; set; }

    public string? Author { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public DateOnly Purchasedate { get; set; }

    public decimal? Price { get; set; }

    public string? Status { get; set; }

    public string? Reason { get; set; }

    public int? Availablebook { get; set; }
    public string? Language { get; set; }

    [JsonIgnore]
    public virtual ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
}
