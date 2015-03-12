# azure-guidance
Solution Architecture for building large scale distributed Cloud apps on Azure.

## Drop 1- Distribued Apps with Azure Cloud Services and Service Bus Topic

### Technologies:

* Azure Cloud Services
  * Web Role
  * Worker Role
* Azure Service Bus Topic
* Azure Notification Hub
* Azure DocumentDB
* Azure Web Sites

### Solution Architecture 

![alt tag](https://github.com/AccelNA/azure-guidance/blob/master/contents/azure-arch.JPG)

### Problem Domain
1. Order processing app in a distributed computing environment.
2. Application logic should be deployed into multiple systems so that individual systems can separately scale-up and scale-down in Cloud.
3. An order request from customer, should notify to multiple systems – including mobile apps.
4. Cross platform mobile apps should be able to receive real-time push notifications from Cloud powered systems. 
5. A RESTful API for working as backend for mobile apps.


### Solution Workflow

1. End user submits an order through a web app which hosted in Azure web role.
2. The Azure web role publishes a message into Azure Service Bus Topic for the subscribers of the system in a distributed computing environment.
3. A worker processor  running in a Azure worker role subscribes the message from Azure Service Bus Topic , then process the order and finally persist the data into a NoSQL Database – Azure DocumentDB.
4. A notification hub hosted in Azure worker role subscribes the message from Azure Service Bus Topic, then send cross platform, real-time push notifications to mobile apps running in Android, iOS and Windows Phone.
5. The web app hosted in Azure web role shows the status of orders which is processed in worker processor.
6. Mobile apps are consuming REST API hosted in Azure web site.

