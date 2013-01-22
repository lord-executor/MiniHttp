using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MiniHttp.Server;
using MiniHttp.Utilities;
using System.Text.RegularExpressions;

namespace MiniHttp.RequestHandlers
{
    public class DirectoryListingHandler : IRequestHandler
    {
        private static readonly Regex PlaceholderExp = new Regex(@"@(\w+)");
        private static string _fileListingTemplate;

        private static string FileListingTemplate
        {
            get
            {
                if (_fileListingTemplate == null)
                {
                    _fileListingTemplate = new StreamReader(
                        Assembly.GetExecutingAssembly()
                            .GetManifestResourceStream(typeof (DirectoryListingHandler), "Resources.FileListing.html")
                        ).ReadToEnd();
                }

                return _fileListingTemplate;
            }
        }

        private readonly IUrlMapper _urlMapper;

        public DirectoryListingHandler(IUrlMapper urlMapper)
        {
            _urlMapper = urlMapper;
        }

        private string Replace(Match m)
        {
            return String.Format("[{0}]", m.Groups[1].Value);
        }

        #region IRequestHandler Members

        public bool HandleRequest(RequestContext context)
        {
            var dir = _urlMapper.MapUrlToDirectory(context.Url);
            if (!dir.Exists)
                return false;

            context.Response.ContentType = "text/html";

            var replacementContext = new ReplacementContext(_urlMapper, dir, context.Url);
            var writer = new StreamWriter(context.Response.OutputStream);
            writer.Write(PlaceholderExp.Replace(FileListingTemplate, replacementContext.Replace));
            writer.Flush();

            return true;
        }

        #endregion

        private class ReplacementContext
        {
            private readonly IUrlMapper _mapper;
            private readonly DirectoryInfo _dir;
            private readonly Uri _requestUrl;
            private readonly Dictionary<string, Func<string>> _replacements;

            public ReplacementContext(IUrlMapper mapper, DirectoryInfo dir, Uri requestUrl)
            {
                _mapper = mapper;
                _dir = dir;
                _requestUrl = requestUrl;

                _replacements = new Dictionary<string, Func<string>>
                                    {
                                        {"FolderName", () => requestUrl.AbsolutePath},
                                        {"FileList", ListFiles},
                                    };
            }

            public string Replace(Match m)
            {
                var varName = m.Groups[1].Value;

                if (_replacements.ContainsKey(varName))
                    return _replacements[varName]();

                return String.Empty;
            }

            private string ListFiles()
            {
                var writer = new StringWriter();

                if (_requestUrl.Segments.Length > 1)
                {
                    var parent = _mapper.MapFileToUrl(_dir.Parent, _requestUrl);
                    writer.WriteLine("<li class=\"dir\"><a href=\"{0}\">{1}</a></li>", parent.AbsolutePath, "..");
                }

                foreach (var info in _dir.GetDirectories().OrderBy(i => i.Name))
                {
                    WriteFileSystemInfoItem(writer, info);
                }
                foreach (var info in _dir.GetFiles().OrderBy(i => i.Name))
                {
                    WriteFileSystemInfoItem(writer, info);
                }

                return writer.ToString();
            }

            private void WriteFileSystemInfoItem(TextWriter writer, FileSystemInfo info)
            {
                var infoUrl = _mapper.MapFileToUrl(info, _requestUrl);
                writer.WriteLine("<li class=\"{0}\"><a href=\"{1}\">{2}</a></li>",
                    info.Attributes.HasFlag(FileAttributes.Directory) ? "dir" : "file",
                    infoUrl.AbsolutePath,
                    info.Name);
            }
        }
    }
}
