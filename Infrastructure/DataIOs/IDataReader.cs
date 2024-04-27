namespace Infrastructure.DataIOs;

public interface IDataReader<out T>
{
    IEnumerable<T> ReadData();
}