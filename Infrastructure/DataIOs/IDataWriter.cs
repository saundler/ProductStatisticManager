namespace Infrastructure.DataIOs;

public interface IDataWriter<in T>
{
    void WriteData(T data);
}