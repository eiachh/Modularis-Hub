#!/bin/bash

echo "Starting modularis main"
dotnet /app/ModularisWebInterface/ModularisWebInterface.dll &

healthyResult=$(curl http://localhost:5000/healthy)
echo $healthyResult
while [[ $healthyResult != *"Healthy"* ]]; 
do
    dotnet /app/ProjectModularisBot/ProjectModularisBot.dll &
    healthyResult=$(curl http://localhost:5000/healthy)
    echo $healthyResult
done