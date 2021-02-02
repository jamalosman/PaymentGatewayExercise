# Payment Gateway Exercise

## Assumptions
- The service will need to identify the merchant
	- This is implemented as a simple API key authentication with the header X-API-KEY
- The service that speaks to the aquiring bank will be able to identify the merchant based of their ID
- The bank service will know where to put the money and only needs a merchant ID from the payment gateway
- We do not need to store full card numbers, only masked ones

## How to run
There are three projects that need to run
- PaymentGateway.Api
- BankServices.SubmitPaymentsWorker
- PaymentGateway.PersistPaymentsWorker

each has an appsettings.json file with some configs that need to be set
- `PaymentsMessageBroker.Uri` should point to a rabbitmq instance, it should have a virtual host and a user who can access it
- `ConnectionStrings.PaymentsDatabase` should point to a SQL Server instance with a database created


## Design

### Bank Service Integration
 - PaymentGateway.Api will be the entry point where the merchant will request to make/view payments
 - It will communicate with the aquiring bank service (PaymentGatewayExercise.BankingServices.SubmitPaymentsWorkers) via a message broker,
  to ensure tolerance, avoid timeouts, and keep the servies loosely coupled
 - The API will fire an command when a payment request is submitted, including a generated ID, 
 - The API will immediately return a response indicating the payment request has been 'Submitted', and redirect to the payment record
 - The bank service will subscribe to these commands, attempt to porcess the payment and fire an event when it is completed indicating the success
 - A separate worker (PaymentGateWay.PersistPaymentsWorker) will subscribe to these events, and update the payemnt record with the status and bank-generated ID
 - Bank payments will be queryable by either the API-generated ID, or the bank-generated ID, to support querying the status of pending payments

### Data Storage
- Data will be stored via EF Core in a SQL Server database
- Payments, including card details will be stored in a single table, as send in the request.
- Merchant information will be stored in a separate table, and Payments will have a reference.
- Card numbers will be saved masked

### Message Broker
- I've used the message broker library Rebus, with RabbitMq as the underlying provider

### Logging
- I've used serilog for logging, Exception handling is in place so all unhandled errors are logged
- ILogger can be injected anywhere via the DI container
- Logs are written to console, file and Seq (a log viewer). There are many sinks available to send logs pretty much anaywhere

### Authentication
- I implemented a simple API key based authentication and there is no flow built for creating new merchants/keys
- To be able to use the app, I've added some code to MerchantsRepository constructor to create two users if none exist
- When sending reqeusts to the API, pass one of these two keys, or the admin key from appsettings.json, in order to pass authentication
```csharp
                _paymentsContext.AddRange(
                    new Merchant
                    {
                        ApiKey = "84e3868bd19b48d7b2c2c1a420afca96",
                        EncryptionKey = "abcd1234$%^&8765£$%^poiu",
                        CompanyName = "PaperShop",
                        ContactName = "Ian Papier",
                        EmailAddress = "ian.papier@papershop.com",
                    },
                    new Merchant
                    {
                        ApiKey = "51453ab10b194cea9004da4501f846fc",
                        EncryptionKey = "8765£$%^poiuabcd1234$%^&",
                        CompanyName = "Peanuts R Us",
                        ContactName = "Alex Nutter",
                        EmailAddress = "alex.nutter@peanutsrus.com",
                    });
                _paymentsContext.SaveChanges();
```

### Authorization
- Only merchants can create payments, and they can only view payments which they created
- If a merchant tries to access another merchants payment via ID, they get a 404 (they shouldn't know it exists)
- Admin users (the admin token) can view all payments but they cannot create any

### Metrics
 - I didn't get around to this, but looked into some tools like https://www.app-metrics.io/
 - some useful metrics could be
  - counters on success/failed/pending payments
  - response times in the api, 
  - time between reqeust submitted and completed (payment flow)
 - There are plenty of tools that can spot anomalies in these things and alert on them.

### Api CLient
 - Again, I didn't get around to this, but building a Client library as a nuget package 
 would make integration easier for merchants
 - I've published private nugets before.

### Containerization
- ...again unfortunately I didn't get around to this
- I was looking forward to getting the API and the two works to run from docker containers and talk to each other
- The app also depends on rabbitmq and SQLServer so it would be nice if there was a build script that would set that up as well and run everything
- for now the app requires Rabbitmq to be setup and the URL should be in the config in order for everything to work

### Other Possible improvements
- Move authentication/authorization into a separate, reusable project
- support more authenticatio in a cleaner way
- Move message classes sent via the message bus to a private nuget package
- Proper unit test coverage
- full documentation (e.g. via swagger and xmlcomments)

### Encryption
- Inside PaymentGateway.Services I've added an implementation of AES-GCM encryption, and each merchant 
has his own key in the database to support encrypting sensative data, however, I never used it.


