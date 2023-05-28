var languages = function () {
    return {
        init: function () {
            this.FirstLoad = true;
        },
        loadData: function () {
            var self = this;
            $("#loading").show();
            AjaxService.POST("/languages/ListData", null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        onchangeLanguges: function (languages) {
            if (languages == null || languages == undefined)
                languages == "vn";
           
            $.ajax({
                url: "/Languages/ChangeLanguages",
                data: { languages: languages },
                dataType: "json",
                type: "get",
                success: function (res) {
                    if (res.IsSuccess) {
                        window.location.reload();
                    }
                }
            });
        },
        changeLangugesHome: function (languages) {
            if (languages == null || languages == undefined)
                languages == "vn";

            $.ajax({
                url: "/Home/ChangeLanguage",
                data: { languages: languages },
                dataType: "json",
                type: "get",
                success: function (res) {
                    if (res.IsSuccess) {
                        window.location.href = "/";
                    }
                }
            });
        },
    };
}();
$(function () { languages.init(); });


