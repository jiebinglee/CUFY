var FreshiAjax = $.extend({
    AjaxCallCounter: 0,
    TimeSpan: 20,
    AjaxStartTime: dateAdd("n",0),
    CheckTimeOut: function () {
        var self = this;
        var flag = true;
        if (dateAdd("n", 0) - self.AjaxStartTime >= self.TimeSpan * 60000) {
            flag = false;
            alert("超时");
            //back to home page.
        } else {
            self.AjaxStartTime = dateAdd("n", 0);
        }

        return flag;
    },
    AjaxCallWebMethod: function (url, methodName, data, pane, callback, async) {
        var self = this;
        var showLoadingPane = $("body");
        if (!isNullOrUndefined(pane)) {
            //showLoadingPane = "#" + pane;
            showLoadingPane = pane;
        }
        if (self.CheckTimeOut() == true) {            
            self.AjaxCallCounter++;

            $.ajax(
                {
                    url: url + methodName,
                    type: "POST",
                    headers: { "Cache-Control": "no-cache" },
                    async: async,
                    dataType: "json",
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () { showLoadingPane.showLoading(); },
                    complete: function () { showLoadingPane.hideLoading(); },
                    success: function (data, jqXHR, textStatus) {
                        self.AjaxCallCounter--;
                        if (self.AjaxCallCounter <= 0) {
                        }

                        data = verifyJsonString(data).d;
                        callback(data, jqXHR, textStatus);
                    },
                    error:  function (data) {
                        self.AjaxCallCounter--;
                        if (self.AjaxCallCounter <= 0) {
                        }

                        showLoadingPane.hideLoading();
                    }
                    
                });
        }
    }
})