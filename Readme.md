# OpenHardwareMonitorLib + DLLExport

Forked from 
https://github.com/openhardwaremonitor/openhardwaremonitor/

I want to use this awesome library for my win32 apps.  
So I installed some nuget package (DLLExport, ILAsm) and very satisfied.   
But it was barely maintaind so I found useful fork of this repository.   

https://github.com/hexagon-oss/openhardwaremonitor

It based on new .NET environment. Unfortunately DLLExport can't support this version.   
So I decided to keep project in `.NET 4.5` and apply patches after 0.9.6.

# Changes

## 2023.9.12. 
Add suppor for Intel Core 11~13th
https://github.com/openhardwaremonitor/openhardwaremonitor/pull/1475/commits


## 2023.10.4
Add Raptor Lane-N (Intel COre 13th) support.  
https://github.com/LibreHardwareMonitor/LibreHardwareMonitor  
https://github.com/LibreHardwareMonitor/LibreHardwareMonitor/commit/a6f28dc7e6b7fd8333d7257bf86ab21d0829beda  

