var newscomment = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.theEnd = false;
            this.wait = false;
        },
        loadData: function (pageindex) {
            var self = this;
            if (this.FirstLoad) {
                $("#loading").show();
            }
            AjaxService.GET("/Admin/news/ListDataComment?page=" + pageindex, null, function (res) {
                console.log(res.viewContent);
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
                            newscomment.loadData(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        
        loadDataCMT: function (id,idcomment) {
            debugger;
            AjaxService.POST("/Admin/news/ListDataComment", { id: id, idcomment: idcomment }, function (res){
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });

        },

        loadDataReply: function (id) {
            debugger;
            AjaxService.POST("/Admin/news/ListDataReply/" + id, null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });

        },
        loadDataComment: function (id) { 
            AjaxService.POST("/Admin/news/ListDataComment/" + id, null, function (res) {
                debugger;
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });

        },
        
        onAddRepSuccess: function (res) {
            debugger;
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                newscomment.loadDataReply(res.cmtid);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        onAddCommentSuccess: function (res) {
            debugger;
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                newscomment.loadDataCMT(res.cmtid);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadFrmAdd: function (CommentID) {
            modal.Render("/Admin/news/AddRepCmt/" + CommentID, "Thêm mới bài viết", "modal-lg");
        },
        detailscomment: function (id){
            modal.Render("/Admin/news/DetailComment/" + id, "Chi tiết bình luận", "modal-lg");
        },
        editcomment: function (id) {
            modal.Render("/Admin/news/EditComment/" + id, "Chỉnh sửa bình luận", "modal-lg");
        },
        detailsRep: function (id) {
            modal.Render("/Admin/news/DetailReply/" + id, "Chi tiết trả lời", "modal-lg");
        },
        editsRep: function (id) {
            modal.Render("/Admin/news/EditReply/" + id, "Chỉnh sửa trả lời", "modal-lg");
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                newscomment.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
      
        active: function (id, cmtid, str) {
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
                    AjaxService.POST("/Admin/news/ChangeStatus", { id: id , str: str}, function (res) {
                        newscomment.loadDataCMT(cmtid);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        activeRep: function (id,cmtid) {
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
                    AjaxService.POST("/Admin/news/ChangeStatus", { id: id }, function (res) {
                        newscomment.loadDataReply(cmtid);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
       
        multiactive: function () {
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một bình luận cần kích hoạt");
            } else {
                $("#loading").show();
                swal({
                    title: "Bạn có chắc chắn muốn kích hoạt không?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Có",
                    cancelButtonText: "không",
                }, function (isConfirm) {
                    if (isConfirm) {
                        AjaxService.POST("/Admin/news/EditAll", { lstid: $("#hdfID").val() }, function (res) {
                            newscomment.loadFrmCMT(cmtid);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
        ondeleteRep: function (id, cmtid) {
                $("#loading").show();
                var self = this;
                swal({
                    title: "Bạn có muốn xóa tin này không?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Có",
                    cancelButtonText: "không",
                }, function (isConfirm) {
                    if (isConfirm) {
                        AjaxService.POST("/Admin/news/DeleteComment", { id: id }, function (res) {
                            newscomment.loadDataReply(cmtid);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
        },
        onSearchRep: function (res) {
            debugger;
            var self = this;
            $('#paginationholder').html('');
            if (res == undefined) {
                $("#gridData").html("");
            }
            else {
                $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                $("#gridData").html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#pagination').twbsPagination({
                        startPage: 1,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            newscomment.pageIndex = page;
                            newscomment.loadDataReply(res.cmtid);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            }
        },
        onmultiRepdelete: function (cmtid) {
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một bình luận cần xóa");
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
                        AjaxService.POST("/Admin/news/DeleteAllRep", { lstid: $("#hdfID").val() }, function (res) {
                            newscomment.loadDataReply(cmtid);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
       
        ondelete: function (id, cmtid) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có muốn xóa tin này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/news/DeleteComment", { id: id }, function (res) {
                        newscomment.loadDataCMT(cmtid);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
       
        onmultidelete: function (cmtid) {
            debugger;
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một bình luận cần xóa");
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
                        AjaxService.POST("/Admin/news/DeleteAllCMT", { lstid: $("#hdfID").val() }, function (res) {
                            newscomment.loadDataCMT(cmtid);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
        ListData: function () {
            debugger;
            AjaxService.POST("/Admin/ThongBao/ListData/", null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
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
                        newscomment.pageIndex = page;
                        newscomment.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        },
    }
}();
$(function () { newscomment.init(); });


