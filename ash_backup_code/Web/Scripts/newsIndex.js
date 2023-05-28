$(document).ready(function () {
    $('.datepicker-v').datepicker({
        format: 'dd/mm/yyyy',
        mask: '39/19/9999',
        autoclose: true,
    });
    // tim kiem nang cao
    $('#show').on('click', function () {
        $("#myContent").toggle("slow");
    })
    //thong ke
    var thongke = $('#thongke').val();
    if (thongke == 1) {
        $('#btnAdd').hide();
        $('#btnDeleteMulti').hide();
    }
    //check ngay thang
    $('#NgayKet').on("change", function () {
        var datePost = $('#NgayDang').val();
        var dateFinish = $('#NgayKet').val();
        var d1 = Convertdate(datePost);
        var d2 = Convertdate(dateFinish);
        if (d1 > d2) {
            alertmsg.error("Ngày đăng phải nhỏ hơn ngày kết thúc");
            $('#NgayKet').val("");
            return false;
        } else {
            return true;
        }
    })
    $('#NgayDang').on("change", function () {
        var datePost = $('#NgayDang').val();
        var dateFinish = $('#NgayKet').val();
        var d1 = Convertdate(datePost);
        var d2 = Convertdate(dateFinish);
        if (d1 > d2) {
            alertmsg.error("Ngày đăng phải nhỏ hơn ngày kết thúc");
            $('#NgayKet').val("");
            return false;
        } else {
            return true;
        }
    })
    //
})
var Convertdate= function(strdate) {
    var day = parseInt(strdate.substr(0, 2));
    var month = parseInt(strdate.substr(3, 2));
    var year = parseInt(strdate.substr(6, 4));
    var ipday = new Date(year, month, day);
    return ipday;
}