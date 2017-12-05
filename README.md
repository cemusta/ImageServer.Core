# .Net Core Image Server

This project is made for replacing python tornado image proxy server project internally used in [Hurriyet](www.hurriyet.com.tr). Written on .net core 2.0 with async i/o. Uses magick.net (imagemagick for .net) library for image operations. Tested in Linux and Windows.

Can read image files from Mongo GridFS, File System or web url.

## Getting Started

Setting up the project is pretty straight thru. There is no external configuration necessary except the host setup. See deployment for notes on how to deploy the project on a live system.


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

### Prerequisites

-TBD-

### Installing

-TBD-

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
