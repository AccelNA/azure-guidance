 Distribued Apps with Azure App Services, WebJobs, Service Bus Topic and Notification Hub

### Technologies:

* Azure App Services Web App
* Azure App Services API App
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
3. An order request from customer, should notify to multiple systems – including order processing system and mobile apps.
4. Cross platform mobile apps should be able to receive real-time push notifications from Cloud powered systems. 
5. A RESTful API for providing as a backend for mobile apps and web apps.
6. Distributed in-memory caching is required for the system.



### Solution Workflow

1. End user submits an order through a web app, which is hosted in Azure App Serviced Web App. 
2. The Azure Web App publishes a message into Azure Service Bus Topic for the subscribers of the distributed systems environment.
3. A WebJob running in a Azure App Services Web App (hosted for running WebJobs only) subscribes the message from Azure Service Bus Topic , then process the order and finally persist the data into a NoSQL Database – Azure DocumentDB.
4. A notification hub hosted running in a WebJob in a Azure App Services Web App (hosted for running WebJobs only) subscribes the message from Azure Service Bus Topic, then send cross platform, real-time push notifications to mobile apps running in Android, iOS and Windows Phone.
5. The web app hosted in Azure App Services shows the status of orders which is processed in the WebJob, which performs order processing logic.
6. Mobile apps are consuming REST API hosted in Azure App Serviced API App.


### Azure Services

1. Azure App Serviced Web App – Used for running web app for receiving orders from customers. 
2. Azure App Serviced API App – Used for running RESTful API.
3. Azure App Serviced Web App - Running WebJob for a background worker for receiving messages from Service Bus Topic and then processing the orders.
4. Azure App Serviced Web App - Running WebJob for a background worker for receiving messages from Service Bus Topic and then sends real-time push notifications to cross platform mobile apps.
5. Service Bus – Working as a message broker
6. Notification Hub – Sending cross platform push notifications.
7. DocumentDB – NoSQL data store. 
8. Redis Cache – Distributed caching.

