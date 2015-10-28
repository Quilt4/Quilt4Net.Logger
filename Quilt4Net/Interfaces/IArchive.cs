namespace Tharga.Quilt4Net.Interfaces
{
    public interface IArchive
    {
        IArchiveInfo Info { get; }
        void Load();
    }
}