
## Scala
```scala
object abstractTypes extends Application {
  abstract class SeqBuffer {
    type T; val element: Seq[T]; def length = element.length
  }
}
```

## Ruby
```ruby
require 'redcarpet'
markdown = Redcarpet.new("Hello World!")
puts markdown.to_html
```

## XML
```xml
<?xml version="1.0" encoding="utf-8" ?>
<server port="8888">
	<preHook type="MiniHttp.RequestHooks.IndexRouting" />
	<route expression=".*" type="MiniHttp.Plugins.RequestHandlers.MarkdownHandler, MiniHttp.Plugins" />
	<route expression=".*" type="MiniHttp.RequestHandlers.StaticFileHandler" />
	<route expression=".*" type="MiniHttp.RequestHandlers.DirectoryListingHandler" />
	<route expression=".*" type="MiniHttp.RequestHandlers.NotFoundHandler" />
	<postHook type="MiniHttp.RequestHooks.ServerError" />
</server>
```

## C&#0035;
```cs
if (File.Exists(String.Format("{0}.md", file.FullName)))
{
    var builder = new UriBuilder(context.Url);
    builder.Path += ".md";
    context.Url = builder.Uri;
}
```

## Bash
```bash
cat /proc/mdstat
```

## C&#0035;
	var builder = new UriBuilder(context.Url);
		builder.Path += ".md";
    context.Url = builder.Uri;
