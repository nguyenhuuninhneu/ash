var news = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};
            this.check_hide_thongke();
           
        },

    loadData: function (pageindex,id) {
        var self = this;
        self.pageIndex = pageindex;
        if (this.FirstLoad) {
            $("#loading").show();
        }
           
        var Status = "";
        if (id == null || id == undefined) {
            Status = $("#Status").val();
        }
        else {
            Status = id;
        }
        var position = $('#position').val();
        var keySearch = $("#keySearch").val();
        var CreatedBy = $("#CreatedBy").val();
        var CategoryId = $("#CategoryId").val();
        var tuNgay = $("#tuNgay").val();
        var denNgay = $("#denNgay").val();
        var IsPublish = $("#IsPublish").val();
        var TraLai = $("#TraLai").val();
        var luaChon = $("#luaChon").val();
        var Type = $("#Type").val();
        news.DataSearch = { keySearch: keySearch, Status: Status, Position: position, CreatedBy: CreatedBy, CategoryId: CategoryId, tuNgay: tuNgay, denNgay: denNgay, luaChon: luaChon, TraLai: TraLai, IsPublish: IsPublish, Type: Type }

        AjaxService.POST("/Admin/news/ListData?page=" + self.pageIndex, news.DataSearch, function (res) {
            $("#gridData").html(res.viewContent);
            $('#paginationholder').html('');
            if (res.totalPages > 1) {
                $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                $('#pagination').twbsPagination({
                    startPage: self.pageIndex,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        self.pageIndex = page;
                        self.currentpage = page;
                        news.loadData(self.pageIndex); 
                    }
                });
            }
            self.FirstLoad = false;
            self.check_hide_thongke();
            $("#loading").hide();
        });
    },
    
    loadDataSent: function (pageindex) {
        var self = this;
        self.pageIndex = pageindex;
        if (this.FirstLoad) {
            $("#loading").show();
        }
        var Title = $("#Title").val();
        var CreatedBy = $("#CreatedBy").val();
        var CategoryId = $("#CategoryId").val();
        var Status = $("#Status").val();
        var NgayDang = $("#NgayDang").val();
        var NgayKet = $("#NgayKet").val(); 
        news.DataSearch = { Title: Title, Status: Status, CreatedBy: CreatedBy, CategoryId: CategoryId, NgayDang: NgayDang, NgayKet: NgayKet}
        AjaxService.POST("/Admin/news/ListDataSent?page=" + self.pageIndex, news.DataSearch, function (res) {
            $("#gridDataSent").html(res.viewContent);
            $('#paginationholder').html('');
            if (res.totalPages > 1) {
                $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                $('#pagination').twbsPagination({
                    startPage: self.pageIndex,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        self.pageIndex = page;
                        news.loadDataSent(self.pageIndex);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        });
    },
    exportExcel: function () {
           
        var tuNgay = $('#tuNgay').val();
        var denNgay = $('#denNgay').val();
        $.ajax({
            url: '/Admin/News/ExportExcel',
            type: 'POST',
            data: { tuNgay: tuNgay, denNgay: denNgay },
            success: function (res) {

            }
        });
    },
    addRelatedNews: function () {
        AjaxService.POST("/Admin/News/LoadRelatedNews/", null, function (res) {
            $('#imgload').hide();
            $("#loadrelated").hide();
            $("#loadnews").html(res.viewContent);
            $("#RelatedNews").select2();
            $("#RelatedNews").select2("open");
        });
    },
    editRelatedNews: function () {
        AjaxService.POST("/Admin/News/LoadRelatedNews/", null, function (res) {
            $('#imgload').hide();
            $("#loadrelated").hide();
            $("#loadnews").html(res.viewContent);
            $("#RelatedNews").select2();
            var IdHide = $("#IdHide").val();
            if (IdHide != "") {
                var arrIdHide = IdHide.split(',');
                $('#RelatedNews').val(arrIdHide).trigger('change');
            }
            $("#RelatedNews").select2("open");
        });

    },
    loadfrmDetail: function (id) {
        modal.Render("/Admin/news/Detail/" + id, "Chi tiết bài viết", "modal-lg");
    },
    onSearchSuccess: function (res) {
        var self = this;
        $('#paginationholder').html('');
        $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
        $("#gridData").html(res.viewContent);
        news.DataSearch = {
            Title: $("#Title").val(),
            Author: $("#Author").val(),
            DisplayZone: $("#DisplayZone").val(),
            LangCode: $("#LangCode").val(),
            PageElementId: $("#PageElementId").val(),
            Status: $("#Status").val(),
            CreatedBy: $("#CreatedBy").val(),
            CategoryId: $("#CategoryId").val(),
            tuNgay: $("#tuNgay").val(),
            denNgay: $("#denNgay").val()
        };
        if (res.totalPages > 1) {
            $('#pagination').twbsPagination({
                startPage: 1,
                totalPages: res.totalPages,
                visiblePages: 5,
                onPageClick: function (event, page) {
                    news.pageIndex = page;
                    news.loadData(page);
                }
            });
        }
        self.FirstLoad = false;
        $("#loading").hide();
    },
        //Check validate form trước khi xuất bản
    checkpublish: function () {
        var photo = $("#Photo").val();
        if (photo == "") {
            alertmsg.error("Bạn chưa chọn ảnh");
            return false;
        }
        swal({
            title: "Bạn có muốn đăng tin này không?",
            text: "",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "không",
        }, function (isConfirm) {
            if (isConfirm) {
                var status = $('#Status').val();
                var money = $('#NewMoney').val();
                if (status != 14) {
                    alertmsg.error("Bài viết phải ở trạng thái xuất bản");
                    return false;
                }
                var newsmoney = $('#NhuanBut').val();
                if (newsmoney == "") {
                    return false;
                }
                if (money == "") {
                    $('#typemoney').html('<span style="color:red" id="loaitin">Bạn chưa chọn loại tin</span>');
                    return false;
                } else {
                    $('#loaitin').remove();
                    $('#Publish').val("1");
                    $('#frmpost').submit();
                } 
            }

        }); 
    },
        // Hàm xác nhận và xuất bản
    publish: function () {
        swal({
            title: "Bạn có muốn đăng tin này không?",
            text: "",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "không",
        }, function (isConfirm) {
            if (isConfirm) {
                $('#Publish').val("1");
                $('#frmpost').submit();
            }
         
        });
    },
        // Check validate form trước khi chuyển xử lý
 
    checkprocess: function (id, code) {
        var cate = $('#CategoryIdStr').val();
        var description = $('#Description').val().trim();
        var title = $('#Title').val().trim();
        var author = $('#Author').val().trim();
        var photo = $("#Photo").val();
        var money = $('#NewMoney').val();

        if (cate ==null) {
            alertmsg.error("Bạn chưa chọn danh mục")
            return false;
        }

        if (title == "") {
            alertmsg.error("Bạn chưa thêm tiêu đề")
            return false;
        }
        if (photo == "") {
            alertmsg.error("Bạn chưa chọn ảnh")
            return false;
        }
        if (description == "") {
            alertmsg.error("Bạn chưa thêm miêu tả")
            return false;
        }
        if (author == "") {
            alertmsg.error("Bạn chưa thêm tác giả")
            return false;
        }
        if (money == "-- Chọn --") {
            $('#typemoney').html('<span style="color:red" id="loaitin">Bạn chưa chọn loại tin</span>');
            return false;
        } else {
            $('#loaitin').remove();
            news.changeprocess(id, code);
        }
    },
        //Xác nhận chuyển
    changeprocess: function (id, code) {
        swal({
            title: "Bạn có muốn chuyển xử lý không?",
            text: "",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "không",
        }, function (isConfirm) {
            if (isConfirm) {
                news.chuyenXuLy(id, code);
            }

        });
    },
       //Hàm chuyển xử lý
    chuyenXuLy: function (id,code) {
        $("#loading").show();
        $.ajax({
            url: "/Admin/process/ProcessTransfer",
            type: "POST",
            data: { FkId: id, Code: code },
            success: function (data) {
                if (data) {
                    $("#chuyenXuLyForm").html(data);
                    $("#chuyenXuLyForm").modal("show");
                }
            }
        })
    },
    unpublish: function () {
        var photo = $("#Photo").val();
        if (photo == "") {
            alertmsg.error("Bạn chưa chọn ảnh");
            return false;
        }
        swal({
            title: "Bạn có muốn hủy tin này không?",
            text: "",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "không",
        }, function (isConfirm) {
            if (isConfirm) {
                $('#Publish').val("1");
                $('#frmpost').submit();
            }

        });
    },
        //Check validate form trước khi duyệt
    checkduyet: function () {
        var photo = $("#Photo").val();
        if (photo == "") {
            alertmsg.error("Bạn chưa chọn ảnh");
            return false;
        }
        var money = $('#NewMoney').val();
        var IsUserXB = $('#IsUserXB').val();
        if (money == "") {
            $('#typemoney').html('<span style="color:red" id="loaitin">Bạn chưa chọn loại tin</span>');
            return false;
        } else {
            swal({
                title: "Bạn có muốn duyệt tin này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    $('#loaitin').remove();
                    $('#isDuyet').val(IsUserXB);
                    $('#frmpost').submit();
                } 
            }); 
        }
    },
        //Hàm xác nhận duyệt 
    duyettin: function () { 
        swal({
            title: "Bạn có muốn duyệt tin này không?",
            text: "",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "không",
        }, function (isConfirm) {
            if (isConfirm) {
                $('#isDuyet').val("1");
                $('#frmpost').submit();
            } 
        });
    },
    ondelete: function (id) {
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
                AjaxService.POST("/Admin/news/Delete", { id: id }, function (res) {
                    if (res.IsSuccess == true) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    }
                    else {
                        alertmsg.error(res.Messenger);
                    }
                });
            } 
            $("#loading").hide();
        });
    },
    onmultidelete: function () {
        var self = this;
        if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
            alertmsg.error("Bạn cần chọn ít nhất một tin bài cần xóa");
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
                    AjaxService.POST("/Admin/news/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                        if (res.IsSuccess == true) {
                            self.pageIndex = 1;
                            self.loadData(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        }
                        else {
                            alertmsg.error(res.Messenger);
                        }
                    });
                }
                $("#loading").hide();
            });
        }
    },
    loadCatbyLang: function (langcode) {
        AjaxService.GET("/Admin/Category/GetCatbyLang", { lang: langcode }, function (res) {
            var html = "";
            for (var i = 0; i < res.length; i++) {
                html += "<option value='" + res[i].ID + "'>" + Array(res[i].Level).join("--") + res[i].Name + "</option>";
            }
            $("#CategoryId").html(html);
        });
    },
    loadBoxNewsConfig: function (_catid, zoneselected) {
        var arrzone = [];
        if (zoneselected != undefined) {
            arrzone = zoneselected.split(',');
        }
        AjaxService.GET("/Admin/BoxNewsConfig/GetListConfigByCatId", { catid: _catid }, function (res) {
            var html = "";
            for (var i = 0; i < res.length; i++) {
                var chk = jQuery.inArray(res[i].Code, arrzone) != -1 ? "checked" : "";
                html += '<div class="checkbox checkbox-success checkbox-inline">\
                                <input type="checkbox" ' + chk + ' id="' + res[i].Code + '" name="DisplayZone" value="' + res[i].Code + '">\
                                <label for="' + res[i].Code + '"> ' + res[i].Name + ' </label>\
                            </div>';
            }
            $("#boxnewsconfig").html(html);
        });
    },
    onApproved: function (id) {
        $("#loading").show();
        var self = this;
        swal({
            title: "Bạn có chắc chắn duyệt tin này không?",
            text: "",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "không",
        }, function (isConfirm) {
            if (isConfirm) {
                AjaxService.POST("/Admin/News/Approved", { id: id}, function (res) {
                    self.pageIndex = 1;
                    self.loadData(self.pageIndex);
                    alertmsg.success(res.Messenger);
                });
            }
            $("#loading").hide();
        });
    }, 
    onApprovedDown: function (id) {
        $("#loading").show();
        var self = this;
        swal({
            title: "Hủy duyệt tin này?",
            text: "",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "không",
        }, function (isConfirm) {
            if (isConfirm) {
                AjaxService.POST("/Admin/News/ApprovedDown", { id: id }, function (res) {
                    self.pageIndex = 1;
                    self.loadData(self.pageIndex);
                    alertmsg.success(res.Messenger);
                });
            }
            $("#loading").hide();
        });
    },
    getNewsSuccess: function (res) {
        $('#modalgetnews').modal('hide');
        news.loadData(1);
        alertmsg.success("Lấy tin bài thành công !");
    },
    changePublic: function (id) {
        $("#loading").show();
        var self = this;
        swal({
            title: "Thay đổi trạng thái xuất bản?",
            text: "",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "không",
        }, function (isConfirm) {
            if (isConfirm) {
                AjaxService.POST("/Admin/News/ChangePublic", { id: id}, function (res) {
                    self.pageIndex = 1;
                    self.loadData(self.pageIndex);
                    alertmsg.success(res.Messenger);
                });
            }
            $("#loading").hide();
        });
    },
    check_hide_thongke: function (id) {
       
        var thongke = $("#thongke").val();
        $(".hide_thongke").show();
        if (thongke == 1) {
            $(".hide_thongke").hide();
            $(".publish").css('cursor', 'default');
            $(".publish").removeAttr('onclick');
            $(".chonbai").remove();
        }
    }, 
    ChooseCT: function () {
        var IdCTV = $('#CTVID').val();
        var IdCreatedBy = $('#CreatedBy').val();
        if (IdCTV != "") {
            $('#infoTG').attr("onclick", "account.loadfrmInfo(" + IdCTV + ")");
        } else {
            $('#infoTG').attr("onclick", "account.loadfrmInfo(" + IdCreatedBy + ")");
        }
    }, 
};
}();
$(function() {
    news.init(); 
    var count = $("#tongbanghi").val();
    $('#countrecord').html(count);

    var chat = $.connection.loadNews;
    chat.client.addNewMessageToPage = function (id) {
        //debugger;
         AjaxService.POST("/Admin/News/ReplaceRow",{ id: id },function(res) {
             $('#rownews_' + id).html(res.viewContent);
         }); 
    };
    $.connection.hub.start().done(function () {
        $('.btnchosse').click(function () {
            var id = $(this).attr("data-id");
            var string = $(this).attr("data-string"); 
            var boolchoose = $(this).attr("data-choose");
            if (boolchoose ==  0) {
                var self = this;
                swal({
                        title: "Bạn có muốn " + string + " tin bài này không?",
                        text: "",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Có",
                        cancelButtonText: "không",
                    },
                    function (isConfirm) {
                        if (isConfirm) { 
                            AjaxService.POST("/Admin/News/Choose", { id: id }, function (res) {
                                if (res.IsSuccess == true) {
                                    alertmsg.success(res.Messenger);
                                    chat.server.send(id);
                                } else {
                                    alertmsg.error(res.Messenger);
                                }
                            });
                        }
                    });
            } else if (boolchoose == 3) { alertmsg.error("Bạn không có quyền chọn!"); }
        });

        $('.getnews').click(function () { 
            var id = $(this).attr("data-id");
            swal({
                title: "Bạn có muốn lấy tin này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    debugger;
                    AjaxService.POST("/Admin/News/GetNewsRow", { id: id }, function (res) {
                        if (res.IsSuccess == true) {
                            alertmsg.success(res.Messenger);
                            chat.server.send(id);
                        } else {
                            alertmsg.error(res.Messenger);
                        }
                    });
                }
            });
        });

        $('.xbclick').click(function() {
            var id = $(this).attr("data-id");
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có chắc chắn duyệt tin này không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/News/ApprovedXB", { id: id }, function (res) { 
                        alertmsg.success(res.Messenger);
                        chat.server.send(id);
                    });
                }
                $("#loading").hide();
            });
        }); 
    });
    // load tin lien quan
    $('#RelatedNews').select2(
    {
        placeholder: 'Nhập từ khóa tìm kiếm...',
        allowClear: true,
        multiple: true,
        ajax: {
            quietMillis: 150,
            url: "/Admin/News/GetRelateNews",
            dataType: 'json',
            data: function (term, page) {
                return {
                    pageSize: 10,
                    pageNum: page,
                    keySearch: term
                };
            },
            results: function (res) {
                var more = (res.page * res.pagesize) < res.Total;
                return { results: res.Data, more: more };
            }
        }
    });
    var RelatedNewsIdHide = $("#RelatedNewsIdHide").val();
    if (RelatedNewsIdHide != "" && RelatedNewsIdHide != undefined) {
        debugger;
        var textRelateNews = $("#jsonRelateNews").val();
        var jsonRelateNews = JSON.parse(textRelateNews);
        $('#RelatedNews').select2('data', jsonRelateNews);
    }
});
  