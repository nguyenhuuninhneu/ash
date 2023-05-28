var topic = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};
           
        },
        loadData: function (pageindex,id) {
            var self = this;
            self.pageIndex = pageindex;
            if (this.FirstLoad) {
                $("#loading").show();
            }
            debugger
            var Status = "";
            if (id == null || id == undefined) {
                Status = $("#Status").val();
            }
            else {
                Status = id;
            }
            var position = $('#position').val();
            var sourcetopic = $('#sourcetopic').val();
            var author = $('#author').val();
            var tag = $('#tag').val();
            var Title = $("#Title").val();
            var CreatedBy = $("#CreatedBy").val();
            var CategoryId = $("#CategoryId").val();
            var NgayDang = $("#NgayDang").val();
            var NgayKet = $("#NgayKet").val();
            var Send = $("#Send").val();
            var Return = $("#Return").val();
            topic.DataSearch = { Title: Title, Tag: tag, SourceTopic: sourcetopic, Author: author, Status: Status, Position: position, CreatedBy: CreatedBy, CategoryId: CategoryId, NgayDang: NgayDang, NgayKet: NgayKet, Send: Send, Return: Return }
            AjaxService.POST("/Admin/topic/ListData?page=" + self.pageIndex, topic.DataSearch, function (res) {
                $("#gridData").html(res.viewContent);
                $('#paginationholder').html('');
                if (res.totalPages > 1) {
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            topic.loadData(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadDataSent: function (pageindex) {
            var self = this;
            self.pageIndex = pageindex;
            if (this.FirstLoad) {
                $("#loading").show();
            }
            var Title = $("#Title").val();
            var CreatedBy = $("#CreatedBy").val();
            var CategoryId = $("#CategoryId").val();
            var Status = $("#Status").val();
            var NgayDang = $("#NgayDang").val();
            var NgayKet = $("#NgayKet").val();
            topic.DataSearch = { Title: Title, Status: Status, CreatedBy: CreatedBy, CategoryId: CategoryId, NgayDang: NgayDang, NgayKet: NgayKet }
            AjaxService.POST("/Admin/topic/ListDataSent?page=" + self.pageIndex, topic.DataSearch, function (res) {
                $("#gridDataSent").html(res.viewContent);
                $('#paginationholder').html('');
                if (res.totalPages > 1) {
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            topic.loadDataSent(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadfrmDetail: function (id) {
            modal.Render("/Admin/topic/Detail/" + id, "Chi tiết bài viết", "modal-lg");
        },
        onSearchSuccess: function (res) {
            var self = this;
            $('#paginationholder').html('');
            $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
            $("#gridData").html(res.viewContent);
            topic.DataSearch = {
                Title: $("#Title").val(),
                Author: $("#Author").val(),
                DisplayZone: $("#DisplayZone").val(),
                LangCode: $("#LangCode").val(),
                PageElementId: $("#PageElementId").val(),
                Status: $("#Status").val(),
                CreatedBy: $("#CreatedBy").val(),
                CategoryId: $("#CategoryId").val(),
                NgayDang: $("#NgayDang").val(),
                NgayKet: $("#NgayKet").val()
            };
            if (res.totalPages > 1) {
                $('#pagination').twbsPagination({
                    startPage: 1,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        topic.pageIndex = page;
                        topic.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        },
        ondelete: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có muốn xóa tin này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/topic/Delete", { id: id }, function (res) {
                        if (res.IsSuccess == true) {
                            self.pageIndex = 1;
                            self.loadData(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        }
                        else {
                            alertmsg.error(res.Messenger);
                        }
                    });
                } 
                $("#loading").hide();
            });
        },
        onmultidelete: function () {
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một tin bài cần xóa");
            } else {
                $("#loading").show();
                swal({
                    title: "Bạn có chắc chắn không?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Có",
                    cancelButtonText: "không",
                }, function (isConfirm) {
                    if (isConfirm) {
                        AjaxService.POST("/Admin/topic/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            if (res.IsSuccess == true) {
                                self.pageIndex = 1;
                                self.loadData(self.pageIndex);
                                alertmsg.success(res.Messenger);
                            }
                            else {
                                alertmsg.error(res.Messenger);
                            }
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
        loadCatbyLang: function (langcode) {
            AjaxService.GET("/Admin/Category/GetCatbyLang", { lang: langcode }, function (res) {
                var html = "";
                for (var i = 0; i < res.length; i++) {
                    html += "<option value='" + res[i].ID + "'>" + Array(res[i].Level).join("--") + res[i].Name + "</option>";
                }
                $("#CategoryId").html(html);
            });
        },
        loadBoxTopicConfig: function (_catid, zoneselected) {
            var arrzone = [];
            if (zoneselected != undefined) {
                arrzone = zoneselected.split(',');
            }
            AjaxService.GET("/Admin/BoxTopicConfig/GetListConfigByCatId", { catid: _catid }, function (res) {
                var html = "";
                for (var i = 0; i < res.length; i++) {
                    var chk = jQuery.inArray(res[i].Code, arrzone) != -1 ? "checked" : "";
                    html += '<div class="checkbox checkbox-success checkbox-inline">\
                                <input type="checkbox" ' + chk + ' id="' + res[i].Code + '" name="DisplayZone" value="' + res[i].Code + '">\
                                <label for="' + res[i].Code + '"> ' + res[i].Name + ' </label>\
                            </div>';
                }
                $("#boxtopicconfig").html(html);
            });
        },
        onApproved: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có chắc chắn duyệt tin này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/Topic/Approved", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        onApprovedDown: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Hủy duyệt tin này?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/Topic/ApprovedDown", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        getTopicSuccess: function (res) {
            $('#modalgettopic').modal('hide');
            topic.loadData(1);
            alertmsg.success("Lấy tin bài thành công !");
        },
        changePublic: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Thay đổi trạng thái xuất bản?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/Topic/ChangePublic", { id: id}, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        active: function (id, str) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Thay đổi trạng thái?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/Topic/ChangeStatus", { id: id, str: str }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
    };
}();
$(function() {
    topic.init();
    var count = $("#tongbanghi").val();
    $('#countrecord').html(count);
});


