using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeCollections.Models
{
    public class Genre
    {

        public int GenreID { get; set; }
        public int ID { get; set; }


        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Required]
        [StringLength(30)]
        public string Subject { get; set; }

        public ICollection<GenreRating> GenreRatings { get; set; }

    }
}
