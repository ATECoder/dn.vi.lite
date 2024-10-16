### VI Lite

![IDNQueryOnAndroidEmulator](https://user-images.githubusercontent.com/791370/200157385-80713165-cb2a-4223-9894-44ccf09f420b.png)

TCP Socket-based control and communication library for LXI-based instruments. 

### *** WARNING: Work in progress ***

* [Objectives](#Objectives)
* [Issues](#Issues)
  * [Read fails](#Read-fails) 
  * [Additional exploration](#Additional-exploration)
* [Status](#Status) 
* [Work plan](#work-plan)
* [Supported .Net Releases](#Supported-.Net-Releases)
* [Runtime Pre-Requisites](#Runtime-Pre-Requisites)
* Project README files:
  * [cc.isr.VI.Lite.IdentityReader](/src/apps/cc.isr.VI.Lite.IdentityReader/readme.md) 
* [Attributions](Attributions.md)
* [Change Log](./CHANGELOG.md)
* [Cloning](Cloning.md)
* [Code of Conduct](code_of_conduct.md)
* [Contributing](contributing.md)
* [Legal Notices](#legal-notices)
* [License](LICENSE)
* [Open Source](Open-Source.md)
* [Repository Owner](#Repository-Owner)
* [Security](security.md)

<a name="objectives"></a>
#### Objectives

Create a very rudimentary way to communicate with LXI instruments in mobile and desktop platforms.

Unlike VXI-11 and HiSlip, this will likely have no way of determining service request status. 

This, I hope, will evolve to implementing VXI-11 and HiSlip protocols hopefully leading to production grade APIs.

#### Status

Presently I am working on a proof of concept.

The proof-of-concept implementation defines a session class which owns an instance of a System.Net.Sockets.TcpClient and uses a NetworkStream instance to communicate with the instrument asynchronously. 

Pressing the 'Click Me', sends an IDN? query to the instrument and reads the instrument response using asynchronous socket methods. 

<a name="Issues"></a>
#### Issues

The following issues have been observed thus far:

##### Read fails

Initially, the socket send and receive commands were used. This worked as long as a new session was instantiated for each query and the socket was closed after each read.

With the current, network stream, the instrument occasionally fails to send the response.

Temporary solution:
Adding a 10 delay between queries seems to address these failures.

Obviously, this is a kludge.

##### Additional exploration

###### Test results for read after write and inter-query delays

|Instrument | Read after Write Delay [ms] | Inter-query Delay [ms]
|-----------|:-------------:|:------------:
|2450 |2  |0
|2600 |2  |0
|6510 |2  |1
|7510 |2  |1

Table 1. Delay times are required between the write and read commands of the same queries (Read After Write Delay) and between the read and the next write command of two sequential queries to accomplish error free readings. The delays listed in this table were determined for 40 error free queries initiated by button clicks. Missing responses were observed when reducing the delays by 1 ms.

#### Work plan

* Proof of concept: implement a .NET MAUI *IDN?
* Minimal VI Lite message based socket session:
	* Define the minimal interface
	* Implement and test the minimal interface.
	* Implement a .NET MAUI Lite application.
* Add device clear.

<a name="Supported-.Net-Releases"></a>
#### Supported .NET Releases

* .NET 6.0
* .NET MAUI 

<a name="Runtime-Pre-Requisites"></a>
#### Runtime Pre-Requisites

<a name="Repository-Owner"></a>
#### Repository Owner
[ATE Coder]

<a name="Authors"></a>
#### Authors
* [ATE Coder]  

<a name="legal-notices"></a>
#### Legal Notices

Integrated Scientific Resources, Inc., and any contributors grant you a license to the documentation and other content in this repository under the [Creative Commons Attribution 4.0 International Public License], see the [LICENSE](./LICENSE) file, and grant you a license to any code in the repository under the [MIT License], see the [LICENSE-CODE](./LICENSE-CODE) file.

Integrated Scientific Resources, Inc., and/or other Integrated Scientific Resources, Inc., products and services referenced in the documentation may be either trademarks or registered trademarks of Integrated Scientific Resources, Inc., in the United States and/or other countries. The licenses for this project do not grant you rights to use any Integrated Scientific Resources, Inc., names, logos, or trademarks.

Integrated Scientific Resources, Inc., and any contributors reserve all other rights, whether under their respective copyrights, patents, or trademarks, whether by implication, estoppel or otherwise.

[Creative Commons Attribution 4.0 International Public License]: https://creativecommons.org/licenses/by/4.0/legalcode
[MIT License]: https://opensource.org/licenses/MIT
 
[ATE Coder]: https://www.IntegratedScientificResources.com
[dn.core]: https://www.bitbucket.org/davidhary/dn.core

