$(document).ready(function () {
    $('#NhuanBut').on('keyup', function () {
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
    var price = $("#NewMoney").find('option:selected').attr("data-price");
    price = accounting.formatNumber(price)
    var count = price.length;
    var mumber = count / 3;
    for (var i = 0; i <= mumber; i++) {
        price = price.replace(",", ".");
    }
    $("#NhuanBut").val(price);
    $("#showMoney").html(price);
    // check tieu de
    $('#Title').on('change', function () {
        var author = $('#Title').val();
        var pattern = new RegExp(/[<>]/);
        if (pattern.test(author)) {
            alertmsg.error("Tiêu đề có chứa ký tự đặc biệt !");
            $('#Title').css('border-color', 'red')
            $('#validate').hide();
            $('#vali').hide();
            return false;
        }
        else {
            $('#Title').css('border-color', '#eee')
            $('#validate').show();
            $('#vali').show();
            return true;
        }
    })
    // lock
    $('.lockChar').on("keypress", function () {
        return event.charCode >= 48 && event.charCode <= 57
    })
    // luu va them tiep
    $('#validate').on('click',
        function() {
            $('#isContinue').val("1");
            var photo = $("#Photo").val();
            if (photo == "") {
                alertmsg.error("Bạn chưa chọn ảnh")
                return false;
            }
        });
    // check publish 
    $('#publish').on('click', function () {
        $('#isPublish').val("1");
        var photo = $("#Photo").val();
        var money = $('#NewMoney').val();
        if (photo == "") {
            alertmsg.error("Bạn chưa chọn ảnh")
            return false;
        }

        if (money == "-- Chọn --") {
            $('#typemoney').html('<span style="color:red" id="loaitin">Bạn chưa chọn loại tin</span>');
            return false;
        } else {
            $('#loaitin').remove();
            return true;
        }
    })
     //validate 
    $('#vali').on('click', function () {
        var photo = $("#Photo").val();
        var money = $('#NewMoney').val();
        if (photo == "") {
            alertmsg.error("Bạn chưa chọn ảnh")
            return false;
        }
        if (money == "-- Chọn --") {
            $('#typemoney').html('<span style="color:red" id="loaitin">Bạn chưa chọn loại tin</span>');
            return false;
        } else {
            $('#loaitin').remove();
            return true;
        }
    })
    //ảnh đại diện
    
    // duyet tin
    $('#duyettin').on('click', function () {
        //$('#isDuyet').val("1");
        //var photo = $("#Photo").val();
        //var money = $('#NewMoney').val();
        //if (photo == "") {
        //    alertmsg.error("Bạn chưa chọn ảnh")
        //    return false;
        //}
        //if (money == "-- Chọn --") {
        //    $('#typemoney').html('<span style="color:red" id="loaitin">Bạn chưa chọn loại tin</span>');
        //    return false;
        //} else {
        //    $('#loaitin').remove();
        //    return true;
        //}

    })
    
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
    //var selectid = $("#NewMoney").val();
    //price = accounting.formatNumber(price)
    //price = price.replace(",", ".");
    //$("#NhuanBut").val(0);
    //$("#showMoney").html(0);
    //if (selectid > 0) {
    //    $('#loaitin').remove();
    //    var price = $("#NewMoney").find('option:selected').attr("data-price");
    //    price = accounting.formatNumber(price)
    //    price = price.replace(",", ".");
    //    $("#NhuanBut").val(price);
    //    $("#showMoney").html(price);
    //}
}
//check linh tinh
//var CheckValidate = function () {
//    var photo = $("#Photo").val();
//    var money = $('#NewMoney').val();
//    if (photo == "") {
//        alertmsg.error("Bạn chưa chọn ảnh")
//        return false;
//    }
//    if (money == "-- Chọn --") {
//        $('#typemoney').html('<span style="color:red" id="loaitin">Bạn chưa chọn loại tin</span>');
//        return false;
//    } else {
//        $('#loaitin').remove();
//        return true;
//    }
//}