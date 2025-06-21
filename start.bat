@echo off
echo Launching server...
start cmd /k "ngrok http --url=ladybug-ideal-strangely.ngrok-free.app 1729"
node server.js
pause