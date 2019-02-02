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
