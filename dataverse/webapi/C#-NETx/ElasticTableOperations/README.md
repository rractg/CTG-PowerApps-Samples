﻿---
languages:
- csharp
products:
- power-platform
- power-apps
page_type: sample
description: "This sample demonstrates how to perform common data operations on elastic tables using the Dataverse Web API."
---
# Web API Elastic Table Operations sample

This .NET 6.0 sample demonstrates how to perform common data operations on elastic tables using the Dataverse Web API.

This sample uses the common helper code in the [WebAPIService](../WebAPIService) class library project.

## Prerequisites

- Microsoft Visual Studio 2022
- Access to Dataverse with System Administrator privileges to create tables and perform data operations.

## How to run the sample

1. Clone or download the [PowerApps-Samples](https://github.com/microsoft/PowerApps-Samples) repository.
1. Open the [ElasticTableOperations.sln](ElasticTableOperations.sln) file using Visual Studio 2022
1. Edit the [appsettings.json](../appsettings.json) file to set the following property values:

   |Property|Instructions  |
   |---------|---------|
   |`Url`|The Url for your environment. Replace the placeholder `https://yourorg.api.crm.dynamics.com` value with the value for your environment. See [View developer resources](https://docs.microsoft.com/en-us/power-apps/developer/data-platform/view-download-developer-resources) to find this. |
   |`UserPrincipalName`|Replace the placeholder `you@yourorg.onmicrosoft.com` value with the UPN value you use to access the environment.|
   |`Password`|Replace the placeholder `yourPassword` value with the password you use.|

1. Save the `appsettings.json` file
1. Press F5 to run the sample.

## Demonstrates

This sample has 8 regions:

- [Create Elastic table](#create-elastic-table)
- [Create Record](#create-record)
- [Update Record](#update-record)
- [Upsert Record](#upsert-record)
- [Delete Record](#delete-record)
- [Demonstrate ExecuteCosmosSqlQuery](#demonstrate-executecosmossqlquery)
- [Delete Table](#delete-table)


### Create Elastic table

The code in this region sends this request to create a user-owned elastic table named `contoso_SensorData` with the following columns:


|Schema Name  |Type  |Description  |
|---------|---------|---------|
|`contoso_SensorType`|String|The primary name column for the `contoso_SensorData` table.|
|`contoso_DeviceId`|String|The ID of the device. Also used as the partitionid value.|
|`contoso_Value`|Integer|The value of the sensor data|
|`contoso_TimeStamp`|DateTime|The time of the reading.|
|`contoso_EnergyConsumption`|String|A string column using JSON format to demonstrate setting and querying JSON data using `ExecuteCosmosSqlQuery` function.|

The `TableType` property set to 'Elastic' makes this an elastic table.


**Request**

```http
POST [Organization Uri]/api/data/v9.2/EntityDefinitions
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
Content-Type: application/json; charset=utf-8
Content-Length: 6713

{
  "@odata.type": "Microsoft.Dynamics.CRM.EntityMetadata",
  "CanCreateCharts": {
    "@odata.type": "Microsoft.Dynamics.CRM.BooleanManagedProperty",
    "Value": false,
    "CanBeChanged": false
  },
  "Description": {
    "@odata.type": "Microsoft.Dynamics.CRM.Label",
    "LocalizedLabels": [
      {
        "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
        "Label": "Stores IoT data emitted from devices",
        "LanguageCode": 1033,
        "IsManaged": false
      }
    ],
    "UserLocalizedLabel": {
      "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
      "Label": "Stores IoT data emitted from devices",
      "LanguageCode": 1033,
      "IsManaged": false
    }
  },
  "DisplayCollectionName": {
    "@odata.type": "Microsoft.Dynamics.CRM.Label",
    "LocalizedLabels": [
      {
        "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
        "Label": "Sensor Data",
        "LanguageCode": 1033,
        "IsManaged": false
      }
    ],
    "UserLocalizedLabel": {
      "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
      "Label": "Sensor Data",
      "LanguageCode": 1033,
      "IsManaged": false
    }
  },
  "DisplayName": {
    "@odata.type": "Microsoft.Dynamics.CRM.Label",
    "LocalizedLabels": [
      {
        "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
        "Label": "Sensor Data",
        "LanguageCode": 1033,
        "IsManaged": false
      }
    ],
    "UserLocalizedLabel": {
      "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
      "Label": "Sensor Data",
      "LanguageCode": 1033,
      "IsManaged": false
    }
  },
  "HasActivities": false,
  "HasNotes": false,
  "IsActivity": false,
  "OwnershipType": "UserOwned",
  "SchemaName": "contoso_SensorData",
  "TableType": "Elastic",
  "Attributes": [
    {
      "@odata.type": "Microsoft.Dynamics.CRM.StringAttributeMetadata",
      "AttributeType": "String",
      "AttributeTypeName": {
        "Value": "StringType"
      },
      "MaxLength": 100,
      "DisplayName": {
        "@odata.type": "Microsoft.Dynamics.CRM.Label",
        "LocalizedLabels": [
          {
            "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
            "Label": "Sensor Type",
            "LanguageCode": 1033,
            "IsManaged": false
          }
        ],
        "UserLocalizedLabel": {
          "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
          "Label": "Sensor Type",
          "LanguageCode": 1033,
          "IsManaged": false
        }
      },
      "IsPrimaryName": true,
      "SchemaName": "contoso_SensorType"
    },
    {
      "@odata.type": "Microsoft.Dynamics.CRM.StringAttributeMetadata",
      "AttributeType": "String",
      "AttributeTypeName": {
        "Value": "StringType"
      },
      "MaxLength": 1000,
      "DisplayName": {
        "@odata.type": "Microsoft.Dynamics.CRM.Label",
        "LocalizedLabels": [
          {
            "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
            "Label": "Device Id",
            "LanguageCode": 1033,
            "IsManaged": false
          }
        ],
        "UserLocalizedLabel": {
          "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
          "Label": "Device Id",
          "LanguageCode": 1033,
          "IsManaged": false
        }
      },
      "SchemaName": "contoso_DeviceId"
    },
    {
      "@odata.type": "Microsoft.Dynamics.CRM.IntegerAttributeMetadata",
      "AttributeType": "Integer",
      "AttributeTypeName": {
        "Value": "IntegerType"
      },
      "MaxValue": 2147483647,
      "MinValue": -2147483648,
      "Format": "None",
      "SourceTypeMask": 0,
      "DisplayName": {
        "@odata.type": "Microsoft.Dynamics.CRM.Label",
        "LocalizedLabels": [
          {
            "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
            "Label": "Value",
            "LanguageCode": 1033,
            "IsManaged": false
          }
        ],
        "UserLocalizedLabel": {
          "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
          "Label": "Value",
          "LanguageCode": 1033,
          "IsManaged": false
        }
      },
      "SchemaName": "contoso_Value"
    },
    {
      "@odata.type": "Microsoft.Dynamics.CRM.DateTimeAttributeMetadata",
      "AttributeType": "DateTime",
      "AttributeTypeName": {
        "Value": "DateTimeType"
      },
      "Format": "DateOnly",
      "DisplayName": {
        "@odata.type": "Microsoft.Dynamics.CRM.Label",
        "LocalizedLabels": [
          {
            "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
            "Label": "Time Stamp",
            "LanguageCode": 1033,
            "IsManaged": false
          }
        ],
        "UserLocalizedLabel": {
          "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
          "Label": "Time Stamp",
          "LanguageCode": 1033,
          "IsManaged": false
        }
      },
      "SchemaName": "contoso_TimeStamp"
    },
    {
      "@odata.type": "Microsoft.Dynamics.CRM.StringAttributeMetadata",
      "AttributeType": "String",
      "AttributeTypeName": {
        "Value": "StringType"
      },
      "MaxLength": 1000,
      "FormatName": {
        "Value": "Json"
      },
      "Description": {
        "@odata.type": "Microsoft.Dynamics.CRM.Label",
        "LocalizedLabels": [
          {
            "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
            "Label": "Stores unstructured energy consumption data as reported by device",
            "LanguageCode": 1033,
            "IsManaged": false
          }
        ],
        "UserLocalizedLabel": {
          "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
          "Label": "Stores unstructured energy consumption data as reported by device",
          "LanguageCode": 1033,
          "IsManaged": false
        }
      },
      "DisplayName": {
        "@odata.type": "Microsoft.Dynamics.CRM.Label",
        "LocalizedLabels": [
          {
            "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
            "Label": "Energy Consumption",
            "LanguageCode": 1033,
            "IsManaged": false
          }
        ],
        "UserLocalizedLabel": {
          "@odata.type": "Microsoft.Dynamics.CRM.LocalizedLabel",
          "Label": "Energy Consumption",
          "LanguageCode": 1033,
          "IsManaged": false
        }
      },
      "SchemaName": "contoso_EnergyConsumption"
    }
  ]
}
```

**Response**

```http
HTTP/1.1 204 NoContent
OData-Version: 4.0
OData-EntityId: [Organization Uri]/api/data/v9.2/EntityDefinitions(47db17bf-2df8-ed11-8849-000d3a993550)
```



### Create Record

The code in this section creates an record.

**Request**

```http
POST [Organization Uri]/api/data/v9.2/contoso_sensordatas
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
Content-Type: application/json; charset=utf-8
Content-Length: 225

{
  "contoso_deviceid": "Device-ABC-1234",
  "contoso_sensortype": "Humidity",
  "contoso_value": 40,
  "contoso_timestamp": "2023-05-21T23:18:37.7184981Z",
  "partitionid": "Device-ABC-1234",
  "ttlinseconds": 86400
}
```

**Response**

```http
HTTP/1.1 204 NoContent
OData-Version: 4.0
OData-EntityId: [Organization Uri]/api/data/v9.2/contoso_sensordatas(da9c32cc-2df8-ed11-8849-000d3a993550)
x-ms-session-token: 207:8#142792103#7=-1
```

### Update Record

The code in this section updates the record create.

First, using the `partitionId` query parameter

**Request**

```http
PATCH [Organization Uri]/api/data/v9.2/contoso_sensordatas(da9c32cc-2df8-ed11-8849-000d3a993550)?partitionId=Device-ABC-1234
If-Match: *
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
Content-Type: application/json; charset=utf-8
Content-Length: 27

{
  "contoso_value": 60
}
```

**Response**

```http
HTTP/1.1 204 NoContent
OData-Version: 4.0
OData-EntityId: [Organization Uri]/api/data/v9.2/contoso_sensordatas(da9c32cc-2df8-ed11-8849-000d3a993550)
x-ms-session-token: 207:8#142792104#7=-1
```

Then, by using the alternate key to identify the record with the `partitionid` value.

**Request**

```http
PATCH [Organization Uri]/api/data/v9.2/contoso_sensordatas(contoso_sensordataid=da9c32cc-2df8-ed11-8849-000d3a993550,partitionid='Device-ABC-1234')
If-Match: *
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
Content-Type: application/json; charset=utf-8
Content-Length: 27

{
  "contoso_value": 80
}
```

**Response**

```http
HTTP/1.1 204 NoContent
OData-Version: 4.0
OData-EntityId: [Organization Uri]/api/data/v9.2/contoso_sensordatas(contoso_sensordataid=da9c32cc-2df8-ed11-8849-000d3a993550,partitionid='Device-ABC-1234')
x-ms-session-token: 207:8#142792105#7=-1
```

### Retrieve Record

The code in this section retrieves the record using two different ways to identify the record.

First, using the `partitionId` parameter:

**Request**

```http
GET [Organization Uri]/api/data/v9.2/contoso_sensordatas(da9c32cc-2df8-ed11-8849-000d3a993550)?partitionId=Device-ABC-1234&$select=contoso_value
MSCRM.SessionToken: 207:8#142792105#7=-1
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
```

**Response**

```http
HTTP/1.1 200 OK
ETag: W/"0401b4fd-0000-0200-0000-646aa6d10000"
OData-Version: 4.0

{
  "@odata.context": "[Organization Uri]/api/data/v9.2/$metadata#contoso_sensordatas(contoso_value)/$entity",
  "@odata.etag": "W/\"0401b4fd-0000-0200-0000-646aa6d10000\"",
  "contoso_value": 80,
  "contoso_sensordataid": "da9c32cc-2df8-ed11-8849-000d3a993550",
  "versionnumber": 638203079218106789,
  "_ownerid_value": "4026be43-6b69-e111-8f65-78e7d1620f5e",
  "_owningbusinessunit_value": "38e0dbe4-131b-e111-ba7e-78e7d1620f5e"
}
```

Then using alternate key:

**Request**

```http
GET [Organization Uri]/api/data/v9.2/contoso_sensordatas(contoso_sensordataid=da9c32cc-2df8-ed11-8849-000d3a993550,partitionid='Device-ABC-1234')?$select=contoso_value
MSCRM.SessionToken: 207:8#142792105#7=-1
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
```

**Response**

```http
HTTP/1.1 200 OK
ETag: W/"0401b4fd-0000-0200-0000-646aa6d10000"
OData-Version: 4.0

{
  "@odata.context": "[Organization Uri]/api/data/v9.2/$metadata#contoso_sensordatas(contoso_value)/$entity",
  "@odata.etag": "W/\"0401b4fd-0000-0200-0000-646aa6d10000\"",
  "contoso_value": 80,
  "contoso_sensordataid": "da9c32cc-2df8-ed11-8849-000d3a993550",
  "versionnumber": 638203079218106789,
  "_ownerid_value": "4026be43-6b69-e111-8f65-78e7d1620f5e",
  "_owningbusinessunit_value": "38e0dbe4-131b-e111-ba7e-78e7d1620f5e"
}
```

### Upsert Record

**Note** : When Upserting a record you must update the entire record. The contents will be overwritten and any data not included in the upsert payload will be lost.

The code in this section performs an upsert operation on the record that already exists, using two different ways to identify the record:

Using only the id in the URL. No PartitionId parameter. You don't need to include it because the `partitionid` value must be part of the payload.

**Request**

```http
PATCH [Organization Uri]/api/data/v9.2/contoso_sensordatas(da9c32cc-2df8-ed11-8849-000d3a993550)
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
Content-Type: application/json; charset=utf-8
Content-Length: 224

{
  "contoso_deviceid": "Device-ABC-1234",
  "contoso_sensortype": "Humidity",
  "contoso_value": 40,
  "contoso_timestamp": "2023-05-21T23:18:39.850711Z",
  "partitionid": "Device-ABC-1234",
  "ttlinseconds": 86400
}
```

**Response**

```http
HTTP/1.1 204 NoContent
OData-Version: 4.0
OData-EntityId: [Organization Uri]/api/data/v9.2/contoso_sensordatas(da9c32cc-2df8-ed11-8849-000d3a993550)
x-ms-session-token: 207:8#142792106#7=-1
```

Using the alternate key:

**Request**

```http
PATCH [Organization Uri]/api/data/v9.2/contoso_sensordatas(contoso_sensordataid=da9c32cc-2df8-ed11-8849-000d3a993550,partitionid='Device-ABC-1234')
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
Content-Type: application/json; charset=utf-8
Content-Length: 224

{
  "contoso_deviceid": "Device-ABC-1234",
  "contoso_sensortype": "Humidity",
  "contoso_value": 40,
  "contoso_timestamp": "2023-05-21T23:18:39.850711Z",
  "partitionid": "Device-ABC-1234",
  "ttlinseconds": 86400
}
```

**Response**

```http
HTTP/1.1 204 NoContent
OData-Version: 4.0
OData-EntityId: [Organization Uri]/api/data/v9.2/contoso_sensordatas(contoso_sensordataid=da9c32cc-2df8-ed11-8849-000d3a993550,partitionid='Device-ABC-1234')
x-ms-session-token: 207:8#142792107#7=-1
```


### Delete Record

The code in this section deletes the record using the `partitionId` parameter.

**Request**

```http
DELETE [Organization Uri]/api/data/v9.2/contoso_sensordatas(da9c32cc-2df8-ed11-8849-000d3a993550)?partitionId=Device-ABC-1234
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
```

**Response**

```http
HTTP/1.1 204 NoContent
OData-Version: 4.0
```

Since this can only be done once, the code to delete using alternate key can be uncommented and used instead.

### Demonstrate ExecuteCosmosSqlQuery

This section has three parts:

- [Create records to query](#create-records-to-query)
- [Execute the query to retrieve the first 50 records](#execute-the-query-to-retrieve-the-first-50-records)
- [Execute the query again to retrieve the next 50 records](#execute-the-query-again-to-retrieve-the-next-50-records)

#### Create records to query

This code performs a $batch operation to create 100 records that have `contoso_energyconsumption` set to a JSON value.

**Request**

```http
POST [Organization Uri]/api/data/v9.2/$batch
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
Content-Type: multipart/mixed; boundary="batch_f984ee1c-ea4c-46a9-9671-475e263de2dd"
Content-Length: 50783

--batch_f984ee1c-ea4c-46a9-9671-475e263de2dd
Content-Type: application/http
Content-Transfer-Encoding: binary
Content-Length: 388

POST /api/data/v9.2/contoso_sensordatas HTTP/1.1
Host: orgname.api.crm.dynamics.com
Content-Type: application/json; type=entry

{
  "contoso_deviceid": "Device-ABC-1234",
  "contoso_sensortype": "Humidity",
  "partitionid": "Device-ABC-1234",
  "contoso_energyconsumption": "{\"power\":0,\"powerUnit\":\"Watts\",\"voltage\":0,\"voltageUnit\":\"Volts\"}",
  "ttlinseconds": 86400
}

[99 records Truncated for brevity]

--batchresponse_6ae53466-55c3-4a06-b407-96b3e95d907a--

```

#### Execute the query to retrieve the first 50 records

The query parameters in this example have been URL decoded for readability. They should be URL encoded when sent.

**Request**

```http
GET [Organization Uri]/api/data/v9.2/ExecuteCosmosSqlQuery(QueryText=@p1,EntityLogicalName=@p2,QueryParameters=@p3,PageSize=@p4,PartitionId=@p5)?@p1='select c.props.contoso_deviceid as deviceId, c.props.contoso_timestamp as timestamp, c.props.contoso_energyconsumption.power as power from c where c.props.contoso_sensortype=@sensortype and c.props.contoso_energyconsumption.power > @power'
&@p2='contoso_sensordata'
&@p3={"Count":0,"IsReadOnly":false,"Keys":["@sensortype","@power"],"Values":[{"Type":"System.String","Value":"Humidity"},{"Type":"System.Int32","Value":"5"}]}
&@p4=50
&@p5='Device-ABC-1234'
MSCRM.SessionToken: 207:8#142792107#7=-1
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
```

**Response**

```http
HTTP/1.1 200 OK
OData-Version: 4.0

{
  "@odata.context": "[Organization Uri]/api/data/v9.2/$metadata#expando/$entity",
  "@odata.type": "#Microsoft.Dynamics.CRM.expando",
  "PagingCookie": "W3sidG9rZW4iOiIrUklEOn5DVm9OQUpJaWRuTjBJajRBQUFBd0R3PT0jUlQ6MSNUUkM6NTAjSVNWOjIjSUVPOjY1NTUxI1FDRjo4I0ZQQzpBWFFpUGdBQUFEQVBveUkrQUFBQU1BOD0iLCJyYW5nZSI6eyJtaW4iOiIxNDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMCIsIm1heCI6IjE0ODAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwIn19XQ==",
  "HasMore": true,
  "Result@odata.type": "#Collection(Microsoft.Dynamics.CRM.expando)",
  "Result": [
    {
      "@odata.type": "#Microsoft.Dynamics.CRM.expando",
      "deviceId": "Device-ABC-1234",
      "power": 6
    },
    [ 49 records truncated for brevity]
  ]
}
```

#### Execute the query again to retrieve the next 50 records

This time the `PagingCookie` parameter has the value returned from the previous response.


**Request**

```http
GET [Organization Uri]/api/data/v9.2/ExecuteCosmosSqlQuery(QueryText=@p1,EntityLogicalName=@p2,QueryParameters=@p3,PageSize=@p4,PagingCookie=@p5,PartitionId=@p6)?@p1='select c.props.contoso_deviceid as deviceId, c.props.contoso_timestamp as timestamp, c.props.contoso_energyconsumption.power as power from c where c.props.contoso_sensortype=@sensortype and c.props.contoso_energyconsumption.power > @power'
&@p2='contoso_sensordata'
&@p3={"Count":0,"IsReadOnly":false,"Keys":["@sensortype","@power"],"Values":[{"Type":"System.String","Value":"Humidity"},{"Type":"System.Int32","Value":"5"}]}
&@p4=50
@p5='W3sidG9rZW4iOiIrUklEOn5DVm9OQUpJaWRuTjBJajRBQUFBd0R3PT0jUlQ6MSNUUkM6NTAjSVNWOjIjSUVPOjY1NTUxI1FDRjo4I0ZQQzpBWFFpUGdBQUFEQVBveUkrQUFBQU1BOD0iLCJyYW5nZSI6eyJtaW4iOiIxNDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMCIsIm1heCI6IjE0ODAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwIn19XQ=='
&@p6='Device-ABC-1234'
MSCRM.SessionToken: 207:8#142792107#7=-1
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
```

**Response**

```http
HTTP/1.1 200 OK
OData-Version: 4.0

{
  "@odata.context": "[Organization Uri]/api/data/v9.2/$metadata#expando/$entity",
  "@odata.type": "#Microsoft.Dynamics.CRM.expando",
  "PagingCookie": "",
  "HasMore": false,
  "Result@odata.type": "#Collection(Microsoft.Dynamics.CRM.expando)",
  "Result": [
    {
      "@odata.type": "#Microsoft.Dynamics.CRM.expando",
      "deviceId": "Device-ABC-1234",
      "power": 106
    },
    [49 records truncated for brevity]
  ]
}
```

### Delete Table

The code in this section sends this request to delete the `contoso_SensorData` table using the `MetadataId` value that was returned when the table was created.

**Request**

```http
DELETE [Organization Uri]/api/data/v9.2/EntityDefinitions(47db17bf-2df8-ed11-8849-000d3a993550)
OData-MaxVersion: 4.0
OData-Version: 4.0
If-None-Match: null
Accept: application/json
```

**Response**

```http
HTTP/1.1 204 NoContent
OData-Version: 4.0
```



## Clean up

By default this sample will delete the `contoso_SensorData` table created at the beginning. It should leave no data in the system to clean up.