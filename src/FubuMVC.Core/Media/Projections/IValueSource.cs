namespace FubuMVC.Core.Media.Projections
{
    public interface IValueSource<T>
    {
        IValues<T> FindValues();
    }
}