var FreshiCore = $.extend({
    URL: "http://localhost:1081/cufyWebService.asmx/",
    PostProcess: function (managerName, methodName, params, pane, callback, async) {
        var self = this;

        params = FreshiTransBox.Create(managerName, methodName, params);
        params = { data: params, token: CreditManagement.CurrentToken };
        var method = "Process";

        FreshiAjax.AjaxCallWebMethod(self.URL, method, params, pane, callback, async);
    }
})

var FreshiTransBox = $.extend({
    Create: function (owner, id, content) {
        var transbox = {};
        transbox.OwnerClass = owner;
        transbox.OPID = id;
        transbox.IsOK = false;
        transbox.Message = "";
        transbox.Content = "";

        transbox.GetContent = function () {
            return  verifyJsonString(this.Content);
        };
        transbox.SetContent = function (data) {
            if ( isNullOrUndefined(data)) return;
            this.Content = JSON.stringify(data);
        };

        transbox.SetContent(content);

        return transbox;
    },
    CreateFrom: function (data) {
        if ( isNullOrUndefined(data)) return null;

        var transbox = {};
        transbox.OwnerClass = "";
        transbox.OPID = "";
        transbox.IsOK = false;
        transbox.Message = "";
        transbox.Content = "";

        transbox.GetContent = function () {
            return  verifyJsonString(this.Content);
        };
        transbox.SetContent = function (data) {
            if ( isNullOrUndefined(data)) return;
            this.Content = JSON.stringify(data);
        };

        if (! isNullOrUndefined(data.OwnerClass)) {
            transbox.OwnerClass = data.OwnerClass;
        }
        if (! isNullOrUndefined(data.OPID)) {
            transbox.OPID = data.OPID;
        }
        if (! isNullOrUndefined(data.IsOK)) {
            transbox.IsOK = data.IsOK;
        }
        if (! isNullOrUndefined(data.Message)) {
            transbox.Message = data.Message;
        }
        if (! isNullOrUndefined(data.Content)) {
            transbox.Content = data.Content;
        }

        return transbox;
    },    
    ConvertKeyValuePairList2SelectOptionList: function (pairList) {
        if ( isNullOrUndefined(pairList)) return null;
        if (pairList.length <= 0) return null;

        var optionList = [];
        $.each(pairList, function (idx, itm) {
            if ( isNullOrUndefined(itm.Key) ||  isNullOrUndefined(itm.Value)) return;
            optionList.push(new selectOption(itm.Value, itm.Key));
        });

        return optionList;
    },
    FindValueByKey: function (pairList, key) {
        if ( isNullOrUndefined(pairList)) return null;
        if (pairList.length <= 0) return null;

        var item = null;
        $.each(pairList, function (idx, itm) {
            if ( isNullOrUndefined(itm.Key) ||  isNullOrUndefined(itm.Value)) return;
            if (itm.Key == key) item = itm;
        });

        return ( isNullOrUndefined(item)) ? null : item.Value;
    }
})

var FreshiConfig = $.extend({
    PagerDefaultOptions: {
        currentPage: 1,
        totalPages: 1,
        bootstrapMajorVersion: 3,
        numberOfPages: 10,
        itemTexts: function (type, page, current) {
            switch (type) {
                case "first":
                    return "首页";
                case "prev":
                    return "上一页";
                case "next":
                    return "下一页";
                case "last":
                    return "末页";
                case "page":
                    return page;
            }
        },
        //useBootstrapTooltip: true,
        tooltipTitles: function (type, page, current) {
            switch (type) {
                case "first":
                    return "首页";
                case "prev":
                    return "上一页";
                case "next":
                    return "下一页";
                case "last":
                    return "末页";
                case "page":
                    return "第 " + page + " 页";
            }
        },
        shouldShowPage: function (type, page, current) {
            switch (type) {
                case "first":
                case "last":
                case "next":
                case "prev":
                case "page":
                    return true;
                default:
                    return true;
            }
        },
        itemContainerClass: function (type, page, current) {
            return (page === current) ? "active disabled" : "";
        },
        onPageChangedCallback: null,
        onPageChanged: function (e, oldPage, newPage) {
            FreshiConfig.PagerDefaultOptions.onPageChangedCallback(newPage);
        }
    }
})