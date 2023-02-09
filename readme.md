# Telemetry Resender
## Description
This code component is designed to act as a mediator between two Dapr endpoints. It retrieves data from a source Dapr endpoint, processes it (if required), and then resends it to a destination Dapr endpoint. This component can be useful in scenarios where data needs to be transferred from one microservice to another, or where data needs to be transformed before being sent to its final destination. The component uses the Dapr API to communicate with the source and destination endpoints, ensuring seamless and efficient data transfer between them.

## Components foder
Components folder gives an example of using this component getting data from a Azure Service Bus Topic and sending that data to RabbitMQ exchange

### a. [subscription-dest-secret.yaml](components/subscription-dest-secret.yaml)
This manifest describes the creation of a Kubernetes Secret named `pubsub-resender-destiny-secret` of type `Opaque`. The secret contains a key-value pair, where the key is `connectionString` and the value is base64 encoded.

Note: The base64 encoded value should be replaced with your actual connection string for the RabbitMQ instance.

### b. [subscription-dest.yaml](components/subscription-dest.yaml)
This manifest describes the deployment of a Dapr Component named `pubsub-resender-destiny`. The component is of type `pubsub.rabbitmq` and is versioned `v1`. The component references a Kubernetes Secret `pubsub-resender-destiny-secret` for the RabbitMQ connection string. The component also specifies the name of the RabbitMQ exchange as `your_exchange_name` and sets the `durable` flag to `true`. The component is scoped to `telemetryresender`.

Note: The `your_exchange_name` value should be replaced with the actual name of your RabbitMQ exchange.

### c. [subscription-origin-secret.yaml](components/subscription-origin-secret.yaml)
This manifest describes the creation of a Kubernetes Secret named `pubsub-resender-origin-secret` of type `Opaque`. The secret contains a key-value pair, where the key is `connectionString` and the value is base64 encoded.

Note: The base64 encoded value should be replaced with your actual connection string for the Azure Service Bus.

### d. [subscription-origin.yaml](components/subscription-origin.yaml)
This manifest describes the deployment of a Dapr component and a subscription to an Azure Service Bus topic. The component is named `pubsub-resender-origin` and is of type `pubsub.azure.servicebus`. The subscription is named `myevent-subscription-otigin` and is for the topic `topic_name`. The subscription routes all incoming events to the `/process` endpoint by default and is scoped to `telemetryresender`.

Note: The `namespace` field in both the `Subscription` and `Component` sections is optional and can be omitted or filled in with your desired namespace.







