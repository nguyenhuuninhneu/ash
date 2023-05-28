var TTHCcomment = function () {
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
            AjaxService.GET("/TTHC/ListDataComment?page=" + pageindex, null, function (res) {
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
                            TTHCcomment.loadData(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadDataCMT: function (id) {
                debugger;
                AjaxService.POST("/TTHC/ListDataComment/" + id, null, function (res) {
                    $("#gridData").html(res.viewContent);
                    self.FirstLoad = false;
                    $("#loading").hide();
                });

            },
        loadDataReply: function (id) {
            debugger;
            AjaxService.POST("/TTHC/ListDataReply/" + id, null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });

        },

      
        onAddRepSuccess: function (res) {
            debugger;
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                TTHCcomment.loadDataReply(res.cmtid);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        details: function (id) {
            modal.Render("/TTHC/DetailsComment/" + id, "Chi tiết bình luận", "modal-lg");
        },
        loadFrmAdd: function (CommentID) {
            modal.Render("/TTHC/AddRepCmt/" + CommentID, "Thêm mới bài viết", "modal-lg");
        },
        detailsRep: function (id) {
            debugger;
            modal.Render("/TTHC/DetailsReply/" + id, "Chỉnh sửa trả lời", "modal-lg");
        },
        activeRep: function (id, cmtid) {
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
                    AjaxService.POST("/TTHC/EditComment", { id: id }, function (res) {
                        TTHCcomment.loadDataReply(cmtid);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
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
                    AjaxService.POST("/TTHC/EditComment", { id: id , str: str}, function (res) {
                        TTHCcomment.loadDataCMT(cmtid);
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
                        AjaxService.POST("/TTHC/EditAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadData(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
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
                            TTHCcomment.pageIndex = page;
                            TTHCcomment.loadDataReply(res.cmtid);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
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
                    AjaxService.POST("/TTHC/DeleteComment", { id: id }, function (res) {
                        TTHCcomment.loadDataReply(cmtid);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
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
                    AjaxService.POST("/TTHC/DeleteComment", { id: id }, function (res) {
                        TTHCcomment.loadDataCMT(cmtid);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        onmultiRepdelete: function (cmtid) {
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
                        AjaxService.POST("/TTHC/DeleteAllRep", { lstid: $("#hdfID").val() }, function (res) {
                            TTHCcomment.loadDataReply(cmtid);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
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
                        AjaxService.POST("/TTHC/DeleteAllCMT", { lstid: $("#hdfID").val() }, function (res) {
                            TTHCcomment.loadDataCMT(cmtid);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
        
        onSearchSuccess: function (res) {
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
                        TTHCcomment.pageIndex = page;
                        TTHCcomment.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        },
    }
}();
$(function () { TTHCcomment.init(); });


