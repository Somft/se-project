using System;
using System.Collections.Generic;

#nullable disable

namespace ExBook.Data
{
    public partial class Subject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
