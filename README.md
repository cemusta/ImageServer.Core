# .Net Core Image Server

[![GitHub Actions](https://github.com/cemusta/ImageServer.Core/workflows/build/badge.svg)](https://github.com/cemusta/ImageServer.Core/actions) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=cemusta_ImageServer.Core&metric=alert_status)](https://sonarcloud.io/dashboard?id=cemusta_ImageServer.Core) [![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/cemusta/ImageServer.Core/master/LICENSE)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=cemusta_ImageServer.Core&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=cemusta_ImageServer.Core) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=cemusta_ImageServer.Core&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=cemusta_ImageServer.Core) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=cemusta_ImageServer.Core&metric=security_rating)](https://sonarcloud.io/dashboard?id=cemusta_ImageServer.Core)

[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=cemusta_ImageServer.Core&metric=coverage)](https://sonarcloud.io/dashboard?id=cemusta_ImageServer.Core)

This project is made for replacing python tornado image proxy server project internally used in [Hurriyet](http://www.hurriyet.com.tr). Written on .net core 3.0 with async i/o. Uses magick.net (imagemagick for .net) library for image operations. Tested in Linux and Windows.

Can read files from Mongo GridFS, File System or web url (can support multiple image sources simultaniously). Logs errors to console and Elastic stack (ELK) using NLog.

## Getting Started

Setting up the project is pretty straight thru. There is no external configuration necessary except the host setup. See deployment for notes on how to deploy the project on a live system.

When started as self hosted, project starts listening on ports 5000 and 5001.

At least one host configuration should be defined appsettings.json or webserver will not answer image or file endpoints.

## Configuration

Image Server takes image source configurations from appsettings.json. Image sources can defined inside the **"Hosts"** section of the json configuration file like this:

```sh
"Hosts": [
    {
      "Slug": "mng",
      "Type": 1,
      "ConnectionString": "mongodb://192.168.0.1:27017",
      "DatabaseName": "CMS_FS"
    },
    {
      "Slug": "local or shared disk",
      "Type": 0,
      "Path": "X:\\",
      "Whitelist": [ "800x600", "100x100" ]
    },
    {
      "Slug": "proxy",
      "Type": 2,
      "Backend": "https://imagizer.imageshack.com/"
    },
    {
      "Slug": "gsbucket",
      "Type": 3,
      "Backend": "bucket-name"
    }
  ]
```

There are 3 different host types: 0

- File Server: type 0, needs **"Path"** property which application has file access (can be readonly).
- GridFs: type 1, needs **"ConnectionString"** and **"DatabaseName"** properties to connect to a Mongo GridFs.
- Web Proxy: type 2, needs **"Backend"** property to a web host.
- Google Storege Bucket: type 3, needs **"Backend"** property of the target bucket containing imagery.

All of these host types need mandatory a **"Slug"** property which will be used later in routing for the image source.

**"Whitelist"** property can be used for limiting resizing requests to a specific set of resolutions.

**"FallbackImage"** property can be used for changing 404 results with another image, return 302 with that image's link.

## Usage

/i/ endpoint is used for image operations, has 5 different usage:

```
/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{id:gridfs}
/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{id:gridfs}
/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{options:opt}/{*id}
/i/{slug}/{quality:range(0,100)}/{w:range(0,5000)}x{h:range(0,5000)}/{*id}
/i/{slug}/{*id}
```

/f/ endpoint is used for file download operations:

```
/f/{slug}/{id:gridfs}.{ext}
/f/{slug}/{*id}
```

version endpoints can be used to check alive status and version of libraries used.

```
/status
/ver
/hosts
/formats
```

## Options

| Option     | Description                                                                                                                                                                                                                         |
| ---------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `f` or `t` | Make this image into a thumbnail. This method modifies the image to contain a thumbnail version of itself, no larger than the given size. This method calculates an appropriate thumbnail size to preserve the aspect of the image. |
| `g`        | This will convert the given image into a grayscale image.                                                                                                                                                                           |

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Deployment

Todo: Add additional notes about how to deploy this on a live system

## Built With

- [.Net Core 3.0](https://github.com/aspnet/Home)
- [Magick.NET](https://github.com/dlemstra/Magick.NET) - The .NET wrapper for the ImageMagick library
- [NLog.Web.Core](https://github.com/NLog/NLog.Web) - free logging platform for .NET

## Contributors

See the list of [contributors](https://github.com/cemusta/ImageServer.Core/graphs/contributors) who participated in this project.

## Acknowledgments

- Levent Yıldırım
- Ogun Isık
