# .Net Core Image Server

This project is made for replacing python tornado image proxy server project internally used in [Hurriyet](www.hurriyet.com.tr). Written on .net core 2.0 with async i/o. Uses magick.net (imagemagick for .net) library for image operations. Tested in Linux and Windows.

Can read image files from Mongo GridFS, File System or web url.

## Getting Started

Setting up the project is pretty straight thru. There is no external configuration necessary except the host setup. See deployment for notes on how to deploy the project on a live system.

When started as self hosted, project starts listening on ports 5000 and 5001. 

At least one host configuration should be defined appsettings.json or webserver will not answer image or file endpoints.

## Usage

/status and /ver endpoints can be used to check alive status and version of libraries used.
```
/status
/ver
```

/i/ endpoint is used for image operations, has 4 different usage:
```
/i/{host}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{id:gridfs}
/i/{host}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{id:gridfs}
/i/{host}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{*id}
/i/{host}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{*id}
```
/f/ endpoint is used for file download operations:
```
/f/{host}/{id:gridfs}.{ext}
/f/{host}/{*id}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Deployment

Add additional notes about how to deploy this on a live system

## Built With

* [.Net Core 2.0](https://github.com/aspnet/Home) - Dependency Management
* [Magick.NET](https://github.com/dlemstra/Magick.NET) - The .NET wrapper for the ImageMagick library
* [NLog.Web](https://github.com/NLog/NLog.Web) - free logging platform for .NET

## Authors

* **Cem Usta** - *Initial work* - [cemusta](https://github.com/cemusta)

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## Acknowledgments

* Levent Yıldırım
* Ogun Isık
