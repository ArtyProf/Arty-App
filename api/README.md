# Telegram bot with Open AI integration

Hosted on AWS Lambda. CI/CD is set up for AWS Lambda.

Functionality is in development.

Integrated with Open AI.

## Status checks
[![Deployment on AWS Lambda](https://github.com/ArtyProf/Arty-App/actions/workflows/backend_deployment.yml/badge.svg?branch=master)](https://github.com/ArtyProf/Arty-App/actions/workflows/backend_deployment.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=arty-app-api&metric=alert_status)](https://sonarcloud.io/summary/overall?id=arty-app-api)

## Usage:

/start - Greeting

/currency - Currency Exchange rate. Example: 
> /currency UAH USD 10

/question - Ask any question. Based on Open AI (ChatGPT). Example: 
> /question Top movie titles 2023

/image - Image description. Based on Open AI (ChatGPT). Example: 
> /image orange sky
