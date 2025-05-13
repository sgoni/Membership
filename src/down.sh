#!/bin/bash

echo Membership unmounting
docker compose down

echo Deleting volumes
docker volume prune

echo Unmounted sstup

read -p "Press enter to continue"