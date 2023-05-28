var cateHome = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};

            $('#btnSearch').on('click', function () {
                var key = $('#Search').val();
                if (key === "" || key === null) {
                    alertmsg.error("Chưa nhập thông tin tìm kiếm.");
                    $('#Search').focus();
                    return;
                }
                
                window.location.href = "/pages/search.html?key=" + key;
            });
            //chuyển hướng sang trang tìm kiếm 
            $(".btnsearch").keypress(function (e) {
                if (e.which == 13) {
                    var key = $('#Search').val();
                    if (key === "" || key === null) {
                        $('#Search').focus();
                        return false;
                    }
                    window.location.href = "/pages/search.html?key=" + key;
                    return false;
                }
            });
        },
        loadData: function (pageindex) {
            var self = this;
          
            var id = $('#getId').val();
            $("#loading").show();
            AjaxService.POST("/Category/ListData?page=" + pageindex, { id: id }, function (res) {
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
                            cateHome.loadData(self.pageIndex);
                            var hnewslist = $('#newslist').offset().top - 50;
                            $('body,html').animate({ scrollTop: hnewslist }, 1600);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
    };
}();
$(function () { cateHome.init(); });


