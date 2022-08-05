using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Masstransit.Test.Components.Models.Exceptions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Masstransit.Test.Components.Contracts.Browse {
    public class BrowseCurrentData {
        public string OrganizationId { get; set; }
    }
    public class BrowseCurrentDataSuccess {
        public string Record { get; set; }
    }

    public class BrowseCurrentDataConsumer : IConsumer<BrowseCurrentData> {
        private readonly ILogger<BrowseCurrentDataConsumer> _logger;
        public BrowseCurrentDataConsumer (
            ILogger<BrowseCurrentDataConsumer> logger
        ) {
            _logger = logger;
        }
        public async Task Consume (ConsumeContext<BrowseCurrentData> context) {
            string queryName = typeof(BrowseCurrentData).Name;
            try {
                _logger.LogCritical("hello");
                await context.RespondAsync (new BrowseCurrentDataSuccess {
                        Record = "123"
                    });
            } catch (Exception ex) {
                // unknown error
                // string errorMessage = Utilties.LogMessage (queryName, context.Message.OrganizationId, null, ex.Message);
                _logger.LogError (ex.Message);
                await context.RespondAsync (new BadRequestViewModel {
                    Title = queryName,
                    Status = 400,
                    Message = ex.Message
                });
            }
        }
    }
}