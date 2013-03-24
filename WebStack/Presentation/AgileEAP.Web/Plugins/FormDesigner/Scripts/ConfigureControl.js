(function (window, undefined) {
    configure = {
        getDataSource: function (options) {
            var tableName = "";
            var id = options.ID;
            var form = options.form ? options.form : null;
            if (form && form.DataSource)
                tableName = form.DataSource;
            tableName = options.tableName ? options.tableName : tableName;
            if ($("#TableName").length > 0) {
                $("#TableName").remove();
            }
            $.post("/FormDesigner/Home/GetTable", { TableName: tableName }, function (value) {
                var retValue = value;
                var availableTags = [
                ];
                var tableContentItem = "<ul id='TableName'>";
                if (retValue.length > 1) {
                    for (var i = 0; i < retValue.length; i++) {
                        var tableInfo = retValue[i];
                        var tableItem = "<li><a href='#'>" + tableInfo.TableName + "</a>";
                        var cloumnsItem = "";
                        if (tableInfo.Columns && tableInfo.Columns.length > 0) {
                            cloumnsItem = "<ul>";
                            for (var j = 0; j < tableInfo.Columns.length; j++) {
                                cloumnsItem = cloumnsItem + "<li><a href='#'>" + tableInfo.Columns[j] + "</a></li>";
                                //availableTags.push(tableInfo.Columns[j] + "[" + tableInfo.TableName + "]");
                                availableTags.push(tableInfo.TableName +"-"+tableInfo.Columns[j]);
                            }
                            cloumnsItem = cloumnsItem + "</ul>";
                        }
                        tableContentItem = tableContentItem + tableItem + cloumnsItem + "</li>";
                    }
                }
                else if (retValue.length == 1) {
                    var tableInfo = retValue[0];
                    var cloumnsItem = "";
                    if (tableInfo.Columns && tableInfo.Columns.length > 0) {
                        cloumnsItem = "";
                        for (var j = 0; j < tableInfo.Columns.length; j++) {
                            cloumnsItem = cloumnsItem + "<li><a href='#'>" + tableInfo.Columns[j] + "</a></li>";
                            availableTags.push(tableInfo.Columns[j]);
                        }
                        cloumnsItem = cloumnsItem;
                    }
                    tableContentItem = tableContentItem + cloumnsItem
                }
                tableContentItem = tableContentItem + "</ul>";
                $("#" + id).parent().css("position", "relative");
                $("#" + id).after(tableContentItem);
                $("#" + id).autocomplete({
                    source: availableTags,
                    open: function (event, ui) {
                        if ($("#TableName").length > 0) {
                            $("#TableName").hide();
                        }
                    },
                    close: function (event, ui) {
                        if ($("#TableName").length > 0) {
                            $("#TableName").show();
                        }
                    },
                    select: function (event, ui) {
                        if (ui.item.value.indexOf("-") > 0) {
                            ui.item.value = ui.item.value; //ui.item.value.substring(0, ui.item.value.indexOf("["));
                        }
                    }
                });
                $("#TableName").menu();
                selectmenuse = 0;
                $(document).bind("click", function () {
                    if (selectmenuse !== 2) {
                        $("#TableName").menu("destroy");
                        $("#TableName").remove();
                    }
                });
                $("#TableName").addClass("TableInfo");
                $(".ui-menu-item").css("width", "210px");
                $("#TableName").on("menuselect", function (event, ui) {
                    if (ui.item[0].children.length == 1) {
                        if (ui.item.parent().parent().find(".ui-menu-icon").length > 0) {
                            $("#" + id).val(ui.item.parent().parent().find("a:first").text() + "-" + ui.item.find("a").text());
                        }
                        else {
                            $("#" + id).val(ui.item.find("a").text());
                        }
                        selectmenuse = 1;
                    }
                    else {
                        selectmenuse = 2;
                    }
                });
            });
        },

        getTreeDataSource: function (options, callback) {
            var defaults =
                {
                    id: "control_datasource",
                    dataSource: null,
                    checkedItems: null
                }
            var treeContent = $.extend({}, defaults, options);
            if ($.isFunction(callback)) {
                var retValue = $("#" + treeContent.id).jstree("get_checked");
                var retItems = '',parentItems;
                if (retValue && retValue.length > 0) {
                    for (var i = 0; i < retValue.length; i++) {
                        if (retItems === "") {
                            retItems = $(retValue[i]).attr("id");
                            if ($(retValue[i]).parent() && $(retValue[i]).parent().parent().length > 0) {
                                parentItems = $(retValue[i]).parent().parent().attr("id");
                            }
                        }
                        else {
                            retItems = retItems + "," + $(retValue[i]).attr("id");
                            if ($(retValue[i]).parent() && $(retValue[i]).parent().parent().length > 0) {
                                parentItems = parentItems + "," + $(retValue[i]).parent().parent().attr("id");
                            }
                        }
                    }
                }
                callback.call(this, retValue, retItems,parentItems);
            }
            else {
                $.ajaxSetup({ cache: false });
                $("#" + treeContent.id).bind("loaded.jstree", function (e, data) {//树形加载前判断哪个节点被选中  
                    $(this).jstree("open_all");
                    $(this).find("li").each(function () {
                        if (typeof treeContent.checkedItems == "string") {
                            treeContent.checkedItems = treeContent.checkedItems.split(",");
                        }
                        if ($.isArray(treeContent.checkedItems)) {
                            for (var i = 0; i < treeContent.checkedItems.length; i++) {
                                if ($(this).attr("id") == treeContent.checkedItems[i]) {
                                    $("#" + treeContent.id).jstree("check_node", $(this));
                                    //<span style="color: #808000;">  //$("#modeltree").jstree("check_node",$(this));//jstree默认方法，如果默认的是父节点，那么子节点也会被选中</span>  
                                    //  <span style="color: #ff6600;">//自定义check_node方法  
                                    //  $("#modeltree").jstree("check_node",function(){  
                                    //      alert($("#"+obj.nodeid).attr("class"));  
                                    //      $("#"+obj.nodeid).removeClass("jstree-unchecked jstree-undetermined").addClass("jstree-checked");  
                                    //  });</span>  
                                }
                            }
                        }
                    });
                }).jstree({
                    "plugins": ["themes", "json_data", "checkbox", "sort", "ui"],
                    "themes": {
                        "theme": "default",
                        "url": "/Plugins/Resources/Content/Themes/Default/jstree/themes/default/style.css",
                        "dots": true,
                        "icons": false
                    },
                    "lang": {
                        "loading": "目录加载中……"
                    },
                    "json_data": {
                        "ajax": {
                            "url": "/FormDesigner/Home/GetDataSorceTree",
                            //"data": { "tableName":null}// treeContent.dataSource }
                        },
                        "progressive_render": true,
                        "metadata": "a string, array, object, etc"
                    },
                    "checkbox": {
                        //"two_state": true
                    }
                }).bind("click.jstree", function (event) {
                    var eventNodeName = event.target.text;
                    // $("#" + treeContent.id).jstree("check_node", $(event.target).parent());
                    //alert(eventNodeName);
                    // var a= $("#" + treeContent.id).jstree("get_checked");
                });
            }
        },

        loadCSS: function (stylePath) {
            var addStyle = document.createElement("link");
            addStyle.rel = "stylesheet";
            addStyle.type = "text/css";
            addStyle.href = stylePath;
            document.getElementsByTagName("head")[0].appendChild(addStyle);
        },
        loadScript: function (src) {
            var endScript = document.createElement("script");
            endScript.type = "text/javascript";
            endScript.src = src;
            document.getElementsByTagName('head')[0].appendChild(endScript);
        }
    }
})(window);