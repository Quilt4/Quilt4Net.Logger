# Quilt4Net Logger

There are three simple steps to get your logs sent to the Quilt4Net service.
1. Get your ApiKey at [Quilt4Net](https://quilt4net.com/)
1. Get nuget package `Install-Package Quilt4Net.Logger`
1. Add Quilt4Net as a logging service

```
builder.Services.AddLogging(configure => configure.Quilt4NetLogger(o =>
{
    o.ApiKey = "[ApiKey]";
}));
```

