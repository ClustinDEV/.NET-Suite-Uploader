# .NET-Suite-Uploader
configurable NS FileCabinet uploader written in C#

The application login in Netsuite and upload files in the Cabinet using Suite Talk webservices

It use .json files for config a task that need to be execute, the .json file contain an array 
of local file path and the cabinet folder internalid where the file will be uploaded.

Before start the exe you need to set params in the app.config:
- email (netsuite user)
- password (netsuite user)
- account (netsuite account code)
- path where put the Task files

The app now work only on a single domain at time: system or system.na1

Next features:
- Change account dynamically
- Select the domain based on the requested account (list in config)
- Use folder in task
- Shortcut to Upload execution
- Enhanced log with execution status (icon, color, ...)
- Custom clustin icon