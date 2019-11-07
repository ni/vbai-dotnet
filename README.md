# NI Vision Builder AI API for .NET

#

NI Vision Builder AI API for .NET is a collection of .NET wrapper functions that call the [Vision Builder AI](http://www.ni.com/en-us/shop/select/vision-builder-for-automated-inspection) API.

The Vision Builder AI support for 32-bit .NET was deprecated after version 2018 SP1.  New features added to the API after that version are not available for .NET users.

Now, the Vision Builder AI .NET API is available as an open source and intends to help users build the 32-bit .NET binary NationalInstruments.VBAI.2015.dll

This project intends to provide users the ability to add new .NET APIs and build custom binaries, for all the features available in Vision Builder AI.

# Dependencies

#

Users need to install the following dependencies to get started:

- Visual Studio – 2015
- .NET - 4.5
- Vision Builder AI – 2018 or later

# Getting Started

#

The following steps should be executed in order to build and use an existing .NET API:

1. Clone from the master branch.

2. Open the VBAI.sln from the src folder.

3. Make sure the target framework in the project-\>Properties-\>Application is set to ".NET Framework 4.5" for the VBAI project in VBAI.sln.
![alt text](https://github.com/ni/vbai-dotnet/blob/master/image.png)

4. Make sure to link "NationalInstruments.Vision.Common.dll" from https://github.com/ni/vdm-dotnet/tree/master/src/dotNet/Exports/VS2008/DotNET/Assemblies/Current to VBAI.csproj located in the src folder.

5. Build VBAI.sln. This will create NationalInstruments.VBAI.2015.dll in src\bin\x86\Release or src\bin\x86\Debug.

6. Link the dlls created in step 5 to your application.

# Examples

#

Run the Vision Builder AI example located in the Examples folder to get started.

1. Open examples\dotNet API.2015.sln in Visual Studio 2015.
2. Navigate to Solution Explorer -\> References.
3. Modify the following references in order to pick them from the right path in your system:
    1. NationalInstruments.VBAI.2015.dll
    2. NationalInstruments.Vision.Common.dll
	
For "NationalInstruments.VBAI.2015.dll", point it to where you have built the assembly in the previous section.
For "NationalInstruments.Vision.Common.dll", point it to https://github.com/ni/vdm-dotnet/tree/master/src/dotNet/Exports/VS2008/DotNET/Assemblies/Current.

4. Build the example project.
5. Navigate to the location where the example executable gets created (look for the path in output window while building the executable).
6. Launch the example executable from the path in step 5 and run.

# Bug Reports

#

To report a bug specific to vbai-dotnet, please use the [Github Issues page](https://github.com/ni/vbai-dotnet/blob/master/.github/ISSUE_TEMPLATE.md).

Fill in the issue template as completely as possible.

# Contributing

#

We welcome contributions! Please refer to [CONTRIBUTING.md](https://github.com/ni/vbai-dotnet/blob/master/CONTRIBUTING.md) page for information on how to contribute as well as what tests to add.

# License

#


**vbai-dotnet** is licensed under the [MIT License](https://github.com/ni/vbai-dotnet/blob/master/LICENSE)
