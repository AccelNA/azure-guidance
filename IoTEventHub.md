
## Drop 3 - IoT Message Hub For Handling Millions Of Events, And Processing It Real-Time

### Technologies:

* Azure Event Hub
* Azure App Services Web App
* SignalR

### Solution Architecture 

![alt tag](https://github.com/AccelNA/azure-guidance/blob/master/contents/eventhub.JPG)

### Problem Domain
1. Build a message hub for IoT scenarios for sending millions of messages and events to the cloud at massive scale, with low latency and high reliability.
2. Process the massive amounts of messages and events in near real-time.

### Solution Workflow

1.	When automotive vehicles are moving towards roads, notify the GPS Coordinates to Android app for sending these events into an IoT Message Hub powered by Azure Event Hub. (For the sake of the demo, we use an Android app instead of automotive vehicles) 
2.	The Android app which receives the events from automotive vehicles, send the messages and events into IoT Message Hub.
3.	A real-time web app running on Azure App Services Web App receives the events from IoT Message Hub and provide real-time insight on the data received.


### Azure Services

1. Azure Event Hub – Working as the IoT Message Hub.
2. Azure App Services Web App – Running the real-time web app with SignalR.

