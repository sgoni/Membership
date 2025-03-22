#!/bin/bash

docker compose -f D:/Repositorio/EasyTrack/docker-compose.yml -f D:/Repositorio/EasyTrack/docker-compose.override.yml up -d

sleep 10 # sleep 30 seconds to give time to docker to finish the setup

#echo setup vault configuration
#./tools/vault/config.sh

echo setup consul configuration
./tools/consul/config.sh

echo completed