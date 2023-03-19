FROM node:18-alpine
WORKDIR /app

COPY artyapp/package*.json ./

RUN npm install

COPY artyapp/. .

RUN npm run build

EXPOSE 3000
CMD ["npm", "start"]