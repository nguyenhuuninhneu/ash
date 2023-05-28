var boxnewsconfig = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};
            $("#LangCode").change(function () {
                $("#btnSearch").click();
            });
            $("#PageElementId").change(function () {
                $("#btnSearch").click();
            });
        },
        loadData: function (pageindex) {
            var self = this;
            $("#loading").show();
            AjaxService.POST("/BoxNewsConfig/ListData?page=" + pageindex, boxnewsconfig.DataSearch, function (res) {
                $("#gridData").html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('');
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            boxnewsconfig.loadData(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        onSearchSuccess: function (res) {
            var self = this;
            $('#paginationholder').html('');
            $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
            $("#gridData").html(res.viewContent);
            boxnewsconfig.DataSearch = {
                LangCode: $("#LangCode").val(),
                PageElementId: $("#PageElementId").val()
            };
            if (res.totalPages > 1) {
                $('#pagination').twbsPagination({
                    startPage: 1,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        boxnewsconfig.pageIndex = page;
                        boxnewsconfig.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        },
        loadfrmAdd: function () {
            modal.Render("/BoxNewsConfig/Add", "Thêm mới zone hiển thị", "modal-lg");
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                boxnewsconfig.loadData();
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadfrmedit: function (Code, PageElementId, LangCode) {
            modal.Render("/BoxNewsConfig/Edit?Code=" + Code + "&PageElementId=" + PageElementId + "&LangCode=" + LangCode, "Cập nhật zone hiển thị", "modal-lg");
        },
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                boxnewsconfig.loadData();
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        ondelete: function (Code, PageElementId, LangCode) {
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
                    AjaxService.POST("/BoxNewsConfig/Delete", { Code: Code, PageElementId: PageElementId, LangCode: LangCode }, function (res) {
                        self.loadData();
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        }
    };
}();
$(function () { boxnewsconfig.init(); });


