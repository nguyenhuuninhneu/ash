var ThongBao = function () {
        return {
            init: function () {
                this.pageIndex = 1;
                this.FirstLoad = true;
                this.DataSearch = {};
            },
            loadData: function (pageindex) {
                debugger;
                var self = this;
                if (this.FirstLoad) {
                    $("#loading").show();
                }
                AjaxService.POST("/ThongBao/ListData?page=" + pageindex, ThongBao.DataSearch, function (res) {
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
                                ThongBao.loadData(self.pageIndex);
                            }
                        });
                    }
                    self.FirstLoad = false;
                    $("#loading").hide();
                });
            },
           
            activeList: function (id,type) {
                debugger;
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
                        AjaxService.POST("/Admin/ThongBao/ChangeStatus", { id: id, type: type }, function (res) {
                            debugger;
                            ThongBao.loadData(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            },
            onSearchSuccess: function (res) {
                debugger;
                var self = this;
                $('#paginationholder').html('');
                $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                $("#gridData").html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#pagination').twbsPagination({
                        startPage: 1,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            ThongBao.pageIndex = page;
                            ThongBao.loadData(page);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            },
    };
}();
$(function () { ThongBao.init(); });
