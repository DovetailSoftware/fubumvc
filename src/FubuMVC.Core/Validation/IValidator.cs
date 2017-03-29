namespace FubuMVC.Core.Validation
{
    public interface IValidator
    {
        Notification Validate(object target);
        void Validate(object target, Notification notification);

        ValidationContext ContextFor(object target, Notification notification);
    }
}