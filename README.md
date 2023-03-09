# Personal app for forging my skills

Back-end is hosted on AWS Lambda.

Functionality is in development.

Integrated with Open AI.

### Configuring for API Gateway HTTP API ###

API Gateway supports the original REST API and the new HTTP API. In addition HTTP API supports 2 different
payload formats. When using the 2.0 format the base class of `LambdaEntryPoint` must be `Amazon.Lambda.AspNetCoreServer.APIGatewayHttpApiV2ProxyFunction`.
For the 1.0 payload format the base class is the same as REST API which is `Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction`.
**Note:** when using the `AWS::Serverless::Function` CloudFormation resource with an event type of `HttpApi` the default payload
format is 2.0 so the base class of `LambdaEntryPoint` must be `Amazon.Lambda.AspNetCoreServer.APIGatewayHttpApiV2ProxyFunction`.


### Configuring for Application Load Balancer ###

To configure this project to handle requests from an Application Load Balancer instead of API Gateway change
the base class of `LambdaEntryPoint` from `Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction` to 
`Amazon.Lambda.AspNetCoreServer.ApplicationLoadBalancerFunction`.

### Project Files ###

* serverless.template - an AWS CloudFormation Serverless Application Model template file for declaring your Serverless functions and other AWS resources
* aws-lambda-tools-defaults.json - default argument settings for use with Visual Studio and command line deployment tools for AWS
* LambdaEntryPoint.cs - class that derives from **Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction**. The code in 
this file bootstraps the ASP.NET Core hosting framework. The Lambda function is defined in the base class.
Change the base class to **Amazon.Lambda.AspNetCoreServer.ApplicationLoadBalancerFunction** when using an 
Application Load Balancer.
* LocalEntryPoint.cs - for local development this contains the executable Main function which bootstraps the ASP.NET Core hosting framework with Kestrel, as for typical ASP.NET Core applications.
* appsettings.json - used for local development.

## Status checks for back-end

[![Build and Deploy on Azure](https://github.com/ArtyProf/ArtyApp/actions/workflows/backend_deployment.yml/badge.svg?branch=master)](https://github.com/ArtyProf/Telegram-bot-from-Arty_Prof/actions/workflows/backend_deployment.yml)
[![Quality Gate Status for Back-end](https://sonarcloud.io/api/project_badges/measure?project=ArtyProf_Arty-App&metric=alert_status)](https://sonarcloud.io/summary/overall?id=ArtyProf_Arty-App)

## Usage:

[Back-end readme.md](https://github.com/ArtyProf/Arty-App/blob/feature/Change_file_structure/api/README.md)
