using System.Collections.Generic;
using LearningKendoUIWeb.Models;

namespace LearningKendoUIWeb.Repository
{
   public class SampleRepository
   {
      public List<Movie> GetAllMovies()
      {
         var movies = new List<Movie>
         {
            new Movie
            {
               Rank = 1,
               Rating = 9.2,
               Title = "The Shawshank Redemption",
               Year = 1994
            },
            new Movie
            {
               Rank = 2,
               Rating = 9.1,
               Title = "The Godfather",
               Year = 1974
            }
         };
         return movies;
      }
   }
}