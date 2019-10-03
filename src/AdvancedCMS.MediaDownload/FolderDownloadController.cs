using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Blobs;
using EPiServer.Web.Routing;

namespace AdvancedCMS.MediaDownload
{
    /// <summary>
    /// Controller used to download media folder as ZIP
    /// </summary>
    //[Authorize(Roles = "CmsEdit")]
    [Authorize]
    public class FolderDownloadController : Controller
    {
        private readonly IContentLoader _contentLoader;

        public FolderDownloadController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public ActionResult Index(string contentFolderIds)
        {
            if (string.IsNullOrWhiteSpace(contentFolderIds))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var contentFolderIdsList = contentFolderIds.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            var hasFiles = false;
            var resultStream = new MemoryStream();
            var zipName = "";
            using (var archive = new ZipArchive(resultStream, ZipArchiveMode.Create, true))
            {
                var onlyOneFolder = contentFolderIdsList.Length == 1;

                foreach (var contentFolderId in contentFolderIdsList)
                {
                    var contentLink = new ContentReference(contentFolderId);
                    var content = _contentLoader.Get<IContent>(contentLink);
                    if (!(content is ContentFolder))
                    {
                        continue;
                    }
                    zipName = onlyOneFolder ? content.Name : "Media";
                    hasFiles = AddEntries(archive, contentLink, onlyOneFolder ? "": content.Name + "/") || hasFiles;
                }
            }

            if (!hasFiles)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            resultStream.Seek(0, SeekOrigin.Begin);
            
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = zipName + ".zip",
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(resultStream, "application/zip");
        }

        private bool AddEntries(ZipArchive archive, ContentReference contentLink, string folderName)
        {
            var result = false;

            var children = _contentLoader.GetChildren<IContent>(contentLink).ToList();

            foreach (var content in children)
            {
                if (content is ContentFolder)
                {
                    result = AddEntries(archive, content.ContentLink, folderName + content.Name + "/") || result;
                } else if (content is IContentMedia)
                {
                    result = true;
                    var contentMedia = content as IContentMedia;
                    var routable = content as IRoutable;
                    var extension = Path.GetExtension(content.Name);
                    if (string.IsNullOrEmpty(extension))
                    {
                        extension = Path.GetExtension(routable.RouteSegment);
                    }
                    var zipEntry = archive.CreateEntry(folderName + Path.GetFileNameWithoutExtension(content.Name) + extension);

                    var bytes = contentMedia.BinaryData.ReadAllBytes();
                    using (var writer = new StreamWriter(zipEntry.Open()))
                    {
                        using (var m = new MemoryStream())
                        {
                            m.Write(bytes, 0, bytes.Length);
                            m.Seek(0, SeekOrigin.Begin);
                            m.WriteTo(writer.BaseStream);
                        }
                    }
                }
            }

            return result;
        }
    }
}
