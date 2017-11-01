# Using dotnet watch in deployment
___

### Introduction
___

At the beginning I'd like to introduce my motivation to create this example. My friends which moved from PHP to .Net stack, were complaining that they had to recompile application every time they made a little change in source code. They said: 

> "In PHP we've changed a script file, reload a page and I can see my changes. In C# we need to recompile whole application and that's annoying..."
 
For my that was naturally that I need to recompile ma application. That's the price for high performance. Accidentally I heard from [Tomasz Kopacz](https://www.linkedin.com/in/tomasz-kopacz-689539/ "Tomasz Kopacz") about *dotnet watch* and my "little" world has changed. Now I would like to share information about that tool with other developers.

This repository contains ASP.Net Core 2.0 application with *dotnet watch* tool used in development phase. I hope this tool will increase your productivity.

### Setup & run
___

Setup of *dotnet watch* tool is super easy. We need to add reference to *Microsoft.Dotnet.Watcher.Tools* in *.csproj file:

```xml
<ItemGroup>
    <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.0.0" />
</ItemGroup> 
```  

After that we need to restore packages in our project and Voila! Now we can use *dotnet watch* tool like:

```sh
dotnet restore

dotnet watch run

# example output
watch : Started
Hosting environment: Production
Content root path: (...)\dotnet-watch-example\ExampleMvcApp
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
``` 

Now we can make changes in our solution files. Every change will trigger *dotnet watch* tool and rebuild our application:

```sh
watch : Exited with error code 1
watch : File changed: (...)\dotnet-watch-example\ExampleMvcApp\Startup.cs
watch : Started
Hosting environment: Production
Content root path: (...)\dotnet-watch-example\ExampleMvcApp
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

That's all. Now you can play with my example on your own but remember that every change triggers a build. That results a downtime of our application. Keep in mind that every mistake in code will broke application which will be shut down until you fix bug in code. Example below:

```sh
watch : Exited with error code 1
watch : File changed: (...)\dotnet-watch-example\ExampleMvcApp\Startup.cs
watch : Started
Startup.cs(43,18): error CS1001: Identifier expected [(...)\dotnet-watch-example\ExampleMvcApp\ExampleMvcApp.csproj]
Startup.cs(43,18): error CS1002: ; expected [(...)\dotnet-watch-example\ExampleMvcApp\ExampleMvcApp.csproj]

The build failed. Please fix the build errors and run again.
watch : Exited with error code 1
watch : Waiting for a file to change before restarting dotnet...
```

### Tests
___
*Dotnet watch* can be used also to trigger application tests if there is any change in code. To achive that we need only setup test with *watch* tool. Example of execution:

```sh
dotnet watch test

watch : Started
Build started, please wait...
Build completed.

Test run for (...)\dotnet-watch-example\ExampleMvcApp.Tests\bin\Debug\netcoreapp2.0\ExampleMvcApp.Tests.dll(.NETCoreApp,Version=v2.0)
Microsoft (R) Test Execution Command Line Tool Version 15.3.0-preview-20170628-02
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
[xUnit.net 00:00:01.9710205]   Discovering: ExampleMvcApp.Tests
[xUnit.net 00:00:02.0615903]   Discovered:  ExampleMvcApp.Tests
[xUnit.net 00:00:02.1304412]   Starting:    ExampleMvcApp.Tests
[xUnit.net 00:00:02.3174366]     ExampleMvcApp.Tests.SimpleTest.SimpleTest_WithoutConditions_ShouldPass [FAIL]
[xUnit.net 00:00:02.3186889]       Assert.True() Failure
[xUnit.net 00:00:02.3188818]       Expected: True
[xUnit.net 00:00:02.3189584]       Actual:   False
[xUnit.net 00:00:02.3199073]       Stack Trace:
[xUnit.net 00:00:02.3212204]         (...)\dotnet-watch-example\ExampleMvcApp.Tests\SimpleTest.cs(11,0): at ExampleMvcApp.Tests.SimpleTest.SimpleTest_WithoutConditions_ShouldPass()
[xUnit.net 00:00:02.3342243]   Finished:    ExampleMvcApp.Tests
Failed   ExampleMvcApp.Tests.SimpleTest.SimpleTest_WithoutConditions_ShouldPass
Error Message:
 Assert.True() Failure
Expected: True
Actual:   False
Stack Trace:
   at ExampleMvcApp.Tests.SimpleTest.SimpleTest_WithoutConditions_ShouldPass() in (...)\dotnet-watch-example\ExampleMvcApp.Tests\SimpleTest.cs:line 11

Total tests: 1. Passed: 0. Failed: 1. Skipped: 0.
Test Run Failed.
Test execution time: 3,1153 Seconds
watch : Exited with error code 1
watch : Waiting for a file to change before restarting dotnet...

watch : Started
Build started, please wait...
Build completed.

Test run for (...)\dotnet-watch-example\ExampleMvcApp.Tests\bin\Debug\netcoreapp2.0\ExampleMvcApp.Tests.dll(.NETCoreApp,Version=v2.0)
Microsoft (R) Test Execution Command Line Tool Version 15.3.0-preview-20170628-02
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
[xUnit.net 00:00:01.9361049]   Discovering: ExampleMvcApp.Tests
[xUnit.net 00:00:02.0139917]   Discovered:  ExampleMvcApp.Tests
[xUnit.net 00:00:02.0731938]   Starting:    ExampleMvcApp.Tests
[xUnit.net 00:00:02.2357251]   Finished:    ExampleMvcApp.Tests

Total tests: 1. Passed: 1. Failed: 0. Skipped: 0.
Test Run Successful.
Test execution time: 2,9806 Seconds
watch : Exited
watch : Waiting for a file to change before restarting dotnet...
```

### Tips
___
We can extend *dotnet watch* tool to trigger a rebuild & restart our application not only after change in *.cs files but in other kind of files as well. In my opinion very usful is file mask on appsettings. Every change in this file will trigger *dotnet watch*. Time needed to restart our application will be shorter that after change in *.cs file becouse we don't need to rebuild the solution. Example of file mask on appsettings file below:

```xml
<ItemGroup>
    <!-- extends watching group to include .\appsettings*.json files -->
    <Watch Include=".\appsettings*.json" />
  </ItemGroup>
```

### Summary
___

To sum up *dotnet watch* is a very useful tool which will improve your productivity. In my opinion it will be espesially powerful is situation when are developing an application or check something very quickly and we don't want to run Visual Studio.

### Links
___

- [.NET command line tools repo](https://github.com/aspnet/DotNetTools ".Net command line tools repo")
- [Developing ASP.NET Core apps using dotnet watch article](https://docs.microsoft.com/en-us/aspnet/core/tutorials/dotnet-watch "Developing ASP.NET Core apps using dotnet watch article")