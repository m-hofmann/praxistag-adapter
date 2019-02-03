# GCloud PubSub &leftrightarrow; ES Adapter

Simple .NET Core adapter to pull IoT measurements from Google Cloud's PubSub and send them to an ElasticSearch instance.

## Configuration

The program reads configuration from the file `config.json` which it expects
in its working directory. It should look like this:

```json
{
    "ElasticUser": "...",
    "ElasticPassword": "...",
    "ElasticHost": "example.org",
    "ElasticPort": 80,
    "ElasticContextRoute": "some/route/or/empty",
    "CredentialsFile": "gcloud-credentials.json",
    "ProjectId": "...",
    "SubscriptionId": "..."
}
```

## Dependencies

- Google.Cloud.PubSub.V1 for subscribing to events on gcloud's PubSub
- NEST as ElasticSearch client
- Newtonsoft.Json for, you know, JSON

## PubSub Message Object

Each temperature measurement received from PubSub should look like this: 

```json
{
    "device_id": "sensor-42",
    "time_stamp": 1549123452,
    "temperature": -5.4,
    "humidity": 20.3
}
```

Before sending it to ElasticSearch, an `_id` field with a randomly generated
GUID is added to serve as document key.
