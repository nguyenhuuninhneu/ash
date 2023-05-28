var confignote = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
            } else {
                alertmsg.error(res.Messenger);
            }
        },
    }
}();
$(function () { confignote.init(); });


