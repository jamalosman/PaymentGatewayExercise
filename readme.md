# Payment Gateway Exercise

## Assumptions
- The aquiring bank will also need way to identify and authenticate the merchant
- The bank service will know where to put the money and only needs a merchant ID from the payment gateway

## Design

### Bank Service Integration
 - The merchants API (PaymentGatewayExercise.Api) will be the entry point
 - It will communicate with the aquiring bank service (PaymentGatewayExercise.BankingServices) via a message broker,
  to ensure tolerance, avoid timeouts, and keep the servies loosely coupled
 - The API will fire an event when a payment request is submitted, including a generated ID, 
 - The API will immediately return a response indicating the payment request has been 'Submitted', and redirect to the payment record
 - The bank service will subscribe to these events, attempt to porcess the payment and fire an event when it is completed indicating the success
 - The API will subscribe to these events, and update the payemnt record with the status and bank-generated ID
 - Bank payments will be queryable by either the API-generated ID, or the bank-generated ID, to support querying the status of pending payments

### Data Storage
- Data will be stored via EF Core in a SQL Server database
- Payments, including card details will be stored in a single table, as send in the request.
- Merchant information will be stored in a separate table, and Payments will have a reference.

## Projects
- PaymentGateway.Api (entry point, REST API)
- PaymentGateway.PersistPaymentsWorker (Consumer service for message broker)
- PaymentGateway.Services (Business logic behind merchant API)
- PaymentGateway.Data (database related code and repositories)
- PaymentGateway.Domain (Domain models, commands, events, etc. for merchant API)
- BankServices.PaymentServices (Aquiring bank business logic (simulated))
- BankServices.SubmitPaymentsWorker (Consumer service for message broker)
- BankServices.Domain (Domain models, commands, events, etc. for bank API)
