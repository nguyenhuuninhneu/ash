var feedback = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.theEnd = false;
            this.wait = false;
            this.DataSearch = {};
        },
        loadData: function (pageindex) {
            var self = this;
            if (this.FirstLoad) {
                $("#loading").show();
            }
            AjaxService.GET("/FeedBackFE/ListData?page=" + pageindex, null, function (res) {
                $("#feedback-details").append(res.viewContent);
                feedback.wait = false;
                if (res.totalPages == feedback.pageIndex) {
                    feedback.theEnd = true;
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadDataAdmin: function (pageindex) {
            var self = this;
            $("#loading").show();
            AjaxService.POST("/FeedBack/ListDataAdmin?page=" + pageindex, feedback.DataSearch, function (res) {
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
                            feedback.loadDataAdmin(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadFormAnswer: function (id) {
            modal.Render("/FeedBack/Answer/" + id, "Trả lời", "modal-lg");
        },
        onAnswerSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                feedback.loadDataAdmin(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
        },
        onSearchSuccess: function (res) {
            var self = this;
            $('#paginationholder').html('');
            $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
            $("#gridData").html(res.viewContent);
            feedback.DataSearch = {
                Status: $("#frmSearch #Status").val(),
            };
            if (res.totalPages > 1) {
                $('#pagination').twbsPagination({
                    startPage: 1,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        feedback.pageIndex = page;
                        feedback.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
            } else {
                alertmsg.error(res.Messenger);
            }
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
                    AjaxService.POST("/FeedBack/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadDataAdmin(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        onmultidelete: function () {
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một câu hỏi cần xóa");
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
                        AjaxService.POST("/FeedBack/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadDataAdmin(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
    }
}();
$(function () { feedback.init(); });


