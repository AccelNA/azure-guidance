using Microsoft.AspNet.SignalR;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureGuidanceEventHub
{
    public class SimpleEventProcessor :  IEventProcessor
    {
        PartitionContext partitionContext;
        Stopwatch checkpointStopWatch;
        

        public SimpleEventProcessor()
        {
           
            
        }

        public Task OpenAsync(PartitionContext context)
        {
            this.partitionContext = context;
            this.checkpointStopWatch = new Stopwatch();
            this.checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> events)
        {
                foreach (EventData eventData in events)
                {
                    var newData = this.DeserializeEventData(eventData);
                    string key = eventData.PartitionKey;
                    IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<AzureGuidanceEventHubReceiver>();                    
                    hubContext.Clients.All.DisplayMyMessage(newData);
                }

                //Call checkpoint every 5 minutes, so that worker can resume processing from the 5 minutes back if it restarts.
                if (this.checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
                {
                    await context.CheckpointAsync();
                    this.checkpointStopWatch.Restart();
                }
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        Object DeserializeEventData(EventData eventData)
        {
            return JsonConvert.DeserializeObject<Object>(Encoding.UTF8.GetString(eventData.GetBytes()));
        }
    }
}
