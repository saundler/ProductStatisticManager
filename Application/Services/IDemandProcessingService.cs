namespace Application.Services;

public interface IDemandProcessingService
{
    Task ProcessDemands(CancellationToken cancellationToken);
    Task MaxDegreeOfParallelismChanged(int newMaxDegreeOfParallelism);
}