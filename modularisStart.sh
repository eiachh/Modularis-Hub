#!/bin/bash

echo "Starting modularis main"
dotnet /app/ModularisWebInterface/ModularisWebInterface.dll > /dev/null &

echo "Waiting 20 secfor startup"
sleep 20

echo "Healthy result:"
healthyResult=$(curl http://localhost:5000/healthy)
echo $healthyResult

echo "Setting workdir /app/ProjectModularisBot"
cd /app/ProjectModularisBot
while [[ $healthyResult != *"Healthy"* ]]; 
do
    echo "Healthy result RECHECK:"
    healthyResult=$(curl http://localhost:5000/healthy)
    echo $healthyResult
    echo "Waiting 5 sec before retrying"
    sleep 5
done
echo "All ok starting bot"
dotnet /app/ProjectModularisBot/ProjectModularisBot.dll
    