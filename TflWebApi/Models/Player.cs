using System.ComponentModel.DataAnnotations;

namespace TflWebApi.Models
{
   public class Player
   {
      [Required]
      public string Name { get; set; }
   }
}