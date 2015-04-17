 Distribued Apps with Azure Web Sites, WebJobs and Service Bus Topic

### Technologies:

* Azure Web Sites
* Azure WebJobs
* Azure Service Bus Topic
* Azure Notification Hub
* Azure DocumentDB
* Azure Redis Cache


### Solution Architecture 

![alt tag](https://github.com/AccelNA/azure-guidance/blob/master/contents/azure-arch-webjobs.JPG)

### Problem Domain
1. Order processing app in a distributed computing environment.
2. Application logic should be deployed into multiple components.
3. An order request from customer, should notify to multiple systems – including order processing system and a Notification Hub.
4. Cross platform mobile apps should be able to receive real-time push notifications from Cloud powered systems. 
5. A RESTful API for working as backend for mobile apps.
6. Cost effectiveness is important for this system.


### Solution Workflow

1. End user submits an order through a web app, which is hosted in Azure Web Site. The Web Site is also hosting two WebJobs: a WebJob for performing order processing logic and another WebJob for sending real-time mobile push notifications.
2. The Azure Web Site publishes a message into Azure Service Bus Topic for the subscribers of the distributed systems environment.
3. A WebJob running in a Azure Web Site subscribes the message from Azure Service Bus Topic , then process the order and finally persist the data into a NoSQL Database – Azure DocumentDB.
4. A notification hub hosted running in a WebJobs, subscribes the message from Azure Service Bus Topic, then send cross platform, real-time push notifications to mobile apps running in Android, iOS and Windows Phone.
5. The web app hosted in Azure Web Site shows the status of orders which is processed in the WebJob which performs order processing logic.
6. Mobile apps are consuming REST API hosted in Azure web site.

### Azure Services

1. Web Site– Used for running web app for receiving orders from customers. The Web Site is also hosting two WebJobs.
2. WebJob -  WebJob - Background worker for receiving messages from Service Bus Topic and then processing the orders.
3. WebJob - Background worker for receiving messages from Service Bus Topic and then sends real-time push notifications to 2. cross platform mobile apps.
3. Service Bus – Working as a message broker
4. Notification Hub – Sending cross platform push notifications.
5. DocumentDB – NoSQL data store. 
6. Web Site – Used for running RESTful API.
