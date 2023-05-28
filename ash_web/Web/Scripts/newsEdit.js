$(document).ready(function () {
    $('#NhuanBut').on('keyup', function () {
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
    var price = $("#NewMoney").find('option:selected').attr("data-price");
    price = accounting.formatNumber(price)
    var count = price.length;
    var mumber = count / 3;
    for (var i = 0; i <= mumber; i++) {
        price = price.replace(",", ".");
    }
    $("#NhuanBut").val(price);
    $("#showMoney").html(price);
    //lockchar
    $('.lockChar').on("keypress", function () {
        return event.charCode >= 48 && event.charCode <= 57
    })
    //check duyet tin

    function checkvali() {
        debugger;
        var photo = $("#Photo").val();
        var money = $('#NewMoney').val();
        if (photo == "") {
            alertmsg.error("Bạn chưa chọn ảnh");
            return false;
        }
        if (money == "-- Chọn --") {
            alertmsg.error("Bạn chưa chọn loại tin");
            //$('#typemoney').html('<span style="color:red" id="loaitin">Bạn chưa chọn loại tin</span>');
            return false;
        } else {
            $('#loaitin').remove();
        }
        $("#frmpost").submit();
    }

    $('#duyettin').on('click',
        function() {
            $('#isDuyet').val("1");
        });
    $('#save').on('click',
        function() {
            checkvali();
        });
    $('#vali').on('click',
        function() {
            checkvali();
        });
});
var showMoney = function () {
    var selectid = $("#NewMoney").val();
    $("#NhuanBut").val(0);
    $("#showMoney").html(0);
    if (selectid > 0) {
        var price = $("#NewMoney").find('option:selected').attr("data-price");
        price = accounting.formatNumber(price)
        price = price.replace(",", ".");
        price = price.replace(",", ".");
        $("#NhuanBut").val(price);
        $("#showMoney").html(price);
    }
}