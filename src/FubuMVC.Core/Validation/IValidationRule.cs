namespace FubuMVC.Core.Validation
{
    public interface IValidationRule
    {
        void Validate(ValidationContext context);
    }
}