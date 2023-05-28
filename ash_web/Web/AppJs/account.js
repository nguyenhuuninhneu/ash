var account = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};
        },
        loadData: function (pageindex) {
            var self = this;
          
            if (this.FirstLoad) {
                $("#loading").show();
            }
            var Name = $("#Name").val();
            var IsCTV = $("#IsCTV").val();
            account.DataSearch = { Name: Name, IsCTV: IsCTV };
            AjaxService.POST("/Admin/Account/ListData?page=" + pageindex, account.DataSearch, function (res) {
                $("#gridData").html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('');
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            account.pageIndex = page;
                            account.loadData(page);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        oneditStatus: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Thay đổi trạng thái công tác viên?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Đồng ý",
                cancelButtonText: "Hủy",
                closeOnConfirm: false,
                closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    swal("OK!", "", "success");
                    AjaxService.POST("/Admin/Account/EditStatusCTV", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                } else {
                    swal("Cancelled", "", "error");
                }
                $("#loading").hide();
            });
        },
        onSearchSuccess: function (res) {
            $('#paginationholder').html('');
            $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
            $("#gridData").html(res.viewContent);
            account.DataSearch = {
                Name: $("#Name").val(),
                IsCTV: $("#IsCTV").val()
            };
            if (res.totalPages > 1) {
                $('#pagination').twbsPagination({
                    startPage: account.pageIndex,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        account.pageIndex = page;
                        account.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
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
                    AjaxService.POST("/Admin/Account/ChangeStatus", { id: id, str: str }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        loadfrmAdd: function (IsCTV) {
            debugger;
            modal.Render("/Admin/Account/Add?IsCTV=" + IsCTV , "Thêm mới người dùng", "modal-lg");
        },
        loadfrmInfo: function (id) {
            modal.Render("/Admin/Account/UserInfo/" + id, "Thông tin tác giả", "modal-lg");
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                account.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadfrmEdit: function (id) {
            modal.Render("/Admin/Account/Edit/" + id, "Cập nhật người dùng", "modal-lg");
        },
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                account.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadfrmReset: function(id) {
            modal.Render("/Admin/Account/ResetPassword/" + id, "Reset mật khẩu", "modal-md");
        },
        onResetSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                account.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadfrmChangePass: function (id) {
            modal.Render("/Admin/Account/ChangePass/" + id, "Đổi mật khẩu", "modal-md");
        },
        loadfrmChangeInfo: function (id) {
            modal.Render("/Admin/Account/ChangeInfo/" + id, "Cập nhật thông tin cá nhân", "modal-md");
        },
        loadfrmViewInfo: function (id) {
            modal.Render("/Admin/Account/ViewInfo/" + id, "Xem thông tin", "modal-md");
        },
        loadExportExcel: function (IsCTV) {
            debugger;
            var IsCTV = $('#IsCTV').val();
            $.ajax({
                url: '/Admin/Account/ExportExcel',
                type: 'POST',
                data: { IsCTV: IsCTV},
                success: function (res) {

                }
            });
        },
        onChangePassSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger); 
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        ondelete: function (id) {
            $("#loading").show();
            var self = this;
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
                    AjaxService.POST("/Admin/Account/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        onmultidelete: function () {
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một người dùng cần xóa");
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
                        AjaxService.POST("/Admin/Account/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadData(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
        initcheckall: function () {
            var countall = $(".chkelement").length;
            var countchecked = 0;
            $(".chkelement").each(function () {
                if ($(this).find($("input[type=checkbox]")).is(':checked')) {
                    countchecked++;
                }
            });
            if (countall == countchecked) {
                $("#basicForm #chkall").prop('checked', true);
            } else {
                $("#basicForm #chkall").prop('checked', false);
            }
            $("#basicForm #chkall").click(function () {
                if ($(this).is(':checked')) {
                    $(".chkelement").each(function () {
                        $(this).find($("input[type=checkbox]")).prop('checked', true);
                    });
                } else {
                    $(".chkelement").each(function () {
                        $(this).find($("input[type=checkbox]")).prop('checked', false);
                    });
                }
            });
            $(".chkelement").click(function () {
                countchecked = 0;
                $(".chkelement").each(function () {
                    if ($(this).find($("input[type=checkbox]")).is(':checked')) {
                        countchecked++;
                    }
                });
                if (countall == countchecked) {
                    $("#basicForm #chkall").prop('checked', true);
                } else {
                    $("#basicForm #chkall").prop('checked', false);
                }
            });
        }, 
    };
}();
$(function () { account.init(); });


