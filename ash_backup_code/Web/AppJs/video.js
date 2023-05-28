var video = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};
            $("#frmSearch #Name").change(function () {
                $("#btnSearch").click();
            });
            $("#frmSearch #LangCode").change(function () {
                $("#btnSearch").click();
            });
            $("#frmSearch #PageElementId").change(function () {
                $("#btnSearch").click();
            });
        },
        loadData: function (pageindex) {
            var self = this;
            $("#loading").show();
            //var NgayDang = $("#NgayDang").val();
            //var NgayKet = $("#NgayKet").val();
            //video.DataSearch = { NgayDang: NgayDang, NgayKet: NgayKet }
            AjaxService.POST("/Admin/video/ListData?page=" + pageindex, video.DataSearch, function (res) {
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
                            video.loadData(self.pageIndex);
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
            video.DataSearch = {
                NgayDang: $("#NgayDang").val(),
                NgayKet: $("#NgayKet").val()
            };
            if (res.totalPages > 1) {
                $('#pagination').twbsPagination({
                    startPage: 1,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        video.pageIndex = page;
                        video.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        },
        AddVideo: function () {
            modal.Render("/Admin/video/AddVideo", "Thêm mới video", "modal-lg");
        },
        loadfrmAdd: function () {
          
            modal.Render("/Admin/video/Add", "Thêm mới video", "modal-lg");
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                video.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadfrmedit: function (id) {
            modal.Render("/Admin/Video/Edit/" + id, "Cập nhật video", "modal-lg");
        },
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                video.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
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
                    
                    AjaxService.POST("/Admin/video/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                } 
                $("#loading").hide();
            });
        },
        onmultidelete: function () {
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một video cần xóa");
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
                    closeOnConfirm: true,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        AjaxService.POST("/Admin/video/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadData(self.pageIndex);
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
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/video/ChangeStatus", { id: id, str: str }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        onupdateposittion: function () {
            var self = this;
            $("#loading").show();
            var arrValue = [];
            $("table tbody tr").each(function () {
                var id = $(this).find("#item_ID").val();
                var ordering = $(this).find("input[type=text]").val();
                var str = id + ":" + ordering;
                arrValue.push(str);
            });
            var strValue = arrValue.join("|");
            AjaxService.POST("/Admin/Video/UpdatePosition", { value: strValue }, function (res) {
                if (res.IsSuccess == true) {
                    alertmsg.success(res.Messenger);
                    $("#gridData").html(res.ViewContent);
                } else {
                    alertmsg.error(res.Messenger);
                }
                self.loadData(self.pageIndex);
                $("#loading").hide();
            });
        },
        jYoutube: function (url, size) {
            if (url === null) { return ""; }

            size = (size === null) ? "big" : size;
            var vid;
            var results;

            results = url.match("[\?&]v=([^&#]*)");

            vid = (results === null) ? url : results[1];

            if (size == "small") {
                return "http://img.youtube.com/vi/" + vid + "/2.jpg";
            } else {
                return "http://img.youtube.com/vi/" + vid + "/0.jpg";
            }
        },
        typeVideo: function () {
            debugger;
            var value = $("#type-video").val();
            if (value == "1") {
                $("#video-youtube").hide();
                $("#video-nguonkhac").hide();
                $("#video-upload").show();
                $("#indirect").val("1");
                $('#video-upload').removeAttr('hidden');
            }
            else if(value=="2") {
                $("#video-youtube").show();
                $("#video-upload").hide();
                $("#video-nguonkhac").hide();
                $("#indirect").val("2");
                $('#video-youtube').removeAttr('hidden');
            }
            else if(value=="3") {
                $("#video-youtube").hide();
                $("#video-upload").hide();
                $("#video-nguonkhac").show();
                $('#video-youtube').removeAttr('hidden');
                $("#indirect").val("3");
            }
        },
        loadfrmDetail: function (id) {
            modal.Render("/Admin/Video/Detail/" + id, "Chi tiết video", "modal-lg");
    },
    };
}();
$(function () { video.init(); });

