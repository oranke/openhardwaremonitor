# OpenHardwareMonitorLib + DLLExport

Forked from 
https://github.com/openhardwaremonitor/openhardwaremonitor/

I want import this awesome library in win32 app. (for example DELPHI) 
So I installed some nuget package (DLLExport, ILAsm) and very satisfied. 
But it was barely maintaind and I found useful fork of this library. 

https://github.com/hexagon-oss/openhardwaremonitor

It based on new .NET framework. Unfortunately DLLExport can't support this version. 
So I decided to keep project in `.NET 4.5` and apply patches after 0.9.6.

# Changes

2023.9.12. 
Add suppor for Intel Core 12th
https://github.com/openhardwaremonitor/openhardwaremonitor/pull/1475/commits

https://github.com/oranke/openhardwaremonitor.git