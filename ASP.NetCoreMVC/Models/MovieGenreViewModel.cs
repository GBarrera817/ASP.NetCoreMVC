using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ASP.NetCoreMVC.Models
{
    public class MovieGenreViewModel
    {
        public List<Movie>? Movies { get; set; }    // Movies List
        public SelectList? Genres { get; set; }     // Genres list, This allow to user to select a genre of the list
        public string? MovieGenre {  get; set; }    // Selected Movie Genre
        public string? SearchString { get; set; }   // Text in the input search bar 
    }
}
