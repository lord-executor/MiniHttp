using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MiniHttp.Server;
using MiniHttp.Utilities;

namespace MiniHttp.RequestProcessors
{
    public class IndexRouting : IRequestProcessor
    {
        private readonly IUrlMapper _urlMapper;

		public IndexRouting(IUrlMapper urlMapper)
        {
			_urlMapper = urlMapper;
        }

        #region IRequestProcessor Members

        public void ProcessRequest(RequestContext context)
        {
			var dir = _urlMapper.MapUrlToDirectory(context.Url);
            if (!dir.Exists)
                return;

            var file = dir.GetFiles("index.*").FirstOrDefault();
            if (file != null)
            {
				context.Url = _urlMapper.MapFileToUrl(file, context.Url);
            }
        }

        #endregion
    }
}
