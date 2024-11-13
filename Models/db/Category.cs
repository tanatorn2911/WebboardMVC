using System;
using System.Collections.Generic;

namespace WebboardMVC.Models.db
{
    public partial class Category
    {
        public Category()
        {
            Kratoos = new HashSet<Kratoo>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Kratoo> Kratoos { get; set; }
    }
}
