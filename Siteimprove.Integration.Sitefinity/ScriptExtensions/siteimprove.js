function OnPageDetailViewLoaded(sender, args) {
    Sys.Application.add_init(function () {
        sender.add_formCreated(formCreatedHandler);
    });
}

function formCreatedHandler(sender, args) {
    if (args._commandName === 'create' || args._commandName === 'createChild' || args._commandName === 'duplicate') {
        var siteId = getUrlParameter("sf_site");

        jQuery.ajax({
            type: "GET",
            url: "/siteimprove/propertiesBySite/" + siteId,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: false,
            success: function (data) {
                if (data != "ERROR") {
                    pushDomainToSiteimprove(data.Token, data.Domain, data.ShouldLogActivity);
                }
            },
            error: function (data) { }
        });
    }
    else if (isEditDialogOnFrontendView(args)) {
        var pageItem = args.get_dataItem();

        if (pageItem) {
            var pageId = pageItem.Id;
            var pageUrl = pageItem.PageViewUrl;

            jQuery.ajax({
                type: "GET",
                url: "/siteimprove/propertiesByPage/" + pageId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                processdata: false,
                success: function (data) {
                    if (data != "ERROR") {
                        var fullUrl = data.Domain + pageUrl;
                        pushInputToSiteimprove(data.Token, fullUrl, data.ShouldLogActivity);
                    }
                },
                error: function (data) { }
            });
        }
    } else if (isEditDialogOnPageEditor(args)) {
        var pageItem = args.get_dataItem();

        if (pageItem) {
            var pageId = pageItem.Id;
            var rootId = pageItem.RootId;
            var culture = (typeof args._commandArgument.language !== 'undefined' && args._commandArgument.language !== null) ? args._commandArgument.language : "";
            var url = "/siteimprove/properties/" + rootId + "/" + pageId;
            if (culture.length > 0)
                url = url + "/" + culture;

            jQuery.ajax({
                type: "GET",
                url: url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                processdata: false,
                success: function (data) {
                    if (data != "ERROR") {
                        if (typeof data.PageUrl !== 'undefined' && data.PageUrl.length > 0)
                            pushInputToSiteimprove(data.Token, data.PageUrl, data.ShouldLogActivity);
                        else
                            pushDomainToSiteimprove(data.Token, data.Domain, data.ShouldLogActivity);
                    }
                },
                error: function (data) { }
            });
        }
    }
}

function pushInputToSiteimprove(token, pageUrl, shouldLogActivity) {
    var _si = window._si || [];
    _si.push(['input', pageUrl, token, function () {
        if (shouldLogActivity) {
            console.log('Loading the ' + pageUrl + ' page in the Siteimprove overlay');
        }
    }])
}

function pushDomainToSiteimprove(token, domain, shouldLogActivity) {
    var _si = window._si || [];
    _si.push(['domain', domain, token, function () {
        if (shouldLogActivity) {
            console.log('Loading the ' + domain + ' domain into the Siteimprove overlay');
        }
    }])
}

function isEditDialogOnFrontendView(args) {
    return (args._commandName === 'edit' && typeof args._params !== 'undefined' && typeof args._params.ViewName !== 'undefined' && args._params.ViewName === 'FrontendPagesEdit')
}

function isEditDialogOnPageEditor(args) {
    return (args._commandName === 'edit' && typeof args._params !== 'undefined' && typeof args._params.ViewName === 'undefined')
}

function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};