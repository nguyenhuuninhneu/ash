﻿$(document).ready(function () {
    $.validator.addMethod("CheckTrung", function (value, element) {
        var name = $("#Name").val();
        var id = $("#ID").val();
        var isSuccess = true;//không trùng
        $.ajax({
            async: false,
            url: "/DocumentPrice/CheckTrung",
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
                Price_min: {
                    required: true,
                    //digits: true
                },
                Price_max: {
                    required: true,
 
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
                Price_min: {
                    required: "Nhập giá tối thiểu",
                
                },
                Price_max: {
                    required: "Nhập giá tối đa",
        
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
        $('#Price_min').on('keyup', function () {
            debugger;
            var money = $(this).val();
            var count = money.length;
            money = money.replace(".", "");
            money = money.replace(".", "");
            if (count > 3) {
                var mumber = Math.floor(count / 3);
                money = accounting.formatNumber(money)
                for (var i = 1; i <= mumber; i++) {
                    money = money.replace(",", ".");
                }
            }
            $(this).val(money);
        });
        $('#Price_max').on('keyup', function () {
            debugger;
            var money = $(this).val();
            var count = money.length;
            money = money.replace(".", "");
            money = money.replace(".", "");
            if (count > 3) {
                var mumber = Math.floor(count / 3);
                money = accounting.formatNumber(money)
                for (var i = 1; i <= mumber; i++) {
                    money = money.replace(",", ".");
                }
            }
            $(this).val(money);
        })
    });
});