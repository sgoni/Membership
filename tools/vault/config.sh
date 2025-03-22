#!bin/bash
# COnfigure our access
#export VAULT_ADDR="http://127.0.0.1:8200"
#export VAULT_TOKEN: "s.p8ItnHqR5SXlAb8EzXaGSDqp"  # to be able to use the cli on dev
#export VAULT_DEV_ROOT_TOKEN_ID: "p8ItnHqR5SXlAb8EzXaGSDqp"
#export VAULT_DEV_LISTEN_ADDRESS: "0.0.0.0:8200"
#export VAULT_API_ADDR="http://0.0.0.0:8200

sleep 30

# verify the connection 
docker exec -it vault vault status

## 1. Habilitar el motor de secretos de RabbitMQ
##docker exec -it vault vault secrets enable rabbitmq

## 2. Configure las credenciales que Vault utiliza para comunicarse con RabbitMQ para Generar credenciales
##docker exec -t vault vault write rabbitmq/config/connection \
##   connection_uri="http://localhost:15672" \
##   username="admin" \
##   password="wad*dr=9RlGl"

## 3. Configurar una función que asigne un nombre en Vault a los permisos de host virtual    
##docker exec -it vault vault write rabbitmq/roles/rabbit-role \
##    vhosts='{"/":{"write": ".*", "read": ".*"}}'
##Success! Data written to: rabbitmq/roles/my-role

## User&Pass for mongoDb
##docker exec -it vault vault kv put secret/mongodb username=admin password=fosT9p4evAb9e

#docker exec -it vault vault operator init -address=http://127.0.0.1:8200

## User&Pass for postgresql
docker exec -it vault vault secrets enable -path='easytrack-api/database' database
docker exec -it vault vault secrets enable -path='easytrack-api/secrets' -version=2 kv

## Configure PostgreSQL connection 
docker exec -it vault vault write database/config/postgresql \
  plugin_name=postgresql-database-plugin \
  allowed_roles="db-role" \
  connection_url="postgresql://{{username}}:{{password}}@customerdb:5432/CustomerDb?sslmode=disable" \
  username="postgres" \
  password="postgres"
    
## Configure roles for dynamic credentials 
docker exec -it vault vault write database/roles/db-role \
  db_name=postgresql \
  creation_statements="
  CREATE ROLE \"{{name}}\" WITH LOGIN PASSWORD '{{password}}' VALID UNTIL '{{expiration}}'; 
  GRANT CREATE ON SCHEMA public TO \"{{name}}\";  
  GRANT SELECT, INSERT, DELETE, UPDATE ON ALL TABLES IN SCHEMA public TO \"{{name}}\";" \
  default_ttl="1h" \
  max_ttl="24h"  
  
docker exec -it vault vault read database/creds/db-role