namespace Axpo.PowerTrading.Application.Service.Interface
{
	public interface IProcessingService
	{
		Task Process(CancellationToken stoppingToken);
	}
}
