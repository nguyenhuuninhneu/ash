var magazine = function () {
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
            var sourcemagazine = $('#sourcemagazine').val();
            var author = $('#author').val();
            var tag = $('#tag').val();
            var Title = $("#Title").val();
            var CreatedBy = $("#CreatedBy").val();
            var CategoryId = $("#CategoryId").val();
            var NgayDang = $("#NgayDang").val();
            var NgayKet = $("#NgayKet").val();
            var Send = $("#Send").val();
            var Nam = $("#Nam").val();
            var Return = $("#Return").val();
            magazine.DataSearch = { Title: Title, Tag: tag, SourceMagazine: sourcemagazine, Author: author, Status: Status, Position: position, CreatedBy: CreatedBy, CategoryId: CategoryId, NgayDang: NgayDang, NgayKet: NgayKet, Send: Send, Return: Return, Nam: Nam }
            AjaxService.POST("/Admin/magazine/ListData?page=" + self.pageIndex, magazine.DataSearch, function (res) {
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
                            magazine.loadData(self.pageIndex);
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
            magazine.DataSearch = { Title: Title, Status: Status, CreatedBy: CreatedBy, CategoryId: CategoryId, NgayDang: NgayDang, NgayKet: NgayKet }
            AjaxService.POST("/Admin/magazine/ListDataSent?page=" + self.pageIndex, magazine.DataSearch, function (res) {
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
                            magazine.loadDataSent(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadfrmDetail: function (id) {
            modal.Render("/Admin/magazine/Detail/" + id, "Chi tiết tạp chí", "modal-lg");
        },
        onSearchSuccess: function (res) {
            var self = this;
            $('#paginationholder').html('');
            $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
            $("#gridData").html(res.viewContent);
            magazine.DataSearch = {
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
                        magazine.pageIndex = page;
                        magazine.loadData(page);
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
                title: "Bạn có muốn xóa tạp chí này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/magazine/Delete", { id: id }, function (res) {
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
                alertmsg.error("Bạn cần chọn ít nhất một tạp chí cần xóa");
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
                        AjaxService.POST("/Admin/magazine/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
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
        loadBoxMagazineConfig: function (_catid, zoneselected) {
            var arrzone = [];
            if (zoneselected != undefined) {
                arrzone = zoneselected.split(',');
            }
            AjaxService.GET("/Admin/BoxMagazineConfig/GetListConfigByCatId", { catid: _catid }, function (res) {
                var html = "";
                for (var i = 0; i < res.length; i++) {
                    var chk = jQuery.inArray(res[i].Code, arrzone) != -1 ? "checked" : "";
                    html += '<div class="checkbox checkbox-success checkbox-inline">\
                                <input type="checkbox" ' + chk + ' id="' + res[i].Code + '" name="DisplayZone" value="' + res[i].Code + '">\
                                <label for="' + res[i].Code + '"> ' + res[i].Name + ' </label>\
                            </div>';
                }
                $("#boxmagazineconfig").html(html);
            });
        },
        onApproved: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có chắc chắn duyệt tạp chí này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/Magazine/Approved", { id: id }, function (res) {
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
                title: "Hủy duyệt tạp chí này?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/Magazine/ApprovedDown", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        getMagazineSuccess: function (res) {
            $('#modalgetmagazine').modal('hide');
            magazine.loadData(1);
            alertmsg.success("Lấy tạp chí thành công !");
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
                    AjaxService.POST("/Admin/Magazine/ChangePublic", { id: id}, function (res) {
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
                    AjaxService.POST("/Admin/Magazine/ChangeStatus", { id: id, str: str }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        onupdateposittion: function () {
            var self = this;
            $("#loading").show();
            var arrValue = [];
            $("table tbody tr").each(function () {
                var id = $(this).find("#item_ID").val();
                var ordering = $(this).find(".position").val();
             
                var str = id + ":" + ordering;
                arrValue.push(str);
            });
            var strValue = arrValue.join("|");
            AjaxService.POST("/Admin/Magazine/UpdatePosition", { value: strValue }, function (res) {
                if (res.IsSuccess == true) {
                    alertmsg.success(res.Messenger);
                    $("#gridData").html(res.ViewContent);
                } else {
                    alertmsg.error(res.Messenger);
                }
                magazine.loadData(self.pageIndex);
                $("#loading").hide();
            });
        },
    };
}();
$(function() {
    magazine.init();
    var count = $("#tongbanghi").val();
    $('#countrecord').html(count);
});


