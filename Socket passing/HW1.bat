start "Server" %~dp0ServerVM\bin\Release\ServerVM.exe 127.0.0.1
start "Client2" %~dp0Client2VM\bin\Release\Client2VM.exe 127.0.0.1
start "Client1" %~dp0Client1VM\bin\Release\Client1VM.exe 127.0.0.1 127.0.0.1