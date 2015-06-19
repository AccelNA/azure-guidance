# Guidance For Building Cloud Apps on Azure
Solution Architecture for building large scale distributed Cloud apps On Azure.

## Releases 

##### Drop 4 - [Distribued Apps with Azure App Services, WebJobs, Notification Hub and Service Bus Topic] (https://github.com/AccelNA/azure-guidance/blob/master/servicesbus-appservices.md)

Azure App Services Web App and WebJobs to offload complex processing to async workers (using WebJobs) and synchronize and communicate various systems in a distributed environment. Azure Service Bus Topic is used as a message broker to synchronize between Azure App Services Web App and Azure WebJobs. Provides Publish/Subscribe implementation using Azure Service Bus Topic.

[Source Code] (https://github.com/AccelNA/azure-guidance/tree/master/WebSite)

##### Drop 3 - [A Message Hub for IoT Scenarios with Azure Event Hub] (https://github.com/AccelNA/azure-guidance/blob/master/IoTEventHub.md)

A Message Hub for IoT Scenarios, for handling millions of messages and events in near real-time

[Source Code] (https://github.com/AccelNA/azure-guidance/tree/master/EventHub)
 
##### Drop 2 - [Distribued Apps with Azure Web Sites, WebJobs and Service Bus Topic] (https://github.com/AccelNA/azure-guidance/blob/master/servicesbus-webapp.md)

* This is a cost effective solution of Drop 1 with WebJobs and limited scalability power.
* This can be scale-up like Drop 1 if you could deploy WebJobs into individual Azure Web Apps.

Azure Web App (formerly WebSites) and WebJobs to offload complex processing to async workers (using WebJobs) and synchronize and communicate various systems in a distributed environment. Azure Service Bus Topic is used as a message broker to synchronize between Azure WebSites and Azure WebJobs. Provides Publish/Subscribe implementation using Azure Service Bus Topic

[Source Code] (https://github.com/AccelNA/azure-guidance/tree/master/WebSite)

##### Drop 1 - [Distribued Apps with Azure Cloud Services and Service Bus Topic] (https://github.com/AccelNA/azure-guidance/blob/master/servicebus-cloudservices.md)

Web and Worker roles to offload complex processing to async workers (using Worker Roles) and synchronize and communicate various systems in a distributed environment. Azure Service Bus Topic is used as a message broker to synchronize between Web Roles and Worker Roles.Provides Publish/Subscribe implementation using Azure Service Bu Topic

[Source Code] (https://github.com/AccelNA/azure-guidance/tree/master/CloudServices)

