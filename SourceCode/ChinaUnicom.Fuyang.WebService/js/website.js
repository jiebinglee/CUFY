var CreditManagement = $.extend({
    OwnerClass: "CMManager",
    UserType: { Admin: 1, Area: 2, Channel: 3 },
    CurrentToken: "",
    CurrentUserType: 0,
    LoginErrorPopover: function (element, content, placement) {
        $(element).popover({
            html: true,
            placement: 'right',
            content: "<span style='color: #a94442;'>" + content + "</span>",
            container: 'body'
        })

        $(element).popover('show');

        $(element).on('shown.bs.popover', function () {
            setTimeout(function () { $(element).popover('destroy'); }, 2000);
        })
    },
    LoadLoginPage: function () {
        $("#tb_UserName").focus();
        $("#btn_Login").on('click', function () {
            CreditManagement.Login();
        });
    },    
    Login: function () {
        var self = this;
        var userName = $("#tb_UserName").val();
        var password = $("#tb_Password").val();

        if (isNullOrTrimEmpty(userName)) {
            CreditManagement.LoginErrorPopover("#tb_UserName", "请输入用户名！");
            return;
        }

        if (isNullOrTrimEmpty(password)) {
            CreditManagement.LoginErrorPopover("#tb_Password", "请输入密码！");
            return;
        }
        
        var user = {
            UserName: userName,
            Password: password
        };
        FreshiCore.PostProcess(self.OwnerClass, "Login", user, $("#loginPane"), self.LoginCallback);
    },
    LoginCallback: function (data, status) {
        var trans = FreshiTransBox.CreateFrom(data);
        var content = trans.GetContent();
        if (!isNullOrUndefined(content)) {
            var token = content.Token;
            var userType = content.UserType;

            if (isNullOrTrimEmpty(token)) {
                CreditManagement.LoginErrorPopover("#btn_Login", "请输入正确的用户名和密码！");
                return;
            }

            createCookie('token', token, 1);
            CreditManagement.CurrentToken = token;

            switch (userType) {
                case CreditManagement.UserType.Channel:
                    window.location = "index.html";
                    break;
                case CreditManagement.UserType.Area:
                    window.location = "area/index.html";
                    break;
                case CreditManagement.UserType.Admin:
                    window.location = "admin/index.html";
                    break;
            }
        } else {
            CreditManagement.LoginErrorPopover("#btn_Login", "请输入正确的用户名和密码！");
            return;
        }
    },    
    CheckUser: function (userType) {
        var currentToken = readCookie('token');

        var flag = false;

        if (isNullOrTrimEmpty(currentToken)) {
            window.location = "login.html";
        } else {
            CreditManagement.CurrentToken = currentToken;

            var params = {
                UserType: userType
            };

            FreshiCore.PostProcess(CreditManagement.OwnerClass, "CheckUser", params, null, function (data, status) {
                var trans = FreshiTransBox.CreateFrom(data);
                var content = trans.GetContent();
                if (!isNullOrUndefined(content) && content != true) {
                    window.location = "/login.html";
                }

                flag = true;
            }, false);
        }

        return flag;
    },
    LoadChannelIndexPage: function () {
        if (CreditManagement.CheckUser(CreditManagement.UserType.Channel)) {
            $("#bt_Search").on("click", CreditManagement.SearchChannel);
            
            CreditManagement.LoadChannelInfo();
        }
    },
    LoadChannelInfo: function () {
        FreshiCore.PostProcess(CreditManagement.OwnerClass, "GetChannel", null, $("#channelPane"), CreditManagement.LoadChannelInfoCallback);
    },
    LoadChannelInfoCallback: function (data, status) {
        var trans = FreshiTransBox.CreateFrom(data);
        var content = trans.GetContent();
        if (!isNullOrUndefined(content)) {
            var channelInfo = content.ChannelInfo;
            var tdList = $("#table_ChannelInfo").find("tbody tr").eq(0).find("td");
            tdList.eq(0).text(channelInfo.AreaCode);
            tdList.eq(1).text(channelInfo.AreaName);
            tdList.eq(2).text(channelInfo.ChannelCode);
            tdList.eq(3).text(channelInfo.ChannelName);
            tdList.eq(4).text(channelInfo.ChannelLevelDesc);
            tdList.eq(5).text(channelInfo.ChannelContractCredit);
            tdList.eq(6).text(channelInfo.ChannelDevelopmentCredit);
            tdList.eq(7).text(channelInfo.ChannelYearBonus);
            tdList.eq(8).text(channelInfo.ChannelTotalAmount);
            tdList.eq(9).text(channelInfo.ChannelExchangedCredit);
            tdList.eq(10).text(channelInfo.ChannelRemainingTotalAmount);
                        
            var year = [];            
            for (var i = 2014; i <= new Date().getFullYear() ; i++) {
                year.push({ id: i, text: i.toString() });
            }

            var month = [];
            for (var i = 1; i <= 12; i++) {
                month.push({ id: i, text: i.toString() });
            }

            $("#select_SearchBeginYear").select2({                
                data: year
            });
            $("#select_SearchBeginMonth").select2({
                data: month
            });
            $("#select_SearchEndYear").select2({
                data: year
            });
            $("#select_SearchEndMonth").select2({
                data: month
            });
        }
    },
    SearchChannel: function () {
        $("#div_DevSummaryByTotal").empty();
        $("#div_DevSummaryByPeriod").empty();

        var beginYear = $("#select_SearchBeginYear").select2("val");
        var beginMonth = $("#select_SearchBeginMonth").select2("val");
        var endYear = $("#select_SearchEndYear").select2("val");
        var endMonth = $("#select_SearchEndMonth").select2("val");

        if (isNullOrTrimEmpty(beginYear)) {
            $("#alerModal_body").html("请选择查询起始年！");
            $('#alertModal').modal({
                backdrop: true,
                keyboard: true,
                show: true
            });
            return;
        }

        if (isNullOrTrimEmpty(beginMonth)) {
            $("#alerModal_body").html("请选择查询起始月！");
            $('#alertModal').modal({
                backdrop: true,
                keyboard: true,
                show: true
            });
            return;
        }

        if (isNullOrTrimEmpty(endYear)) {
            $("#alerModal_body").html("请选择查询结束年！");
            $('#alertModal').modal({
                backdrop: true,
                keyboard: true,
                show: true
            });
            return;
        }

        if (isNullOrTrimEmpty(endMonth)) {
            $("#alerModal_body").html("请选择查询结束月！");
            $('#alertModal').modal({
                backdrop: true,
                keyboard: true,
                show: true
            });
            return;
        }

        var validatePeriod = true;
        if (parseInt(beginYear) > parseInt(endYear)) {
            validatePeriod = false;
        } else if (parseInt(beginYear) == parseInt(endYear) && parseInt(beginMonth) > parseInt(endMonth)) {
            validatePeriod = false;
        }

        if (validatePeriod == false) {
            $("#alerModal_body").html("查询起始年月不能晚于结束年月！");
            $('#alertModal').modal({
                backdrop: true,
                keyboard: true,
                show: true
            });
            return;
        }        

        var SearchPeriod = {
            BeginYear: beginYear,
            BeginMonth: beginMonth,
            EndYear: endYear,
            EndMonth: endMonth
        };

        FreshiCore.PostProcess(CreditManagement.OwnerClass, "SearchChannel", SearchPeriod, $("#searchChannelPane"), CreditManagement.SearchChannelCallback);
    },
    SearchChannelCallback: function (data, status) {
        var trans = FreshiTransBox.CreateFrom(data);
        var content = trans.GetContent();
        if (!isNullOrUndefined(content)) {
            $("#div_DevSummaryByTotal").append(CreditManagement.CreateDevSummaryPanel(content.DevelopmentSummaryByTotal));

            $.each(content.DevelopmentSummaryByPeriod, function (index, item) {
                $("#div_DevSummaryByPeriod").append(CreditManagement.CreateDevSummaryPanel(item));
            });
        }
    },
    CreateDevSummaryPanel: function (devSummaryData) {
        var div1 = $("<div class='col-md-6'></div>");
        var div2 = $("<div class='panel panel-default'></div>");
        var div3 = $("<div class='panel-heading'></div>");
        div3.text(devSummaryData.SummaryPeriod);
        var div4 = $("<div class='panel-body'></div>");
        var table = $("<table class='table table-bordered table-striped'></table>");

        var thead = $("<thead></thead");
        var thead_tr = $("<tr></tr>");
        thead_tr.addClass('table-header');
        var th1 = $("<th>类型</th>");
        th1.appendTo(thead_tr);
        var th2 = $("<th>数量</th>");
        th2.appendTo(thead_tr);
        var th3 = $("<th>积分基数</th>");
        th3.appendTo(thead_tr);
        var th4 = $("<th>系数</th>");
        th4.appendTo(thead_tr);
        var th5 = $("<th>积分</th>");
        th5.appendTo(thead_tr);
        thead_tr.appendTo(thead);
        thead.appendTo(table);

        var tbody = $("<tbody></tbody>");
        $.each(devSummaryData.DevelopmentInfos, function (index, item) {
            var tbody_tr = $("<tr></tr>");
            var td1 = $("<td></td>");
            td1.text(item.DevTypeDesc);
            td1.appendTo(tbody_tr);
            var td2 = $("<td></td>");
            td2.text(item.DevCount);
            td2.appendTo(tbody_tr);
            var td3 = $("<td></td>");
            td3.text(item.CreditBase);
            td3.appendTo(tbody_tr);
            var td4 = $("<td></td>");
            td4.text(item.CreditRatio);
            td4.appendTo(tbody_tr);
            var td5 = $("<td></td>");
            td5.text(item.CreditAmount);
            td5.appendTo(tbody_tr);
            tbody_tr.appendTo(tbody);
        });        
        tbody.appendTo(table);

        table.appendTo(div4);

        div3.appendTo(div2);
        div4.appendTo(div2);

        div2.appendTo(div1);

        return div1;        
    },
    /*---------------------------------------------------ADMIN---------------------------------------------------*/
    LoadAdminIndexPage: function () {
        if (CreditManagement.CheckUser(CreditManagement.UserType.Admin)) {

            $("#bt_Import").on('click', function () {
                var importType = $("#select_ImportType").select2("val");

                if (isNullOrTrimEmpty(importType)) {
                    $("#alerModal_body").html("请选择导入类型！");
                    $('#alertModal').modal({
                        backdrop: true,
                        keyboard: true,
                        show: true
                    });
                    return;
                }

                var inputFile = $("#file_InputFile").val();

                if (isNullOrTrimEmpty(inputFile)) {
                    $("#alerModal_body").html("请选择导入文件！");
                    $('#alertModal').modal({
                        backdrop: true,
                        keyboard: true,
                        show: true
                    });
                    return;
                }

                FreshiUpload.UploadFile(importType, "file_InputFile");
            });

            $("#bt_Search").on('click', function () {
                CreditManagement.CurrentUserType = CreditManagement.UserType.Admin;
                CreditManagement.SearchChannelList(1);
            });

            $("#bt_Search_CreditExchange").on('click', function () {
                CreditManagement.CurrentUserType = CreditManagement.UserType.Admin;
                CreditManagement.CreditExchangeApprovalList(1);
            });

            $("#bt_AreaUser").on('click', function () {
                CreditManagement.GetAreaUser(1);
            });

            var importType = [{ id: 1, text: "渠道商" }, { id: 2, text: "在网数据" }, { id: 3, text: "发展数据" }];
            
            $("#select_ImportType").select2({
                data: importType
            });
        }
    },
    Import: function (importType, fileName) {
        $("#div_ImportResult").empty();

        var ImportInfo = {
            ImportType: importType,
            FileName: fileName
        };
        FreshiCore.PostProcess(CreditManagement.OwnerClass, "Import", ImportInfo, $("#import"), CreditManagement.ImportCallback);
    },
    ImportCallback: function (data, status) {
        var trans = FreshiTransBox.CreateFrom(data);
        var content = trans.GetContent();
        if (!isNullOrUndefined(content)) {
            $("#div_ImportResult").append(CreditManagement.CreateImportResult(content.ImportResult));
        }
    },
    CreateImportResult: function (importResult) {
        var importType = $("#select_ImportType").select2("val");

        var thArray;

        if (importType == 1) {
            thArray = ["区县", "营服中心", "渠道代码1", "渠道名称", "渠道等级", "加入积分系统时间", "渠道建立时间", "渠道代码2", "状态"];
        } else if (importType == 2) {
            thArray = ["渠道代码", "亲情卡、长话卡", "导入结果", "2G单卡、无线公话", "导入结果", "3G/4G合约终端", "导入结果", "3G/4G单卡、宽带", "导入结果", "新增月份", "状态"];
        } else if (importType == 3) {
            thArray = ["渠道代码", "发展个数", "新增月份", "状态"];
        }

        var div1 = $("<div class='panel panel-default'></div>");
        var div2 = $("<div class='panel-heading'></div>");
        if (importType == 1) {
            div2.text("渠道商导入结果");
        } else if (importType == 2) {
            div2.text("在网数据导入结果");
        } else if (importType == 3) {
            div2.text("发展数据导入结果");
        }
        
        var div3 = $("<div class='panel-body table-responsive'></div>");

        var table = $("<table class='table table-bordered table-striped'></table>");
        var thead = $("<thead></thead");
        var thead_tr = $("<tr></tr>");
        thead_tr.addClass('table-header');

        $.each(thArray, function (index, item) {
            var th = $("<th></th>");
            th.text(item);
            th.appendTo(thead_tr);
        });

        thead_tr.appendTo(thead);
        thead.appendTo(table);      

        var tbody = $("<tbody></tbody>");
        $.each(importResult, function (index, item) {
            var tbody_tr = $("<tr></tr>");
            if (importType == 1) {                
                var td1 = $("<td></td>");
                td1.text(item.AreaCode);
                td1.appendTo(tbody_tr);
                var td2 = $("<td></td>");
                td2.text(item.AreaName);
                td2.appendTo(tbody_tr);
                var td3 = $("<td></td>");
                td3.text(item.ChannelCode);
                td3.appendTo(tbody_tr);
                var td4 = $("<td></td>");
                td4.text(item.ChannelName);
                td4.appendTo(tbody_tr);
                var td5 = $("<td></td>");
                td5.text(item.ChannelLevelDesc);
                td5.appendTo(tbody_tr);
                var td6 = $("<td></td>");
                td6.text(item.JoinYearAndMonth);
                td6.appendTo(tbody_tr);
                var td7 = $("<td></td>");
                td7.text(item.BuildYearAndMonth);
                td7.appendTo(tbody_tr);
                var td8 = $("<td></td>");
                td8.text(item.UserName);
                td8.appendTo(tbody_tr);
                var td9 = $("<td></td>");
                td9.text(item.ImportStatus);
                td9.appendTo(tbody_tr);
            } else if (importType == 2) {
                var td1 = $("<td></td>");
                td1.text(item.ChannelCode);
                td1.appendTo(tbody_tr);
                var td2 = $("<td></td>");
                td2.text(item.DevType1);
                td2.appendTo(tbody_tr);
                var td3 = $("<td></td>");
                td3.text(isNullOrUndefined(item.DevTypeStatus[0]) ? "" : item.DevTypeStatus[0]);
                td3.appendTo(tbody_tr);
                var td4 = $("<td></td>");
                td4.text(item.DevType2);
                td4.appendTo(tbody_tr);
                var td5 = $("<td></td>");
                td5.text(isNullOrUndefined(item.DevTypeStatus[1]) ? "" : item.DevTypeStatus[1]);
                td5.appendTo(tbody_tr);
                var td6 = $("<td></td>");
                td6.text(item.DevType3);
                td6.appendTo(tbody_tr);
                var td7 = $("<td></td>");
                td7.text(isNullOrUndefined(item.DevTypeStatus[2]) ? "" : item.DevTypeStatus[2]);
                td7.appendTo(tbody_tr);
                var td8 = $("<td></td>");
                td8.text(item.DevType4);
                td8.appendTo(tbody_tr);
                var td9 = $("<td></td>");
                td9.text(isNullOrUndefined(item.DevTypeStatus[3]) ? "" : item.DevTypeStatus[3]);
                td9.appendTo(tbody_tr);
                var td10 = $("<td></td>");
                td10.text(item.DevYearAndMonth);
                td10.appendTo(tbody_tr);
                var td11 = $("<td></td>");
                td11.text(item.ImportStatus);
                td11.appendTo(tbody_tr);
            } else if (importType == 3) {
                var td1 = $("<td></td>");
                td1.text(item.ChannelCode);
                td1.appendTo(tbody_tr);
                var td2 = $("<td></td>");
                td2.text(item.ContractCount);
                td2.appendTo(tbody_tr);
                var td3 = $("<td></td>");
                td3.text(item.ContractYearAndMonth);
                td3.appendTo(tbody_tr);
                var td4 = $("<td></td>");
                td4.text(item.ImportStatus);
                td4.appendTo(tbody_tr);                
            }
            tbody_tr.appendTo(tbody);
        });
        tbody.appendTo(table);

        table.appendTo(div3);

        div2.appendTo(div1);
        div3.appendTo(div1);

        return div1;
    },
    LoadAreaIndexPage: function () {
        if (CreditManagement.CheckUser(CreditManagement.UserType.Area)) {
            CreditManagement.CurrentUserType = CreditManagement.UserType.Area;
            CreditManagement.SearchChannelList(1);
        }
    },
    SearchChannelList: function (pageNumber) {
        $("#div_SearchChannel").empty();
        
        var SearchParams = {
            PageNumber: pageNumber,
            PageSize: 15
        };

        FreshiCore.PostProcess(CreditManagement.OwnerClass, "SearchChannelList", SearchParams, $("#channel"), CreditManagement.SearchChannelListCallback);
    },
    SearchChannelListCallback: function (data, status) {
        var trans = FreshiTransBox.CreateFrom(data);
        var content = trans.GetContent();
        if (!isNullOrUndefined(content)) {
            if (content.PageResult.length > 0) {

                var thArray = null;

                if (CreditManagement.CurrentUserType == CreditManagement.UserType.Area) {
                    thArray = ["区县", "营服中心", "渠道代码", "渠道名称", "渠道等级", "发展积分", "在网积分", "年限加分", "总积分", "已兑换积分", "当前总积分", "可兑换积分", ""];
                } else if (CreditManagement.CurrentUserType == CreditManagement.UserType.Admin) {
                    thArray = ["区县", "营服中心", "渠道代码", "渠道名称", "渠道等级", "发展积分", "在网积分", "年限加分", "总积分", "已兑换积分", "当前总积分"];
                }

                var table = $("<table class='table table-bordered table-striped'></table>");
                var thead = $("<thead></thead");
                var thead_tr = $("<tr></tr>");
                thead_tr.addClass('table-header');

                $.each(thArray, function (index, item) {
                    var th = $("<th></th>");
                    th.text(item);
                    th.appendTo(thead_tr);
                });

                thead_tr.appendTo(thead);
                thead.appendTo(table);

                var tbody = $("<tbody></tbody>");
                $.each(content.PageResult, function (index, item) {
                    var tbody_tr = $("<tr></tr>");

                    var td1 = $("<td></td>");
                    td1.text(item.AreaCode);
                    td1.appendTo(tbody_tr);
                    var td2 = $("<td></td>");
                    td2.text(item.AreaName);
                    td2.appendTo(tbody_tr);
                    var td3 = $("<td></td>");
                    td3.text(item.ChannelCode);
                    td3.appendTo(tbody_tr);
                    var td4 = $("<td></td>");
                    td4.text(item.ChannelName);
                    td4.appendTo(tbody_tr);
                    var td5 = $("<td></td>");
                    td5.text(item.ChannelLevelDesc);
                    td5.appendTo(tbody_tr);
                    var td6 = $("<td></td>");
                    td6.text(item.ChannelContractCredit);
                    td6.appendTo(tbody_tr);
                    var td7 = $("<td></td>");
                    td7.text(item.ChannelDevelopmentCredit);
                    td7.appendTo(tbody_tr);
                    var td8 = $("<td></td>");
                    td8.text(item.ChannelYearBonus);
                    td8.appendTo(tbody_tr);
                    var td9 = $("<td></td>");
                    td9.text(item.ChannelTotalAmount);
                    td9.appendTo(tbody_tr);
                    var td10 = $("<td></td>");
                    td10.text(item.ChannelExchangedCredit);
                    td10.appendTo(tbody_tr);
                    var td11 = $("<td></td>");
                    td11.text(item.ChannelRemainingTotalAmount);
                    td11.appendTo(tbody_tr);

                    if (CreditManagement.CurrentUserType == CreditManagement.UserType.Area) {
                        var td12 = $("<td></td>");
                        var td12TextBox = $("<input type='text' style='width: 80px;' class='form-control' id='tb_ExchangeableCredit_" + index + "'/>");
                        td12TextBox.val(item.ChannelExchangeableCredit);
                        if (item.ChannelExchangeableCredit == 0) {
                            td12TextBox.attr("readonly", true);
                        }
                        td12.append(td12TextBox);
                        td12.appendTo(tbody_tr);

                        var td13 = $("<td></td>");
                        var td13Button = $("<input type='button' id='btn_Exchange_" + index + "' value='兑换' style='width:60px;' class='btn btn-primary btn-md btn-block'>");
                        td13Button.on("click", CreditManagement.ExchageCredit);
                        if (item.ChannelExchangeableCredit == 0) {
                            td13Button.attr("disabled", "true");
                        }
                        td13.append(td13Button);
                        td13.appendTo(tbody_tr);
                    }                    

                    tbody_tr.data("ChannelInfo", item);

                    tbody_tr.appendTo(tbody);
                });

                tbody.appendTo(table);

                $("#div_SearchChannel").append(table);

                if (content.PageNumber == 1) {
                    CreditManagement.BindChannelListPaging(content.PageNumber, content.TotalPages);
                }
            }
        }
    },
    BindChannelListPaging: function (currentPage, totalPages) {
        var options = FreshiConfig.PagerDefaultOptions;
        options.currentPage = currentPage;
        options.totalPages = totalPages;
        options.onPageChangedCallback = CreditManagement.SearchChannelList;
                
        $("#ul_Pager").bootstrapPaginator(options);
    },
    ExchageCredit: function () {
        var tr = $(this).parent().parent();
        var channelInfo = tr.data("ChannelInfo");
        var textbox = tr.find("input[id^='tb_ExchangeableCredit_']");
        var button = tr.find("input[id^='btn_Exchange_']");

        var ExchangeInfo = {
            ChannelGuid: channelInfo.ChannelGUID,
            ExchangeCredit: textbox.val()
        };

        FreshiCore.PostProcess(CreditManagement.OwnerClass, "ExchangeCredit", ExchangeInfo, tr, function (data, status) {
            var trans = FreshiTransBox.CreateFrom(data);
            var content = trans.GetContent();
            if (!isNullOrUndefined(content)) {
                var channelInfo = content.ChannelInfo;
                textbox.val(channelInfo.ChannelExchangeableCredit);
                if (channelInfo.ChannelExchangeableCredit == 0) {
                    textbox.attr("readonly", true);
                    button.attr("disabled", true);
                }
            }
        });
    },
    CreditExchangeApprovalList: function (pageNumber) {
        $("#div_CreditExchangeApproval").empty();

        var SearchParams = {
            PageNumber: pageNumber,
            PageSize: 15
        };

        FreshiCore.PostProcess(CreditManagement.OwnerClass, "GetCreditExchangeApprovalList", SearchParams, $("#exchange"), CreditManagement.GetCreditExchangeApprovallListCallback);
    },
    GetCreditExchangeApprovallListCallback: function (data, status) {
        var trans = FreshiTransBox.CreateFrom(data);
        var content = trans.GetContent();
        if (!isNullOrUndefined(content)) {
            if (content.PageResult.length > 0) {

                var thArray = ["区县", "营服中心", "渠道代码", "渠道名称", "渠道等级", "发展积分", "在网积分", "年限加分", "总积分", "已兑换积分", "当前总积分", "申请兑换积分", "状态", ""];

                var table = $("<table class='table table-bordered table-striped'></table>");
                var thead = $("<thead></thead");
                var thead_tr = $("<tr></tr>");
                thead_tr.addClass('table-header');

                $.each(thArray, function (index, item) {
                    var th = $("<th></th>");
                    th.text(item);
                    th.appendTo(thead_tr);
                });

                thead_tr.appendTo(thead);
                thead.appendTo(table);

                var tbody = $("<tbody></tbody>");
                $.each(content.PageResult, function (index, item) {
                    var tbody_tr = $("<tr></tr>");

                    var td1 = $("<td></td>");
                    td1.text(item.AreaCode);
                    td1.appendTo(tbody_tr);
                    var td2 = $("<td></td>");
                    td2.text(item.AreaName);
                    td2.appendTo(tbody_tr);
                    var td3 = $("<td></td>");
                    td3.text(item.ChannelCode);
                    td3.appendTo(tbody_tr);
                    var td4 = $("<td></td>");
                    td4.text(item.ChannelName);
                    td4.appendTo(tbody_tr);
                    var td5 = $("<td></td>");
                    td5.text(item.ChannelLevelDesc);
                    td5.appendTo(tbody_tr);
                    var td6 = $("<td></td>");
                    td6.text(item.ChannelContractCredit);
                    td6.appendTo(tbody_tr);
                    var td7 = $("<td></td>");
                    td7.text(item.ChannelDevelopmentCredit);
                    td7.appendTo(tbody_tr);
                    var td8 = $("<td></td>");
                    td8.text(item.ChannelYearBonus);
                    td8.appendTo(tbody_tr);
                    var td9 = $("<td></td>");
                    td9.text(item.ChannelTotalAmount);
                    td9.appendTo(tbody_tr);
                    var td10 = $("<td></td>");
                    td10.text(item.ChannelExchangedCredit);
                    td10.appendTo(tbody_tr);
                    var td11 = $("<td></td>");
                    td11.text(item.ChannelRemainingTotalAmount);
                    td11.appendTo(tbody_tr);
                    var td12 = $("<td></td>");
                    if (item.Status == 0) {
                        td12.text(item.ExchangeCredit);
                    }

                    td12.appendTo(tbody_tr);

                    var td13 = $("<td></td>");
                    var td13Text = "";
                    switch (item.Status) {
                        case 0:
                            td13Text = "未审批";
                            break;
                        case 1:
                            td13Text = "通过";
                            break;
                        case -1:
                            td13Text = "拒绝";
                            break;
                    }
                    td13.text(td13Text);
                    td13.appendTo(tbody_tr);

                    var td14 = $("<td></td>");
                    if (item.Status == 0) {
                        var td14ApprovalButton = $("<input type='button' id='btn_Approval_" + index + "' value='通过' style='width:60px;' class='btn btn-primary btn-md btn-block'>");
                        td14ApprovalButton.on("click",  function () {
                            var tr = $(this).parent().parent();
                            CreditManagement.ApprovalExchageCredit(tr, 1);
                        });
                        td14.append(td14ApprovalButton);
                        var td14RejectButton = $("<input type='button' id='btn_Reject_" + index + "' value='拒绝' style='width:60px;' class='btn btn-primary btn-md btn-block'>");
                        td14RejectButton.on("click", function () {
                            var tr = $(this).parent().parent();
                            CreditManagement.ApprovalExchageCredit(tr, -1);
                        });
                        td14.append(td14RejectButton);
                    }
                    td14.appendTo(tbody_tr);

                    tbody_tr.data("ChannelCreditExchangeInfo", item);

                    tbody_tr.appendTo(tbody);
                });

                tbody.appendTo(table);

                $("#div_CreditExchangeApproval").append(table);

                if (content.PageNumber == 1) {
                    CreditManagement.BindCreditExchangeApprovalListPaging(content.PageNumber, content.TotalPages);
                }
            }
        }
    },
    BindCreditExchangeApprovalListPaging: function (currentPage, totalPages) {
        var options = FreshiConfig.PagerDefaultOptions;
        options.currentPage = currentPage;
        options.totalPages = totalPages;
        options.onPageChangedCallback = CreditManagement.CreditExchangeApprovalList;

        $("#ul_Pager_CreditExchangeApproval").bootstrapPaginator(options);
    },
    ApprovalExchageCredit: function (tr,approvalStatus) {
        var channelCreditExchangeInfo = tr.data("ChannelCreditExchangeInfo");

        var ApprovalInfo = {
            ExchangeId: channelCreditExchangeInfo.ExchangeId,
            ApprovalStatus: approvalStatus
        };

        FreshiCore.PostProcess(CreditManagement.OwnerClass, "ApprovalExchangeCredit", ApprovalInfo, tr, function (data, status) {
            var trans = FreshiTransBox.CreateFrom(data);
            var content = trans.GetContent();
            if (!isNullOrUndefined(content)) {
                var channelInfo = content.ChannelInfo;
                tr.find("td").eq(9).text(channelInfo.ChannelExchangedCredit);
                tr.find("td").eq(10).text(channelInfo.ChannelRemainingTotalAmount);
                tr.find("td").eq(11).text("");
                tr.find("td").eq(12).text(approvalStatus == 1 ? "通过" : "拒绝");
                tr.find("td").eq(13).html("");
            }
        });
    },
    GetAreaUser: function (pageNumber) {
        $("#div_AreaUser").empty();

        var SearchParams = {
            PageNumber: pageNumber,
            PageSize: 15
        };

        FreshiCore.PostProcess(CreditManagement.OwnerClass, "GetAreaUser", SearchParams, $("#areaUser"), CreditManagement.GetAreaUserCallback);
    },
    GetAreaUserCallback: function (data, status) {
        var trans = FreshiTransBox.CreateFrom(data);
        var content = trans.GetContent();
        if (!isNullOrUndefined(content)) {
            if (content.PageResult.length > 0) {

                var thArray = ["区县", "用户名"];

                var table = $("<table class='table table-bordered table-striped'></table>");
                var thead = $("<thead></thead");
                var thead_tr = $("<tr></tr>");
                thead_tr.addClass('table-header');

                $.each(thArray, function (index, item) {
                    var th = $("<th></th>");
                    th.text(item);
                    th.appendTo(thead_tr);
                });

                thead_tr.appendTo(thead);
                thead.appendTo(table);

                var tbody = $("<tbody></tbody>");
                $.each(content.PageResult, function (index, item) {
                    var tbody_tr = $("<tr></tr>");

                    var td1 = $("<td></td>");
                    td1.text(item.AreaCode);
                    td1.appendTo(tbody_tr);
                    var td2 = $("<td></td>");
                    td2.text(item.UserName);
                    td2.appendTo(tbody_tr);                                  

                    tbody_tr.data("AreaUserInfo", item);

                    tbody_tr.appendTo(tbody);
                });

                tbody.appendTo(table);

                $("#div_AreaUser").append(table);

                if (content.PageNumber == 1) {
                    CreditManagement.BindAreaUserPaging(content.PageNumber, content.TotalPages);
                }
            }
        }
    },
    BindAreaUserPaging: function (currentPage, totalPages) {
        var options = FreshiConfig.PagerDefaultOptions;
        options.currentPage = currentPage;
        options.totalPages = totalPages;
        options.onPageChangedCallback = CreditManagement.GetAreaUser;

        $("#ul_Pager_AreaUser").bootstrapPaginator(options);
    },

})