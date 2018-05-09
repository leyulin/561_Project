using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeCollections.Models
{
    public class GenreRating
    {

        public int GenreRatingID { get; set; }
        public int GenreID { get; set; }

        [Range(1, 200)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }


        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(20)]
        [Required]
        public string Rating { get; set; }

    }
}
