FROM node:16 as build-step
WORKDIR /app
COPY . .
RUN npm install --only=production
CMD ["npm", "run", "build"]

FROM nginx:1.18-alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build-step /app/build /frontend/build