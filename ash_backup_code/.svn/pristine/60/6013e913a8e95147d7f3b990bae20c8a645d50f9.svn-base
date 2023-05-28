var newspaperCMT = function () {
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
            AjaxService.GET("/newspaper/ListData?page=" + pageindex, null, function (res) {
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
                            newspaperCMT.loadData(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        
        loadDataReply: function (id) {
            debugger;
            AjaxService.POST("/newspaper/ListDataCmt/" + id, null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });

        },
        
        onAddRepSuccess: function (res) {
            debugger;
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                newspaperCMT.loadDataReply(res.cmtid);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadFrmEdit: function (id) {
            modal.Render("/newspaper/Edit/" + id, "Cập nhật bài viết", "modal-lg");
        },
        
        details: function (id){
            modal.Render("/newspaper/DetailsComment/" + id, "Chi tiết bình luận", "modal-lg");
        },
      
        loadDetailCmt: function (id) {
            modal.Render("/newspaper/DetailsCmt/" + id, "Nội dung đóng góp ý kiến", "modal-lg");
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                newspaperCMT.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        active: function (id,str) {
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
                    AjaxService.POST("/newspaper/ChangeStatus", { id: id, str }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
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
                    AjaxService.POST("/newspaper/ChangeStatus", { id: id }, function (res) {
                        newspaperCMT.loadDataReply(cmtid);
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
                        AjaxService.POST("/newspaper/EditAll", { lstid: $("#hdfID").val() }, function (res) {
                            newspaperCMT.loadFrmCMT(cmtid);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
        ondelete: function (id) {
            var currentParent = $(".cParent").val();
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có chắc chắn không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "Không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/newspaper/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        //displaymenu();
                        self.loadData(self.pageIndex, currentParent);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        ondeleteCmt: function (id, cmtid) {
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
                        AjaxService.POST("/newspaper/DeleteCmt", { id: id }, function (res) {
                            newspaperCMT.loadDataReply(cmtid);
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
                            newspaperCMT.pageIndex = page;
                            newspaperCMT.loadDataReply(res.cmtid);
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
                        AjaxService.POST("/newspaper/DeleteAllRep", { lstid: $("#hdfID").val() }, function (res) {
                            newspaperCMT.loadDataReply(cmtid);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },        
        onmultidelete: function () {
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
                        AjaxService.POST("/newspaper/DeleteAllCMT", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadData(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        });
                    } 
                    $("#loading").hide();
                });
            }
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
                        newspaperCMT.pageIndex = page;
                        newspaperCMT.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        },
    }
}();
$(function () { newspaperCMT.init(); });


