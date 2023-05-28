var topiccomment = function () {
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
            AjaxService.GET("/Admin/topic/ListDataComment?page=" + pageindex, null, function (res) {
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
                            topiccomment.loadData(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        
        loadDataCMT: function (id) {
            debugger;
            AjaxService.POST("/Admin/topic/ListDataComment/" + id, null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });

        },

        loadDataReply: function (id) {
            debugger;
            AjaxService.POST("/Admin/topic/ListDataReply/" + id, null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });

        },
        
        onAddRepSuccess: function (res) {
            debugger;
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                topiccomment.loadDataCMT(res.cmtid);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadFrmAdd: function (CommentID,topicid) {
            AjaxService.POST("/Admin/topic/CheckReply", { id: CommentID }, function (res) {
                debugger;
                if (res.check == "true") { 
                    if (res.userother == true) {
                        alertmsg.success("Bạn đã chọn câu hỏi này!");
                        topiccomment.loadDataCMT(topicid);
                    }
                    modal.Render("/Admin/topic/AddRepCmt/" + CommentID, "Trả lời bình luận", "modal-lg");
                } else if (res.check == "false") {
                    message = res.FullName + " đang chọn.";
                    alertmsg.error(message);
                } 
            }); 
        },
        details: function (id){
            modal.Render("/Admin/topic/DetailsComment/" + id, "Chi tiết bình luận", "modal-lg");
        },
        detailsRep: function (id) {
            modal.Render("/Admin/topic/DetailsReply/" + id, "Chỉnh sửa trả lời", "modal-lg");
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                topiccomment.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
      
        active: function (id, cmtid,str) {
            $("#loading").show();
            AjaxService.POST("/Admin/topic/CheckReply", { id: id }, function (res) { 
                if (res.check == "true") {
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
                            AjaxService.POST("/Admin/topic/ChangeStatusCMT", { id: id, str: str }, function (res) {
                                topiccomment.loadDataCMT(cmtid);
                                alertmsg.success(res.Messenger);
                            });
                        }
                        $("#loading").hide();
                    });
                } else if (res.check == "false") {
                    message = res.FullName + " đang chọn.";
                    alertmsg.error(message);
                    $("#loading").hide();
                }
            }); 
        },
        changisview: function (id) {
            AjaxService.POST("/Admin/topic/ChangIsView", { id: id }, function (res) {});
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
                    AjaxService.POST("/Admin/topic/ChangeStatus", { id: id }, function (res) {
                        topiccomment.loadDataReply(cmtid);
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
                        AjaxService.POST("/Admin/topic/EditAll", { lstid: $("#hdfID").val() }, function (res) {
                            topiccomment.loadFrmCMT(cmtid);
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
                        AjaxService.POST("/Admin/topic/DeleteComment", { id: id }, function (res) {
                            topiccomment.loadDataReply(cmtid);
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
                            topiccomment.pageIndex = page;
                            topiccomment.loadDataReply(res.cmtid);
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
                        AjaxService.POST("/Admin/topic/DeleteAllRep", { lstid: $("#hdfID").val() }, function (res) {
                            topiccomment.loadDataReply(cmtid);
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
                    AjaxService.POST("/Admin/topic/DeleteComment", { id: id }, function (res) {
                        topiccomment.loadDataCMT(cmtid);
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
                        AjaxService.POST("/Admin/topic/DeleteAllCMT", { lstid: $("#hdfID").val() }, function (res) {
                            topiccomment.loadDataCMT(cmtid);
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
                        topiccomment.pageIndex = page;
                        topiccomment.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        },
        destroycm: function(id) {
            AjaxService.POST("/Admin/topic/DestroyChoose", { id: id }, function (res) {
                if (res.IsSuccess == true) {
                    alertmsg.success("Huỷ chọn bài thành công");
                    $(".modal").modal("hide");
                } else {
                    alertmsg.success("Huỷ chọn bài thất bại");
                }
            });
        },
        choose: function (id, cmtid,string) { 
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có muốn " + string + " câu hỏi này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/topic/Choose", { id: id }, function (res) {
                        if (res.IsSuccess == true) {
                            topiccomment.loadDataCMT(cmtid);
                            alertmsg.success(res.Messenger);
                        } else {
                            alertmsg.error(res.Messenger);
                        }
                    });
                }
                $("#loading").hide();
            });
        },
    }
}();
$(function () { topiccomment.init(); });


