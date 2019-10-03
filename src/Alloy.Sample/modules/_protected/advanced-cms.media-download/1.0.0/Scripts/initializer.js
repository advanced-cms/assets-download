define([
    "dojo/_base/declare",
    "epi/_Module",
    "epi-cms/plugin-area/assets-pane",
    "advanced-cms-media-download/DownloadFolderCommand",
], function (
    declare,
    _Module,
    assetsPanePluginArea,
    DownloadFolderCommand
) {
    return declare([_Module], {
        initialize: function () {
            this.inherited(arguments);

            assetsPanePluginArea.add(DownloadFolderCommand);
        }
    });
});
