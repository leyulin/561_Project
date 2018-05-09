using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeCollections.Models
{
    public class Anime
    {

        public int ID { get; set; }

        public string OwnerID { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        [Range(1, 200)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }


        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(20)]
        [Required]
        public string Rating { get; set; }



        public Status Status { get; set; }
    }

    public enum Status
    {
        Submitted,
        Approved,
        Rejected
    }

}

