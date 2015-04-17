# Guidance For Building Cloud Apps on Azure
Solution Architecture for building large scale distributed Cloud apps On Azure.

## Releases 

##### Drop 1 - Distribued Apps with Azure Cloud Services and Service Bus Topic

Web and Worker roles to offload complex processing to async workers (using Worker Roles) and synchronize and communicate various systems in a distributed environment. Azure Service Bus Topic is used as a message broker to synchronize between Web Roles and Worker Roles. 

[Source Code] (https://github.com/AccelNA/azure-guidance/tree/master/CloudServices)
 
##### Drop 2 - Distribued Apps with Azure Web Sites, WebJobs and Service Bus Topic

WebJobs to offload complex processing to async workers (using WebJobs) and synchronize and communicate various systems in a distributed environment. Azure Service Bus Topic is used as a message broker to synchronize between Azure WebSites(Currently Azure App Services) and Azure WebJobs.

[Source Code] (https://github.com/AccelNA/azure-guidance/tree/master/WebSite)

