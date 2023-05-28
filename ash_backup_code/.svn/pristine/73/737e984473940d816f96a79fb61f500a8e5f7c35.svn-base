var newspaper = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.loadData(this.pageIndex);
        },
        loadData: function (pageIndex) {
            var self = this;
            if (this.FirstLoad) {
                $("#loading").show();
            }
            AjaxService.POST("/newspaper/ListData", null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        onSearchSuccess: function (data) {
            var currentParent = $(".cParent").val();
            $("#gridData").html(data.viewContent);
        },
        loadFrmAdd: function () {
            modal.Render("/newspaper/Add", "Thêm mới bài viết", "modal-lg");
        },
        loadFrmCmt: function (id) {
            AjaxService.POST("/newspaper/ListDataCmt/" + id, null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadDetailCmt: function (id) {
            modal.Render("/newspaper/DetailsCmt/"+id, "Nội dung đóng góp ý kiến", "modal-lg");
        },
        loadDataReply: function (id) {
            debugger;
            AjaxService.POST("/newspaper/ListDataCmt/" + id, null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });

        },
        onSearchRep: function (res) {
            debugger;
            var self = this;
            $('#paginationholder').html('');
            if (res == undefined) {
                $("#gridData").html("");
            }
            else {
                $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                $("#gridData").html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#pagination').twbsPagination({
                        startPage: 1,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            newspaper.pageIndex = page;
                            newspaper.loadDataReply(res.cmtid);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            }
        },
        ondeleteCmt: function (id,pid) {
            var currentParent = $(".cParent").val();
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có chắc chắn không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "Không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/newspaper/DeleteCmt", { id: id }, function (res) {
                        self.pageIndex = 1;
                        //displaymenu();
                        self.loadFrmCmt(pid);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        active: function (id,str) {
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
                    AjaxService.POST("/newspaper/ChangeStatus", { id: id , str: str}, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                newspaper.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadFrmEdit: function (id) {
            modal.Render("/newspaper/Edit/" + id, "Cập nhật bài viết", "modal-lg");
        },
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                newspaper.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        ondelete: function (id) {
            var currentParent = $(".cParent").val();
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có chắc chắn không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "Không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/newspaper/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        //displaymenu();
                        self.loadData(self.pageIndex, currentParent);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        onmultidelete: function () {
            var a = $('#checkall').val();
            var currentParent = $(".cParent").val();
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một bài viết cần xóa");
            } else {
                $("#loading").show();
                swal({
                    title: "Bạn có chắc chắn không?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Có",
                    cancelButtonText: "Không",
                }, function (isConfirm) {
                    if (isConfirm) {
                        AjaxService.POST("/newspaper/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadData(self.pageIndex, currentParent);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
    };
}();
$(function () { newspaper.init(); });


