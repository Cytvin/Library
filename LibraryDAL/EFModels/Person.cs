using System;
using System.Collections.Generic;

namespace LibraryDAL.EFModels;

public partial class Person
{
    public int Id { get; set; }

    public string Fio { get; set; } = null!;

    public virtual ICollection<BookReservation> BookReservations { get; set; } = new List<BookReservation>();
}
