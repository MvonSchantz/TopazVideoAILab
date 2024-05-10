LongPath
========

[![Build status](https://ci.appveyor.com/api/projects/status/w9410p6garuyba2b?svg=true)](https://ci.appveyor.com/project/peteraritchie/longpath) [![NuGet Badge](https://buildstats.info/nuget/pri.longpath)](https://www.nuget.org/packages/PRI.LongPath/)

A drop-in library to support long paths in .NET

|**Note**|_Instead of this package_, You may want to consider native support for long paths in Windows 10&ge;:<br/>[Enable Long Paths in Windows 10, Version 1607, and Later](https://docs.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=cmd#enable-long-paths-in-windows-10-version-1607-and-later)
|-|:-|

Supporting files and directories with a long path is fairly easy with Windows.  Unfortunately, other aspects of Windows haven't supported long paths in their entirely.  The file system (NTFS), for example, supports long paths quite well; but other things like Command Prompt and Explorer don't.  This makes it hard to entirely support long paths in *any* application, let alone in .NET.

This has been a bit tricky in .NET.  Several attempts like [longpaths.codeplex.com](http://longpaths.codeplex.com/) (which a more up to date version has made its way into .NET in classes like [LongPath](http://referencesource.microsoft.com/#mscorlib/system/io/longpath.cs) [LongPathFile](http://referencesource.microsoft.com/#mscorlib/system/io/longpath.cs#734b3020e7ff04fe#references) and [LongPathDirectory](http://referencesource.microsoft.com/#mscorlib/system/io/longpath.cs#ed4ae27b0c89bf61#references).  But, these libraries do not seem to support the entire original API (`Path`, `File`, `Directory`) and not all file-related APSs (including `FileInfo`, `DirectoryInfo`, `FileSystemInfo`).

Often times long path support is an after thought.  Usually after you've released something and someone logs bug (e.g. "When I use a path like c:\\users\\*300 chars removed*\\Document.docx your software gives me an error".  You can likely support long paths with the above-mentioned libraries, but you end up having to scrub your code and re-design it to suit these new APIs (causing full re-tests, potential new errors, potential regressions, etc.).

LongPath attempts to rectify that.

LongPath originally started as a fork of LongPaths on Codeplex; but after initial usage it was clear that much more work was involved to better support long paths.  So, I drastically expanded the API scope to include `FileInfo`, `DirectoryInfo`, `FileSystemInfo` to get 100% API coverage supporting long paths.  (with one caveat: `Directory.SetCurrentDirectory`, Windows does not support long paths for a current directory).

LongPaths allows your code to support long paths by providing a drop-in replacement for the following `System.IO` types: `FileInfo`, `DirectoryInfo`, `FileSystemInfo`, `FileInfo`, `DirectoryInfo`, `FileSystemInfo`.  You simply reference the Pri.LongPath types you need and you don't need to change your code.

Obviously to replace only 6 types in a namespaces (`System.IO`) and not the rest is problematic because you're going to need to use some of those other types (`FileNotFoundException`, `FileMode`, etc.)--which means referencing `System.IO` and re-introducing the original 6 types back into your scope.  I feft that not having to modify your code was the greater of the two evils.  Resolving this conflict is easily solved through aliases (see below).

**Note:** *the units test currently expect that the user running the tests has explicit rights (read/write/create) on the `TestResults` directory and all child directories.  It's usually easiest just to grant full control to your user on the root `LongPath` directory (and let it inherent to children).*

Usage
=====
The APIs provided have been made identical to the System.IO APIs as best I could (if not, please log an issue; fork and provide a failing unit test; or fork, fix, add passing test).  So, you really only need to force reference to the LongPath types.  Referencing the entire `Pri.LongPath` namespace will cause conflicts with `System.IO` because you almost always need to reference something *else* in `System.IO`.  To force reference to the `Pri.LongPath` types simply create aliases to them with the `using` directive:
```
	using Path = Pri.LongPath.Path;
	using Directory = Pri.LongPath.Directory;
	using DirectoryInfo = Pri.LongPath.DirectoryInfo;
	using File = Pri.LongPath.File;
```

`FileSystemInfo` is abstract so you shouldn't need to alias it, but, if you need it (for a cast, for example) alias it as well:
```
	using Path = Pri.LongPath.Path;
	using Directory = Pri.LongPath.Directory;
	using DirectoryInfo = Pri.LongPath.DirectoryInfo;
	using File = Pri.LongPath.File;
	using FileSystemInfo = Pri.LongPath.FileSystemInfo;
```
Then, of course, reference the assembly.

NuGet
=====
[https://www.nuget.org/packages/Pri.LongPath/](https://www.nuget.org/packages/Pri.LongPath/)

Known Issues
============

There are no known issues per se.  The only API that does not work as expected is `Directory.SetCurrentDirectory` as Windows does not support long paths for a current directory.

Caveats
=======

**TBD**

How long paths can be created
=============================

Long paths can be created *accidentally* in Windows making them very hard to process.

One way long paths can get created unintentially is via shares.  You can create a share to a directory (if it's not the root) such that the shared directory becomes the root of a virtual drive which then allows up to 260 characters of path under normal use.  Which means if the shared directory path is 20 chars, the actual path lenghts in the source can now be up to 280 chars--making them *invalid* in many parts of Windows in that source directory

**to be continued**
