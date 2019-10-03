define([
    "dojo/_base/declare",
    "epi/shell/command/_Command",
    "epi/shell/Downloader",
    "epi/i18n!epi/cms/nls/episerver.cms.components.media"
], function (
    declare,
    _Command,
    Downloader,
    resources
) {

        return declare([_Command], {
            label: resources.command.download,
            tooltip: resources.command.download,
            iconClass: "epi-iconDownload",

            _onModelChange: function () {
                if (this.model === null || this.model.length === 0) {
                    this.set("canExecute", false);
                    this.set("isAvailable", false);
                    return;
                }

                if (!this._validateModels(this.model)) {
                    this.set("isAvailable", false);
                    return;
                }

                this.set("canExecute", true);
                this.set("isAvailable", true);
            },

            _validateModels: function (models) {
                if (!(models instanceof Array)) {
                    models = [models];
                }
                var hasUnsupportedTypes = models.some(function (model) {
                    return model.typeIdentifier !== "episerver.core.contentfolder" &&
                        model.typeIdentifier !== "episerver.core.contentassetfolder";
                });
                return !hasUnsupportedTypes;
            },

            _execute: function () {
                var ids = "";
                var name = "";

                if (this.model instanceof Array) {
                    ids = this.model.map(function (model) {
                        return model.contentLink;
                    }).join(",");
                    name = "media";
                } else {
                    ids = this.model.contentLink;
                    name = this.model.name;
                }
                if (!this.model) {
                    return;
                }

                Downloader.download("/cms-content-folder-download/" + ids, name + ".zip");
            }
        });
    });
