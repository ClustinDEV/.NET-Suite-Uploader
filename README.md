# .NET-Suite-Uploader
configurable NETSUITE FileCabinet uploader written in C#

The application login in Netsuite and upload files in the Cabinet using Suite Talk webservices

It use .json files for config a task that need to be execute, the .json file contain an array 
of local file or folder path and the cabinet folder internalid where the file will be uploaded.

Before start the exe you need to set params in the app.config:
- email (netsuite user)
- password (netsuite user)
- account (netsuite account code)
- path where put the Task files

and create your desired task files and put them in the Tasks folder.

Next features:
- Select the domain based on the requested account (list in config)