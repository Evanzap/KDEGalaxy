KDEGalaxy is a fully featured and customisable **ClassiCube Server Software** based on MCGalaxy.

At the time of writing this, the only feature that KDEGalaxy offers is changing the "Console [$(option)]" text in chat to "Konsole [$(option)]"<br>
Here is an example of the Konsole chat text:<br>
![opt2](https://github.com/Evanzap/KDEGalaxy/blob/master/images/konsole.png?raw=true)

I don't have anything else to say about this so here is an of KDE courtesy of https://www.kde.org/

![opt3](https://github.com/Evanzap/KDEGalaxy/blob/master/images/fullscreen_with_apps.png?raw=true)

Compiling - mono and .NET framework
-----------------
**With an IDE:**
* Visual Studio : Open `MCGalaxy.sln`, click `Build` in the menubar, then click `Build Solution`. (Or press F6)
* SharpDevelop: Open `MCGalaxy.sln`, click `Build` in the menubar, then click `Build Solution`. (Or press F8)

**Command line:**
* For Windows: Run `MSBuild command prompt for VS`, then type `msbuild MCGalaxy.sln` into command prompt
* Modern mono: Type `msbuild MCGalaxy.sln` into Terminal
* Older mono: Type `xbuild MCGalaxy.sln` into Terminal

Compiling - .NET 6 / .NET 5 / .NET Core
-----------------

* Compiling for .NET 6: No changes necessary
* Compiling for .NET 5: Change `TargetFramework` in CLI/MCGalaxyCLI_Core.csproj to `net5.0`
* Compiling for .NET Core: Change `TargetFramework` in CLI/MCGalaxyCLI_Core.csproj to `netcoreapp3.1`

Then navigate into `CLI` directory, and then run `dotnet build MCGalaxyCLI_Core.csproj`

**You will also need to copy `libsqlite3.so.0` from system libraries to `libsqlite3.so` in the server folder**

Copyright/License
-----------------
See LICENSE for MCGalaxy license, and license.txt for code used from other software.

Docker support
-----------------
Some **unofficial** dockerfiles for running MCGalaxy in Docker:
* [using Mono](https://github.com/UnknownShadow200/MCGalaxy/pull/577/files)
* [using .NET core](https://github.com/UnknownShadow200/MCGalaxy/pull/629/files)

Documentation
-----------------
* [General documentation](https://github.com/UnknownShadow200/MCGalaxy/wiki)
* [API documentation](https://github.com/ClassiCube/MCGalaxy-API-Documentation)
