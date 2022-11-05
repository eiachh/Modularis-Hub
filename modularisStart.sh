#!/bin/bash

echo "Starting modularis main"
dotnet /app/ModularisWebInterface/ModularisWebInterface.dll > /dev/null &

sleep 20
healthyResult=$(curl http://localhost:5000/healthy)
echo $healthyResult
while [[ $healthyResult != *"Healthy"* ]]; 
do
    cd /app/ProjectModularisBot
    dotnet /app/ProjectModularisBot/ProjectModularisBot.dll > /dev/null &
    healthyResult=$(curl http://localhost:5000/healthy)
    echo $healthyResult
    sleep 5
done