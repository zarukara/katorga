namespace ServiceSystem
{
    public interface IService
    {
        T GetService<T>();
    }
}