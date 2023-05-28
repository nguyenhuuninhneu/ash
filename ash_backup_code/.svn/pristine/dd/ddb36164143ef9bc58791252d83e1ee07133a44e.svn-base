var thamDoYKien = function () {
    return {
        init: function () {
            this.FirtLoad = true;
            this.LoadData();
        },
        //hàm load dữ liệu
        LoadData: function () {
            var self = this;
            if (this.FirtLoad) {
                $("#loading").show();
            }
            $.ajax({
                url:"/ThamDoYKien/ListData",
                type: "POST",
                dataType: "json",
                success: function (res) {
                    $("#gridData").html(res.contentView);
                    self.FirstLoad = false;
                    $("#loading").hide();
                }
            });

           
        },
        //load form add
        loadfrmAdd: function () {
            modal.Render("ThamDoYKien/Add", "Thêm mới thăm dò ý kiến ", "modal-lg");
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                thamDoYKien.LoadData();
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadFrmEdit: function (id) {
            modal.Render("ThamDoYKien/Edit/" + id, "Sửa thăm dò ý kiến", "modal-lg");
        },
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                thamDoYKien.LoadData(this.pageIndex);
                modal.hide();
            }
            else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        //ham cap nhat vi tri
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
            AjaxService.POST("/ThamDoYKien/UpdatePosition", { value: strValue }, function (res) {
                if (res.IsSuccess == true) {
                    alertmsg.success(res.Messenger);
                    $("#gridData").html(res.ViewContent);
                } else {
                    alertmsg.error(res.Messenger);
                }
                self.LoadData(self.pageIndex);
                $("#loading").hide();
            });
        },
        //ham xoa
        onDelete: function (id) {
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
                closeOnConfirm: false,
                closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    swal("OK!", "", "success");
                    AjaxService.POST("/ThamDoYKien/Delete", { id: id }, function (res) {
                        self.LoadData();
                        alertmsg.success(res.Messenger);
                    });
                } else {
                    swal("Cancelled", "", "error");
                }
                $("#loading").hide();
            });
        },
        //ham xoa nhieu
        onmultidelete: function () {
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một danh muc cần xóa");
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
                    closeOnConfirm: false,
                    closeOnCancel: false
                }, function (isConfirm) {
                    if (isConfirm) {
                        swal("OK!", "", "success");
                        AjaxService.POST("/ThamDoYKien/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.LoadData();
                            alertmsg.success(res.Messenger);
                        });
                    } else {
                        swal("Cancelled", "", "error");
                    }
                    $("#loading").hide();
                });
            }
        },
        //hàm bình chọn
        BinhChon:function(pollid)
        {
            var id = $("input[name=radio_" + pollid + "]:checked").val();
            if (id == null)
            {
                alertmsg.error("Bạn chưa kích chọn");
            }
            $.ajax({
                url:"/ThamDoYKien/BinhChon",
                type:"POST",
                dataType: "json",
                data: { id: id, pollid: pollid },
                success:function(res)
                {
                    if(res.IsSuccess)
                    {
                        alertmsg.success(res.Messenger);
                        $('#BinhChon_' + pollid).hide();
                    }
                    else
                    {
                        alertmsg.error(res.Messenger);
                    }
                }
            });
            
        },
       //hàm kết quả 
    }
}();
$(function () { thamDoYKien.init(); });
