# dfc-api-location

## Introduction

This project provides an API for loading ONS location data into an Azure Search Index
The Search Index can then be used to search for locations in England to return location details

## Getting Started

## List of dependencies

|Item	|Purpose|
|-------|-------|
|Azure Search | Index for location data |
|ONS Location API | Office for National Statistics |

## Local Config Files

Once you have cloned the public repo you need to rename the local.settings files by removing the -template part from the configuration file names listed below.

| Location | Repo Filename | Rename to |
|-------|-------|-------|
| DFC.Api.JobProfile | local.settings-template.json | local.settings.json |

## Configuring to run locally

The project contains a number of "local.settings-template.json" files which contain sample local.settings for the web app and the integration test projects. To use these files, rename them to "local.settings.json" and edit and replace the configuration item values with values suitable for your environment.

The settings include the parameters required to call Azure Search, which are:

| Section | Parameter | Value |
|-------|-------|-------|
| LocationSearchIndexConfig | SearchIndex | search-index |
| LocationSearchIndexConfig | SearchServiceName | search-service-name |
| LocationSearchIndexConfig | AccessKey | search-index-access-key |

## Running locally

To run this product locally, you will need to configure the list of dependencies. Once configured and the configuration files updated, it should be F5 to run and debug locally. The application can be run using IIS Express or full IIS.

To run the project, start the JobProfileFunction Azure function app. On your local environment, swagger documentation is available at http://localhost:7071/api/swagger/ui

The API function app contains with the following endpoints:
- /location-load - fetches location data from ONS and rebuild the Azure Search Index  

## Built With

* Microsoft Visual Studio 2019
* .Net Core 3.1

