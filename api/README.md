# Telegram bot with Open AI integration

Hosted on Azure Functions. CI/CD is set up for Azure.

Functionality is in development.

Integrated with Open AI.

## Status checks for back-end
[![Build and Deploy on AWS Lambda](https://github.com/ArtyProf/Arty-App/actions/workflows/backend_deployment.yml/badge.svg?branch=master)](https://github.com/ArtyProf/Arty-App/actions/workflows/backend_deployment.yml)
[![Quality Gate Status for Back-end](https://sonarcloud.io/api/project_badges/measure?project=ArtyProf_Arty-App&metric=alert_status)](https://sonarcloud.io/summary/overall?id=ArtyProf_Arty-App)

## Usage:

/start - Greeting

/currency - Currency Exchange rate. Example: 
> /currency UAH USD 10

/question - Ask any question. Based on Open AI (ChatGPT). Example: 
> /question Top movie titles 2023

/image - Image description. Based on Open AI (ChatGPT). Example: 
> /image orange sky
