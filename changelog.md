# Changelog
Notable changes to this solution are documented in this file using the 
[Keep a Changelog] style. The dates specified are in coordinated universal time (UTC).

[0.1.9371]: https://github.com/atecoder/dn.vi.lite

## [0.1.9371] - 2025-08-28
- Update MSTest SDK to 3.10.3
- Use preview in net standard classes.
- use isr.cc as company name in the Serilog settings generator.
- Turn off source version in MS Test, Demo and Console projects.
- Remove incorrect Generate Assembly Version Attribute project settings.
- Use file version rather than product version when building the Product folder name because starting with .NET 8 the product version includes the source code commit information, which is not necessary for defining the product folder for settings and logging.

## [0.1.8535] - 2023-05-15 Preview 202304
* Use cc.isr.Json.AppSettings.ViewModels project for settings I/O.

## [0.1.8518] - 2023-04-28 Preview 202304
* Split README.MD to attribution, cloning, open-source and read me files.
* Add code of conduct, contribution and security documents.
* Increment version.

## [0.1.8345] - 2022-11-06
* Uses the TCP Client and network stream classes from the System.Net.Sockets namespace. This requires a delay between successive queries; a kludge. 

## [0.1.8344] - 2022-11-05
* Proof of concept. First release using direct Socket class send and receive methods and having communication issues.

&copy;  2022 Integrated Scientific Resources, Inc. All rights reserved.

[Keep a Changelog]: https://keepachangelog.com/en/1.0.0/
