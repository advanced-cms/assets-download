using EPiServer.Framework.Web.Resources;
using EPiServer.Shell;
using EPiServer.Shell.Modules;

namespace Advanced.CMS.MediaDownload
{
    public class MediaDownloadShellModule : ShellModule
    {
        public MediaDownloadShellModule(string name, string routeBasePath, string resourceBasePath)
            : base(name, routeBasePath, resourceBasePath)
        {
        }

        /// <inheritdoc />
        public override ModuleViewModel CreateViewModel(ModuleTable moduleTable, IClientResourceService clientResourceService)
        {
            return new MediaDownloadModuleViewModel(this, clientResourceService);
        }
    }

    public class MediaDownloadModuleViewModel : ModuleViewModel
    {
        public MediaDownloadModuleViewModel(ShellModule shellModule, IClientResourceService clientResourceService) :
            base(shellModule, clientResourceService)
        {
            ControllerUrl = Paths.ToResource("advanced-cms-media-download",
                $"FolderDownload/{nameof(FolderDownloadController.Index)}");
        }

        public string ControllerUrl { get; set; }
    }
}
