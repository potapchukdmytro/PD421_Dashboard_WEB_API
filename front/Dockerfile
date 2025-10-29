FROM node:22.21-alpine AS build

WORKDIR /app

COPY package*.json .

RUN npm ci

RUN npm i -g serve

COPY . .

RUN npm run build

EXPOSE 5173

CMD [ "serve", "-s", "dist", "-p", "5173" ]