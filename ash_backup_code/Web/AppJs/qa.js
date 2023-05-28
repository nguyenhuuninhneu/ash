var qa = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.theEnd = false;
            this.wait = false;
            this.DataSearch = {};
        },
        loadData: function (pageindex, keyword, LinhVucID) {
            var self = this;
            keyword = keyword == undefined ? "" : keyword;
            LinhVucID = LinhVucID == undefined ? "" : LinhVucID;
            if (this.FirstLoad) {
                $("#loading").show();
            }
            AjaxService.GET("/qa/ListData?page=" + pageindex + "&keyword=" + keyword + "&LinhVucID=" + LinhVucID, null, function (res) {
                $("#qa-details").append(res.viewContent);
                qa.wait = false;
                if (res.totalPages == qa.pageIndex) {
                    qa.theEnd = true;
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        showAnswer: function (id) {
            $("#loading").show();
            AjaxService.GET("/qa/Details/" + id, null, function (res) {
                //console.log(res);
                $("#showAnswer").html(res.viewContent);
                $("#modalqa").modal();
                $("#loading").hide();
            });
        },
        loadDataAdmin: function (pageindex) {
            var self = this;
            $("#loading").show();
            AjaxService.POST("/Admin/qa/ListDataAdmin?page=" + pageindex, qa.DataSearch, function (res) {
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
                            qa.loadDataAdmin(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadFormAnswer: function (id) {
            modal.Render("/Admin/qa/Answer/" + id, "Thông Tin Câu Hỏi", "modal-lg");
        },
        onAnswerSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                qa.loadDataAdmin(this.pageIndex);
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
            qa.DataSearch = {
                LinhVucID: $("#frmSearch #LinhVucID").val()
            };
            if (res.totalPages > 1) {
                $('#pagination').twbsPagination({
                    startPage: 1,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        qa.pageIndex = page;
                        qa.loadData(page);
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
                    AjaxService.POST("/Admin/qa/Delete", { id: id }, function (res) {
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
                        AjaxService.POST("/Admin/qa/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadDataAdmin(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
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
                cancelButtonText: "không"
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/QA/ChangeStatus", { id: id, str:str }, function (res) {
                        self.pageIndex = 1;
                        self.loadDataAdmin(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
    }
}();
$(function () { qa.init(); });


