# Cloning

- [Source Code](#Source-Code)
  - [Repositories](#Repositories)
  - [Global Configuration Files](#Global-Configuration-Files)
  - [Packages](#Packages)

<a name="Source-Code"></a>
## Source Code
Clone the repository along with its requisite repositories to their respective relative path.

### Repositories
The repositories listed in [external repositories] are required:
* [IDE Repository] - IDE support files.

```
git clone git@bitbucket.org:davidhary/vs.ide.git
git clone https://github.com/atecoder/dn.vi.lite.git
```

Clone the repositories into the following folders (parents of the .git folder):
```
%vslib%\core\ide
%dnlib%\vi\lite
```
where %dnlib% and %vslib% are  the root folders of the .NET libraries, e.g., %my%\lib\vs 
and %my%\libraries\vs, respectively, and %my% is the root folder of the .NET solutions

### Global Configuration Files
ISR libraries use a global editor configuration file and a global test run settings file. 
These files can be found in the [IDE Repository].

Restoring Editor Configuration:
```
xcopy /Y %my%\.editorconfig %my%\.editorconfig.bak
xcopy /Y %vslib%\core\ide\code\.editorconfig %my%\.editorconfig
```

Restoring Run Settings:
```
xcopy /Y %userprofile%\.runsettings %userprofile%\.runsettings.bak
xcopy /Y %vslib%\core\ide\code\.runsettings %userprofile%\.runsettings
```
where %userprofile% is the root user folder.

### Packages
TBA

[IVI Foundation]: https://www.ivifoundation.org
[Keysight I/O Suite]: https://www.keysight.com/en/pd-1985909/io-libraries-suite
[NI VISA]: https://www.ni.com/en-us/support/downloads/drivers/download.ni-visa.html#346210
[Test Script Builder]: https://www.tek.com/keithley-test-script-builder
[Microsoft .NET Framework]: https://dotnet.microsoft.com/download

[external repositories]: ExternalReposCommits.csv
[IDE Repository]: https://www.bitbucket.org/davidhary/vs.ide

[Use sockets to send and receive data over TCP]: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services
