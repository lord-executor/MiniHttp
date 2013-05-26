using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniHttp.Utilities
{
    public class UrlPath
    {
        public string Path { get; private set; }

        public bool IsAbsolute { get; private set; }

        public UrlPath(string path)
        {
            Path = path;
            if (Path.Length > 1 && Path.EndsWith("/"))
                Path = Path.Substring(0, Path.Length - 1);

            IsAbsolute = path.StartsWith("/");
        }

        public UrlPath Join(UrlPath other)
        {
            if (other.IsAbsolute)
                return other;

            return new UrlPath(String.Format("{0}/{1}", Path, other.Path));
        }

        public override string ToString()
        {
            return Path;
        }
    }

    public static class UriExtensions {
        public static UrlPath GetPath(this Uri uri)
        {
            return new UrlPath(uri.AbsolutePath);
        }
    }
}
