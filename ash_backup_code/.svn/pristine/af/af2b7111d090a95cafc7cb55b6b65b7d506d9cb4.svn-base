var log = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};
        },
        loadData: function (pageindex) {
            var self = this;
            $("#loading").show();
            AjaxService.POST("/Admin/log/ListData?page=" + pageindex, log.DataSearch, function (res) {
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
                            log.loadData(self.pageIndex);
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
            log.DataSearch = {
                fromdate: $("#fromdate").val(),
                todate: $("#todate").val(),
                search: $("#search").val()
            };
            if (res.totalPages > 1) {
                $('#pagination').twbsPagination({
                    startPage: 1,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        log.pageIndex = page;
                        log.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        }
    };
}();
$(function () { log.init(); });


