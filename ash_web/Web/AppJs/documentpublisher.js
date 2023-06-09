﻿var DocumentPublisher = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};
            $("#Name").change(function () {
                $("#btnSearch").click();
            });
            $("#LangCode").change(function () {
                $("#btnSearch").click();
            });
        },
        loadData: function (pageindex) {
            debugger;
            var self = this;
            $("#loading").show();
            var title = $("#NameSeach").val();
            var Status = $("#Status").val();
            var LangCode = $("#LangCode").val();
            var CreatedDate = $("#CreatedDate").val();
            var KyHieu = $("#KyHieu").val();
            var MaSo = $("#MaSo").val();
            DocumentPublisher.DataSearch = { Name: title, Status: Status, CreatedDate: CreatedDate, LangCode: LangCode, KyHieu: KyHieu, MaSo: MaSo };
            AjaxService.POST("/Admin/DocumentPublisher/ListData?page=" + pageindex, DocumentPublisher.DataSearch, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        onupdateposittion: function () {
            var self = this;
            $("#loading").show();
            var arrValue = [];
            $("table tbody tr").each(function () {
                var id = $(this).find("#item_ID").val();
                var ordering = $(this).find("input[type=number]").val();
                if (ordering == "") ordering = 0
                var str = id + ":" + ordering;
                arrValue.push(str);
            });
            var strValue = arrValue.join("|");
            AjaxService.POST("/DocumentPublisher/UpdatePosition", { value: strValue }, function (res) {
                debugger;
                if (res.IsSuccess == true) {
                    alertmsg.success(res.Messenger);
                    $("#gridData").html(res.ViewContent);
                } else if (res=="error") {
                    alertmsg.error("Không có dữ liệu. Cập nhật thất bại");
                }
                else {
                    alertmsg.error(res.Messenger);
                }
                self.loadData(self.pageIndex);
                //$("#loading").hide();
            });
        },
        oneditStatus: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Thay đổi trạng thái?",
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
                    AjaxService.POST("/DocumentPublisher/EditStatus", { id: id }, function (res) {
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
        loadfrmAdd: function () {
            modal.Render("/Admin/DocumentPublisher/Add", "Thêm mới tài liệu", "modal-lg");

        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                DocumentPublisher.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadfrmedit: function (id) {
            modal.Render("/Admin/DocumentPublisher/Edit/" + id, "Cập nhật tài liệu", "modal-lg");
        },
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                DocumentPublisher.loadData(this.pageIndex);
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
                title: "Bạn có muốn xóa tài liệu này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Đồng ý",
                cancelButtonText: "Hủy",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/DocumentPublisher/Delete", { id: id }, function (res) {
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
                alertmsg.error("Bạn cần chọn ít nhất một tài liệu cần xóa");
            } else {
                $("#loading").show();
                swal({
                    title: "Bạn có chắc chắn không?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Đồng ý",
                    cancelButtonText: "Hủy",
                }, function (isConfirm) {
                    if (isConfirm) {
                        AjaxService.POST("/Admin/DocumentPublisher/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadData(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },

    };
}();
$(function () { DocumentPublisher.init(); });