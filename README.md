# Personal app for forging my skills

Back-end is hosted on AWS Lambda. CI/CD is set up for AWS Lambda.

Front-end was bootstrapped with [Create React App](https://github.com/facebook/create-react-app) using typescript template. Hosted on Github Pages.

Functionality is in development.

Integrated with Open AI.

## Status checks

Back-end:
[![Deployment on AWS Lambda](https://github.com/ArtyProf/Arty-App/actions/workflows/backend_deployment.yml/badge.svg?branch=master)](https://github.com/ArtyProf/Arty-App/actions/workflows/backend_deployment.yml)
[![Quality Gate Status for Back-end](https://sonarcloud.io/api/project_badges/measure?project=arty-app-api&metric=alert_status)](https://sonarcloud.io/summary/overall?id=arty-app-api)

Front-end:
[![Quality Gate Status for Front-end](https://sonarcloud.io/api/project_badges/measure?project=arty-app-webapp&metric=alert_status)](https://sonarcloud.io/summary/overall?id=arty-app-webapp)

## Usage:

[Back-end README.md](https://github.com/ArtyProf/Arty-App/blob/master/api/README.md)

[Front-end README.md](https://github.com/ArtyProf/Arty-App/blob/master/webapp/README.md)

## Run project in Docker

`docker-compose -f ./docker-compose.local.yml up --build`
