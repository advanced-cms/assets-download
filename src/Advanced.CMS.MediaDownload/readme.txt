Advanced.CMS.MediaDownload


Installation
============


In order to start using MediaDownload you need to add it explicitly to your site.
Please add the following statement to your Startup.cs


public class Startup
{
    ...
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddMediaDownload();
        ...
    }
    ...
}

or you can just register a new protectedmodule in the appsettings.json

Full documentation can be found here: https://github.com/advanced-cms/assets-download
