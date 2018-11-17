using FluentValidation;

namespace CQRSLiteDemo.Web.Commands.Requests.Seasons
{
   public class CreateSeasonRequest
   {
      public string Year { get; set; }
   }

   public class CreateSeasonRequestValidator : AbstractValidator<CreateSeasonRequest>
   {
      public CreateSeasonRequestValidator()
      { 
         RuleFor( x => x.Year ).NotNull().NotEmpty().WithMessage( "The Year cannot be blank." );
      }
   }
}