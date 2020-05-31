using FluentValidation;
using TaxiBookingAPI.Model;

namespace TaxiBookingAPI.Validators
{
    /// <summary>
    /// Location Validator
    /// </summary>
    public class LocationValidator : AbstractValidator<Location> 
    {
        /// <summary>
        /// Location Validator Constructor
        /// </summary>
        public LocationValidator() {
            RuleFor(x => x.Source).NotNull().WithMessage("Source location Can not be Empty");
            RuleFor(x => x.Destination).NotNull().WithMessage("Destination location Can not be Empty");
            RuleFor(x => x.Destination.X).NotNull().WithMessage("Destination location X Can not be Empty");
            RuleFor(x => x.Destination.Y).NotNull().WithMessage("Destination location Y Can not be Empty");
            RuleFor(x => x.Source.X).NotNull().WithMessage("Source location X Can not be Empty");
            RuleFor(x => x.Source.Y).NotNull().WithMessage("Source location Y Can not be Empty");
        }
    }
}