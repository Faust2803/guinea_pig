FROM alpine:3.20.0

WORKDIR /usr/share/nginx/html

RUN apk add brotli nginx nginx-mod-http-brotli

COPY WebGL/ .

RUN rm -rf /etc/nginx/conf.d/default.conf && \
    nginx -v

EXPOSE 8080

CMD ["nginx", "-g", "daemon off;"]

USER nginx