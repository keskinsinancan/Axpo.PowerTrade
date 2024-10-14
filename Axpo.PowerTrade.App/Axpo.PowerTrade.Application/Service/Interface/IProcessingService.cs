using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axpo.PowerTrading.Application.Service.Interface
{
	public interface IProcessingService
	{
		Task Process(CancellationToken stoppingToken);
	}
}
