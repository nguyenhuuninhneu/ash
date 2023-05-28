$(document).ready(function () {
    $.validator.addMethod("CheckTrung", function (value, element) {
        var name = $("#Name").val();
        var id = $("#ID").val();
        var isSuccess = true;//không trùng
        $.ajax({
            async: false,
            url: "/RecruitCategory/CheckTrung",
            type: "get",
            dataType: "json",
            data: {
                id:id,
                name: name

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