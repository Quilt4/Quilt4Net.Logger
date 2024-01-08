namespace Quilt4Net.Ioc;

//TODO: Possibly make this one internal, if we can crate a prism logger.
public interface IIocProxy
{
    T GetService<T>();
}