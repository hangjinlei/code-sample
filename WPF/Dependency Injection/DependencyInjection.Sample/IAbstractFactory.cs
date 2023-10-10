namespace DependencyInjection.Sample
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}