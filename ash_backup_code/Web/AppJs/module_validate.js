$(document).ready(function () {
    $.validator.addMethod("CheckVietLienKhongDau", function (value, element) {
        var code = $("#Code").val();
        var isSuccess = true;//không trùng
        $.ajax({
            async: false,
            url: "/Module/CheckVietLienKhongDau",
            type: "get",
            dataType: "json",
            data: {
                
                code: code

            },
            success: function (res) {
                if (res.Check == true) {
                    isSuccess = false;
                }
            }
        });
        return isSuccess;

    }, "Mã phải viết liền và không dấu");
    $.validator.addMethod("CheckTrungCode", function (value, element) {
        debugger;
        var code = $("#Code").val();
        var id = $("#ID").val();
        var isSuccess = true;//không trùng
        $.ajax({
            async: false,
            url: "/Admin/Module/CheckTrungCode",
            type: "get",
            dataType: "json",
            data: {
                id: id,
                code: code

            },
            success: function (res) {
                if (res.trungMa == true) {
                    isSuccess = false;
                }
            }
        });
        return isSuccess;

    }, "Mã đã tồn tại");
    $.validator.addMethod("CheckTrung", function (value, element) {
        var name = $("#Name").val();
        var id = $("#ID").val();
        var isSuccess = true;//không trùng
        $.ajax({
            async: false,
            url: "/Module/CheckTrung",
            type: "get",
            dataType: "json",
            data: {
                id:id,
                name: name,
     

            },
            success: function (res) {
                if (res.trungTen == true) {
                    isSuccess = false;
                }
            }
        });
        return isSuccess;

    }, "Tên đã tồn tại");

    $(document).ready(function () {
        $("#basicForm").validate({
            rules:
            {
                Name:
                {
                    required: true,
                    CheckTrung: true
                },
                Code:
               {
                   required: true,
                   CheckVietLienKhongDau: true,
                   CheckTrungCode: true
               },
                Ordering: {
                    required: true,
                    digits: true
                }
            },
            messages:
            {
                Name:
                {
                    required: "Chưa nhập tên tài liệu",
                    CheckTrung: "Tên đã tồn tại"
                },
                Code:
                {
                    required: "Chưa nhập tên tài liệu",
                    CheckVietLienKhongDau: "Tên phải viết liền và không dấu",
                    CheckTrungCode: "Mã đã tồn tại "
                },
                Ordering: {
                    required: "Nhập số thứ tự",
                    digits: "Nhâp kiểu số"
                }

            }
        });
        account.initcheckall();
        $('#pagemanager').slimScroll({
            height: '300px',
        });
        $('.datepicker-mask1').datetimepicker({
            locale: 'vi',
            format: 'DD/MM/YYYY'
        });
        $(".select").select2();
    });
});