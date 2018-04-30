# What are these tools?

NuGetMirror.exe is a command line tool to mirror nuget.org to disk, or any NuGet v3 feed. It supports filtering package ids and wildcards to narrow down the set of mirrored packages.

NuGet.CatalogReader is a library for reading package ids, versions, and the change history of a NuGet v3 feeds or nuget.org.

## Getting NuGetMirror

### Manually getting nugetmirror.exe (Windows and Mono)
1. Download the latest nupkg from [NuGet.org](https://www.nuget.org/packages/NuGetMirror)
1. Extract *tools/NuGetMirror.exe* to a local folder and run it.

### Install global tool (dotnet CLI >= 2.1.300-preview2)
1. `dotnet tool install -g nugetmirror`
1. `nugetmirror` should now be on your *PATH*

### Manually run nugetmirror.dll (dotnet CLI cross platform)
1. Download the latest nupkg from [NuGet.org](https://www.nuget.org/packages/NuGetMirror)
1. Extract the nupkg to a local folder
1. `dotnet <PathToNupkg>/tools/netcoreapp2.1/any/NuGetMirror.dll`

## Build Status

| AppVeyor | Travis | Visual Studio Online |
| --- | --- | --- |
| [![AppVeyor](https://ci.appveyor.com/api/projects/status/1i2k4gx5gfmmtyju?svg=true)](https://ci.appveyor.com/project/emgarten/nuget-catalogreader) | [![Travis](https://travis-ci.org/emgarten/NuGet.CatalogReader.svg?branch=master)](https://travis-ci.org/emgarten/NuGet.CatalogReader) | [![VSO](https://hackamore.visualstudio.com/_apis/public/build/definitions/abbff132-0981-4267-a80d-a6e7682a75a9/4/badge)](https://github.com/emgarten/nuget.catalogreader) |

## CI builds

CI builds are located on the following NuGet feed:

``https://nuget.blob.core.windows.net/packages/index.json``

The list of packages on this feed is [here](https://nuget.blob.core.windows.net/packages/nugetmirror.packageindex.json).

# Quick start


## Using NuGetMirror.exe

Mirror all packages to a folder on disk.

``NuGetMirror.exe nupkgs https://api.nuget.org/v3/index.json -o d:\tmp``
 
## Using NuGet.CatalogReader

Discover all packages in a feed using ``GetFlattenedEntriesAsync``. To see the complete history including edits use ``GetEntriesAsync``.

```csharp
var feed = new Uri("https://api.nuget.org/v3/index.json");

using (var catalog = new CatalogReader(feed))
{
    foreach (var entry in await catalog.GetFlattenedEntriesAsync())
    {
        Console.WriteLine($"[{entry.CommitTimeStamp}] {entry.Id} {entry.Version}");
    }
}
```

## Coding
This solution uses .NET Core, get the tools [here](http://dot.net/).

### License
[MIT License](https://raw.githubusercontent.com/emgarten/NuGet.CatalogReader/master/LICENSE)