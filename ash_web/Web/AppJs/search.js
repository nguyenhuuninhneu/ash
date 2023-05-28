var search = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};
            
        },
        loadData: function (page) { 
            var keyword = $('#keySearch').val();
            var tuychon = 0;
            var radios = document.getElementsByName('tuychon');

            for (var i = 0, length = radios.length; i < length; i++) {
                if (radios[i].checked) {
                    tuychon = radios[i].value;
                    break;
                }
            }
           
            var luaChon = $('#luaChon').val();
            var CategoryId = $('#CategoryId').val();
           var datePost = $('#NgayDang').val();
           var dateFinish = $('#NgayKet').val();
            var self = this;
            $("#loading").show();
            AjaxService.POST("/Home/Search", { keySearch: keyword, luaChon: luaChon, page: page, tuychon: tuychon, CategoryId: CategoryId, tuNgay: datePost, denNgay: dateFinish }, function (res) {
                $("#gridData").html(res.viewContent);
                CropImage();
                if (res.totalPages > 1) {
                    $('#paginationholder').html('');
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            search.loadData(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        onAddSuccess: function (res) {
            $("#loading").hide();
        },
    };
}();
$(function () { search.init(); });


