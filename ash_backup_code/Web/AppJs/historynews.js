var historynews = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};
        },
        loadData: function (pageindex) {
            var self = this;
            $("#loading").show();
            AjaxService.POST("/Admin/historynews/ListData?page=" + pageindex, historynews.DataSearch, function (res) {
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
                            historynews.loadData(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadChange: function (id) {
            modal.Render("/Admin/HistoryNews/SelectVersion/" + id, "So sánh phiên bản", "modal-full");
        },
        restore: function (newsid, version) {
            $("#loading").show();
            swal({
                title: "Bạn có muốn khôi phục tới phiên bản này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/historynews/Restore", { newsid: newsid, version: version }, function (res) {
                        if (res.IsSuccess) {
                            alertmsg.success(res.Messenger);
                        } else {
                            alertmsg.error(res.Messenger);
                        }
                        $("#loading").hide();
                    });
                } 
                $("#loading").hide();
            });           
        }
    };
}();
$(function () { historynews.init(); });


