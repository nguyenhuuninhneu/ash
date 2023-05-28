$(document).ready(function () {
    debugger;
    $.validator.addMethod("checkDate", function (value, element) {

        var tuNgay = $("#tuNgay").val();
        var denNgay = $("#denNgay").val();
        var isSuccess = true;//không trùng
        $.ajax({
            async: false,
            url: "/Admin/eMagazine/checkDate",
            type: "get",
            dataType: "json",
            data: {
                tuNgay: tuNgay,
                denNgay: denNgay
            },
            success: function (res) {
                if (res.check == true) {
                    isSuccess = false;
                }
            }
        });
        return isSuccess;

    }, "Ngày bắt đầu phải nhỏ hơn ngày kết thúc");
    $("#formTimKiem").validate({
        rules: {
            tuNgay: {
                required: true,
                checkDate:true
            },
            denNgay: {
                required: true,
                checkDate:true
            },
        },
        messages: {
            tuNgay: {
                required: "chọn ngày",
                checkDate:"Ngày bắt đầu phải nhỏ hơn ngày kết thúc"
            },
            denNgay: {
                required: "chọn ngày",
                checkDate: "Ngày bắt đầu phải nhỏ hơn ngày kết thúc"
            },
        }
    });
});