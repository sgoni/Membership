#!/bin/bash
# Infrastructure
docker exec -it consul-server consul services register -id=447b3988-f75e-43b6-8a5a-dfd6d65b860b -name=postgres -address=localhost -port=5432 -id=

# Services
#docker exec -it consul-server consul services register -id=c43c780d-e2dc-4a59-baba-42e33dd3a9f1 -name=customer-api -address=https://127.0.0.1 -port=6060