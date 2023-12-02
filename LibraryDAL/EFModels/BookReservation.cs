using System;
using System.Collections.Generic;

namespace LibraryDAL.EFModels;

public partial class BookReservation
{
    public int Id { get; set; }

    public int PeopleId { get; set; }

    public int BookId { get; set; }

    public DateOnly Date { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Person People { get; set; } = null!;
}
