using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MiniHttp.Server;
using MiniHttp.Utilities;

namespace MiniHttp.RequestHooks
{
    public class IndexRouting : IRequestHook
    {
        private readonly IUrlMapper _urlMapper;

		public IndexRouting(IUrlMapper urlMapper)
        {
			_urlMapper = urlMapper;
        }

        #region IRequestHook Members

        public void ProcessRequest(RequestContext context)
        {
			var dir = _urlMapper.MapUrlToDirectory(context.Url.GetPath());
            if (!dir.Exists)
                return;

            var file = dir.GetFiles("index.*").FirstOrDefault();
            if (file != null)
            {
                var builder = new UriBuilder(context.Url);
                builder.Path = _urlMapper.MapFileToUrl(file).Path;
				context.Url = builder.Uri;
            }
        }

        #endregion
    }
}
