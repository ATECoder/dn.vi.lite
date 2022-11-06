# VI Lite

![IDNQueryOnAndroidEmulator](https://user-images.githubusercontent.com/791370/200157385-80713165-cb2a-4223-9894-44ccf09f420b.png)

TCP Socket-based control and communication library for LXI-based instruments. 

# *** WARNING: Work in progress ***

- [Objectives](#Objectives)
- [Issues](#Issues)
  - [Read fails](#Read-fails) 
  - [Additional exploration](#Additional-exploration)
- [Status](#Status) 
- [Work plan](#work-plan)
- [Supported .Net Releases](#Supported-.Net-Releases)
- [Runtime Pre-Requisites](#Runtime-Pre-Requisites)
- [Source Code](#Source-Code)
  - [Repositories](#Repositories)
  - [Global Configuration Files](#Global-Configuration-Files)
  - [Packages](#Packages)
- [Facilitated By](#FacilitatedBy)
- [Authors](#Authors)
- [Acknowledgments](#Acknowledgments)
- [Open Source](#Open-Source)
- [Closed Software](#Closed-software)
- [Legal Notices](#Legal-Notices)

<a name="objectives"></a>
## Objectives

Create a very rudimentary way to communicate with LXI instruments in mobile and desktop platforms.

Unlike VXI-11 and HiSlip, this will likely have no way of determining service request status. 

This, I hope, will evolve to implementing VXI-11 and HiSlip protocols hopefully leading to production grade APIs.

## Status

Presently I am working on a proof of concept.

The proof-of-concept implementation defines a session class which owns an instance of a System.Net.Sockets.TcpClient and uses a NetworkStream instance to communicate with the instrument asynchronously. 

Pressing the 'Click Me', sends an IDN? query to the instrument and reads the instrument response using asynchronous socket methods. 

<a name="Issues"></a>
## Issues

The following issues have been observed thus far:

### Read fails

Initially, the socket send and receive commands were used. This worked as long as a new session was instantiated for each query and the socket was closed after each read.

With the current, network stream, the instrument occasionally fails to send the response.

Temporary solution:
Adding a 10 delay between queries seems to address these failures.

Obviously, this is a kludge.

### Additional exploration

* test with the 7510 and 2450;
* alter the delays with these two instruments;
* document how consistent the results are.

## Work plan

* Proof of concept: implement a .NET MAUI *IDN?
* Minimal VI Lite message based socket session:
	* Define the minimal interface
	* Implement and test the minimal interface.
	* Implement a .NET MAUI Lite application.
* Add device clear.

<a name="Supported-.Net-Releases"></a>
## Supported .NET Releases

* .NET 6.0
* .NET MAUI 

<a name="Runtime-Pre-Requisites"></a>
## Runtime Pre-Requisites

<a name="Source-Code"></a>
## Source Code
Clone the repository along with its requisite repositories to their respective relative path.

### Repositories
The repositories listed in [external repositories] are required:
* [IDE Repository] - IDE support files.

```
git clone git@bitbucket.org:davidhary/vs.ide.git
git clone https://github.com/ATECoder/dn.vi.lite.git
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

<a name="FacilitatedBy"></a>
## Facilitated By
* [Visual Studio]
* [Jarte RTF Editor]
* [Wix Toolset]
* [Atomineer Code Documentation]
* [EW Software Spell Checker]
* [Code Converter]
* [Funduc Search and Replace]
* [IVI Foundation] - IVI Foundation VISA
* [Keysight I/O Suite] - I/O Libraris
* [Test Script Builder] - Test Script Builder

<a name="Repository-Owner"></a>
## Repository Owner
* [ATE Coder]

<a name="Authors"></a>
## Authors
* [ATE Coder]  

<a name="Acknowledgments"></a>
## Acknowledgments
* [Its all a remix] -- we are but a spec on the shoulders of giants  
* [John Simmons] - outlaw programmer  
* [Stack overflow] - Joel Spolsky  
* [.Net Foundation] - The .NET Foundation

<a name="Open-Source"></a>
## Open source
Open source used by this software is described and licensed at the
following sites:  

<a name="Closed-software"></a>
## Closed software
Closed software used by this software are described and licensed on
the following sites:  
[IVI Foundation]  
[Test Script Builder]  
[Keysight I/O Suite]  

<a name="Resources"></a>
## Resources 

[Use sockets to send and receive data over TCP]


<a name="Legal-Notices"></a>
## Legal Notices

Integrated Scientific Resources, Inc., and any contributors grant you a license to the documentation and other content
in this repository under the [Creative Commons Attribution 4.0 International Public License](https://creativecommons.org/licenses/by/4.0/legalcode),
see the [LICENSE](LICENSE) file, and grant you a license to any code in the repository under the [MIT License](https://opensource.org/licenses/MIT), see the
[LICENSE-CODE](LICENSE-CODE) file.

Integrated Scientific Resources, Inc., and/or other Integrated Scientific Resources, Inc., products and services referenced in the documentation
may be either trademarks or registered trademarks of Integrated Scientific Resources, Inc., in the United States and/or other countries.
The licenses for this project do not grant you rights to use any Integrated Scientific Resources, Inc., names, logos, or trademarks.

Integrated Scientific Resources, Inc., and any contributors reserve all other rights, whether under their respective copyrights, patents,
or trademarks, whether by implication, estoppel or otherwise.

[IVI Foundation]: https://www.ivifoundation.org
[Keysight I/O Suite]: https://www.keysight.com/en/pd-1985909/io-libraries-suite
[NI VISA]: https://www.ni.com/en-us/support/downloads/drivers/download.ni-visa.html#346210
[Test Script Builder]: https://www.tek.com/keithley-test-script-builder
[Microsoft .NET Framework]: https://dotnet.microsoft.com/download

[external repositories]: ExternalReposCommits.csv
[IDE Repository]: https://www.bitbucket.org/davidhary/vs.ide

[ATE Coder]: https://www.IntegratedScientificResources.com
[Its all a remix]: https://www.everythingisaremix.info
[John Simmons]: https://www.codeproject.com/script/Membership/View.aspx?mid=7741
[Stack overflow]: https://www.stackoveflow.com

[Visual Studio]: https://www.visualstudio.com/
[Jarte RTF Editor]: https://www.jarte.com/ 
[WiX Toolset]: https://www.wixtoolset.org/
[Atomineer Code Documentation]: https://www.atomineerutils.com/
[EW Software Spell Checker]: https://github.com/EWSoftware/VSSpellChecker/wiki/
[Code Converter]: https://github.com/icsharpcode/CodeConverter
[Funduc Search and Replace]: http://www.funduc.com/search_replace.htm
[.Net Foundation]: https://source.dot.net

[Use sockets to send and receive data over TCP]: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/sockets/socket-services
