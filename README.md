# GCloud PubSub &leftrightarrow; ES Adapter

Simple .NET Core adapter to pull IoT measurements from Google Cloud's PubSub and send them to an ElasticSearch instance.

Provide a valid config (see below) and a GCloud credentials file and run the program 

```bash
$ dotnet run
```

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
    "IndexName": "some_name",
    "CredentialsFile": "gcloud-credentials.json",
    "ProjectId": "...",
    "SubscriptionId": "...",
    "DeviceName": "groupname-device"
}
```

## Dependencies

- Google.Cloud.PubSub.V1 for subscribing to events on gcloud's PubSub
- NEST as ElasticSearch client
- Newtonsoft.Json for, you know, JSON
- .NET Core logging extenstions

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

## ElasticSearch Index Mapping>

The following [index mapping](https://www.elastic.co/guide/en/elasticsearch/reference/7.5/indices-put-mapping.html) can be used
for the ElasticSearch index:

```json
{
  "mappings": {
    "measurement": {
      "properties": {
        "deviceId": {
          "type": "text",
          "fields": {
            "keyword": {
              "type": "keyword",
              "ignore_above": 256
            }
          }
        },
        "humidity": {
          "type": "float"
        },
        "id": {
          "type": "text",
          "fields": {
            "keyword": {
              "type": "keyword",
              "ignore_above": 256
            }
          }
        },
        "temperatureCelsius": {
          "type": "float"
        },
        "timeStamp": {
          "type": "date"
        }
      }
    }
  }
}
``` 
