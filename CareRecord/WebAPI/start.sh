#!/bin/bash

# Start the .NET application
dotnet /app/Interneuron.CareRecord.API.dll &

# Start Nginx
nginx -g 'daemon off;'
