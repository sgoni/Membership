#!/bin/bash

# COnfigure our access
export VAULT_ADDR='http://vault:8200'
#export VAULT_TOKEN="p8ItnHqR5SXlAb8EzXaGSDqp"  # to be able to use the cli on dev

docker compose -f docker-compose.yml -f docker-compose.override.yml up -d --build

sleep 30 # sleep 30 seconds to give time to docker to finish the setup

# verify the connection 
echo docker exec -it vault vault status
docker exec -it vault vault status

## User&Pass for postgresql
echo docker exec -it vault vault secrets enable database
docker exec -it vault vault secrets enable database

## Configure PostgreSQL connection
echo Configure PostgreSQL connection.  
docker exec -it vault vault write database/config/postgresql \
  plugin_name=postgresql-database-plugin \
  allowed_roles="db-role" \
  connection_url="postgresql://{{username}}:{{password}}@memberdb:5432/MemberDb?sslmode=disable" \
  username="postgres" \
  password="postgres"
  
## Configure roles for dynamic credentials 
echo Configure roles for dynamic credentials. 
docker exec -it vault vault write database/roles/db-role \
  db_name=postgresql \
  creation_statements="
  CREATE ROLE \"{{name}}\" WITH LOGIN PASSWORD '{{password}}' VALID UNTIL '{{expiration}}'; 
  GRANT CREATE ON SCHEMA public TO \"{{name}}\";  
  GRANT SELECT, INSERT, DELETE, UPDATE ON ALL TABLES IN SCHEMA public TO \"{{name}}\";" \
  default_ttl="1h" \
  max_ttl="24h"  
  
echo docker exec -it vault vault read database/creds/db-role  
docker exec -it vault vault read database/creds/db-role      
  
#echo setup consul configuration
F:/Repositorio/Membership/tools/consul/config.sh

read -p "Press enter to continue"