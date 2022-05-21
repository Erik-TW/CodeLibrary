using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Data.Models
{
    public class Book
    {
        [Key]
        public string Id { get; set; }
		public string Author { get; set; }
		public string Title { get; set; }
		public string Genre { get; set; }
		public double Price { get; set; }
		public DateTime? Publish_Date { get; set; }
		public string Description { get; set; }
	}
}
