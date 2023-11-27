namespace DC_ARPG
{
    public interface IDependency<T>
    {
        void Construct(T obj);
    }
}
