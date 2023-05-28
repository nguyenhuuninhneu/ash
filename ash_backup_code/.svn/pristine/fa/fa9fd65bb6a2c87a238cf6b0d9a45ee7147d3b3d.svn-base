 
var audio = new Audio('/assets/display/Css/soundchat.mp3');
var title = $("title");
var sl = parseInt(($(window).innerWidth() * 80 / 100) / 290);
if (sl > 10) sl = 10;
var chat = function () {
    return {
        //Hiện cửa sổ chat
        openchat: function (id, id1, fullname, fullname1, boolCountUp, boolSetCookie) {
            //debugger;
            var classchat = '.chatonline.' + id1 + '.' + id;
            $('.chatonline').removeClass('active');
            if ($('body').find(classchat).size() < 1) {
                AjaxService.POST("/Admin/Home/Chat",
                    { id: id1 },
                    function(res1) {
                        debugger;
                        var valueUser = fullname + "|||" + id + "|||" + classchat + "|||" + id1 + "|||" + fullname1;
                        if (boolSetCookie != false) {
                            var cookieId = chat.setcookie(valueUser);
                            cookieId += valueUser + ",";
                            $.cookie("listid", cookieId);
                        }

                        if (boolCountUp != false) {
                            countwindow++;
                        }

                        if (countwindow > sl) {
                            $('#listchat .listchathide').show();
                            var objFirstChat = $.cookie("listid").split(",")[countwindow - sl - 1];
                            var objFirstChat_value = objFirstChat.split("|||");
                            var objFirstChat_name1 = objFirstChat_value[0];
                            var objFirstChat_id = objFirstChat_value[1];
                            var objFirstChat_id1 = objFirstChat_value[3];
                            var objFirstChat_fullname1 = objFirstChat_value[4];
                            chat.closechat(objFirstChat_id, objFirstChat_id1, false);
                            chat.openchatinlist(objFirstChat_id,
                                objFirstChat_id1,
                                objFirstChat_name1,
                                objFirstChat_fullname1);
                        } else {
                            $('#listchat .listchathide').hide();
                        }

                        $('body').append('<input type="hidden" id="displayname_' +id1 +'" value="' +valueUser +'" />');

                        $('#listchat .divchatshow').append(res1.viewContent);
                        var date = $(classchat).find('.timecheck').val();
                        $(classchat).addClass('active');
                        chat.loadData(id, id1, date);

                        $(classchat).find(".message").emojioneArea({
                            autoHideFilters: true,
                            shortnames: true,
                            searchPlaceholder: "Tìm kiếm.."
                        });
                    });
            } else {
                $(classchat).addClass('active');
            } 
        },

        //Đóng cửa sổ chat
        closechat: function (id, id1, boolCountDown) {
            debugger;
            var classchat = '.chatonline.' + id + '.' + id1;
            $(classchat).remove();
            if (boolCountDown != false) { 
                countwindow--;
                var value = $('#displayname_' + id1).val();
                var cookieId = chat.setcookie(value);
                $.cookie("listid", cookieId);
                if ($('#listchat .divchatshow .chatonline').size() < sl && $('#listchat .listchathide ul li').size() > 0) {
                    $('#listchat .listchathide ul li:first-child div a.open').click(); 
                    $('#listchat .listchathide ul li:last-child').remove(); 
                }
            }
            $('#displayname_' + id1).remove(); 
        },

        //Thiết lập cookie lưu lại những cửa sổ chat được mở
        setcookie: function (value) { 
            if ($.cookie("listid") != null && $.cookie("listid") != "") {
                var arrFile = $.cookie("listid").split(",");
                arrFile = jQuery.grep(arrFile,
                    function(a) {
                        return a != value;
                    });
                var strFile = arrFile.join(',');
                return strFile;
            } else {
                return "";
            }
        },

        //Mở các cửa sổ chat đã được lưu trong cookie khi load lại trang
        loadChat: function (value) {
            if (value != null && value != "") {
                var arrFile = value.split(",");
                for (var i = 0; i < arrFile.length; i++) {
                    if (arrFile[i] != "" && arrFile[i] != null) {
                        var name1 = arrFile[i].split('|||')[0];
                        var id = arrFile[i].split('|||')[1];
                        var id1 = arrFile[i].split('|||')[3];
                        var fullname1 = arrFile[i].split('|||')[4];
                        chat.openchat(id, id1, name1, fullname1,true,true);
                    }
                }
            } 
        },

        //Load dữ liệu
        loadData: function (id, id1, date, scrollPos, hbefore) {
            var classchat = '.chatonline.' + id1 + '.' + id;
            AjaxService.POST("/Admin/Home/AjaxChat", { id: id, id1: id1, date: date }, function (res) {
                debugger;
                if (res.check == true) {
                    if ($(classchat + ' .discussion').find('li').size() <= 0)
                        $(classchat + ' .discussion').html(res.viewContent);
                    else {
                        $(classchat + ' .discussion li').removeClass('first');
                        $(classchat + ' .discussion li:first-child').addClass('first');
                        $(classchat + ' .discussion li.first').before(res.viewContent);
                    }
                    $(classchat).find('.timecheck').val(res.timecheck);
                    if (scrollPos == "top") {  
                        var hafter = $('#chat_' + id1).prop("scrollHeight"); 
                        $('#chat_' + id1).scrollTop(hafter - hbefore - 100);
                    } else { 
                        updateScroll('#chat_' + id1);
                        AjaxService.POST("/Admin/Home/UpdateCountChat",
                            { id: id, id1: id1 },
                            function (res) {
                                $('#countchat_' + id + id1).fadeOut();
                            });
                        if ($(classchat + ' .discussion').scrollTop() > 0) return;
                        chat.loadData(id, id1, res.timecheck); 
                    } 
                } else { 
                    $(classchat + ' .discussion').addClass('lastMes');
                    return; 
                }
            }); 
        },

        //Ẩn nội khung nội dung chat
        headerclick: function (id, id1) {
            debugger;
            var classchat = '.chatonline.' + id + '.' + id1;
            $(classchat).find('.panel-body').slideToggle();
            updateScroll('#chat_' + id);
        },

        //Hiện nội chat cho phía người nhận
        messenger: function (currentId) {
            var chat = $.connection.chatHub;
            chat.client.addNewMessageToPage = function (name, message, count, user1,user2) {
                debugger;
                var name1 = name.split('|||')[0];
                var id = name.split('|||')[1];
                var classchat = name.split('|||')[2];
                var id1 = name.split('|||')[3];
                var fullname1 = name.split('|||')[4];
                var time = name.split('|||')[5];
                var img = "/Content/Admin/images/staff.png";  
                openchat(id1, id, fullname1, currentId, name1, count);
                if ($('body').find(classchat).size() > 0) {
                    if (id === currentId) {
                        if (user1 != null && user1 != "") img = user1;
                        $(classchat + ' .discussion').append('<li class="clearfix odd"><div class="chat-avatar"><img src="' + img + '" alt="male" style="max-width:35px"></div><div class="conversation-text"><div class="ctext-wrap"><p>' + message + '<i class="time">' + time + '</i></p></div></div></li>');
                    } else {
                        if (!$(classchat).find('.emojionearea').hasClass('focused')) {
                            $(classchat).addClass('new');
                            $(classchat).find('.countchat').html(count);
                            $(classchat).find('.countchat').fadeIn();
                            $("title").html("Bạn có tin nhắn mới từ " + fullname1);
                            audio.play();
                            checkonlineuser();
                        } else {
                            AjaxService.POST("/Admin/Home/UpdateCountChat",
                                { id: id, id1: id1 },
                                function (res) {
                                    $(classchat).find('.countchat').hide();
                                    $("title").html(title);
                                });
                        }
                        if (user2 != null && user1 != "") img = user1;
                        $(classchat + ' .discussion').append('<li class="clearfix"><div class="chat-avatar"><img src="' + img + '" alt="male" style="max-width:35px"></div><div class="conversation-text"><div class="ctext-wrap"><p>' + message + '<i class="time">' + time + '</i></p></div></div></li>');
                    }

                    if ($('#chat_' + id).size() > 0) {
                        updateScroll('#chat_' + id);
                    } else updateScroll('#chat_' + id1);
                } 
            };
            $.connection.hub.start().done(function () {
                $('.sendmessage').click(function () {
                    debugger;
                    var id = $(this).attr("data-id"); 
                    var message = $(this).parent().prev().find('.emojionearea-editor').html();
                    if (message.trim() != "") {
                        var time = new Date().toLocaleString();
                        chat.server.send($('#displayname_' + id).val() + "|||" + time, message);
                    } 
                    $(this).parent().prev().find('.emojionearea-editor').html('');  
                }); 
            });
        },

        //Đưa cửa sổ chat vào list phụ
        openchatinlist: function (id, id1, fullname, fullname1) { 
            var onclick = "$(this).parents('li').remove();chat.openchat(" + id + "," + id1 + ",'" + fullname + "','" + fullname1 + "'," + false + "," + true + ")";
            var classchat = '.chatonline.' + id1 + '.' + id;
            var value = fullname + "|||" + id + "|||" + classchat + "|||" + id1 + "|||" + fullname1;
            var cookieId = chat.setcookie(value); 
            var closeobj = "$(this).parents('li').remove();" +
                "$.cookie('listid', '" + cookieId + "');" +
                "if ($('#listchat .listchathide ul li').size() == 0)" +
                "$('#listchat .listchathide').hide();";
            $('#listchat .listchathide ul').append('<li class="chatonline ' + id + ' ' + id1 + '"><div><a class="open" onclick="' + onclick + '">' + fullname1 + '</a><a onclick="' + closeobj + '"><i class="fa fa-times" aria-hidden="true"></i></a></div></li>');
        },

        //Thông tin người dùng
        loadfrmDetail: function (id) {
            modal.Render("/Admin/Home/InfoAcount/" + id, "Chi tiết người dùng", "modal-md");
        },

        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                chat.loadGroup();
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },

        loadGroup: function() {
            AjaxService.GET("/Admin/Home/GroupChat", null, function (res) {
                if (res.viewContent != null) {
                    $('#RightSlidebarGroup').html(res.viewContent);
                }
            });
        }
    };
}();

//Mở ô chat bên người nhận tin nhắn nếu chưa mở
function openchat(id, id1, fullname, currentId, name1, count) {
    debugger;
    var classchat = '.chatonline.' + id1 + '.' + id;
    if ($('body').find(classchat).size() < 1) {
        AjaxService.POST("/Admin/Home/CheckUser", null, function (res) {
            currentId = res.id;
        });
        if (id === currentId) {
            AjaxService.POST("/Admin/Home/Chat", { id: id1 },
                function (res1) {
                    debugger;
                    $('#listchat .divchatshow').append(res1.viewContent);
                    var date = $(classchat).find('.timecheck').val();
                    AjaxService.POST("/Admin/Home/AjaxChat", { id: id, id1: id1, date: date }, function (res) {
                        var valueUser = fullname + "|||" + id + "|||" + classchat + "|||" + id1 + "|||" + name1;
                        var cookieId = chat.setcookie(valueUser);
                        cookieId += valueUser + ",";
                        $.cookie("listid", cookieId);
                        countwindow++;
                        if (countwindow > sl) {
                            var objFirstChat = $.cookie("listid").split(",")[countwindow - sl - 1];
                            var objFirstChat_value = objFirstChat.split("|||");
                            var objFirstChat_name1 = objFirstChat_value[0];
                            var objFirstChat_id = objFirstChat_value[1];
                            var objFirstChat_id1 = objFirstChat_value[3];
                            var objFirstChat_fullname1 = objFirstChat_value[4];
                            chat.closechat(objFirstChat_id, objFirstChat_id1, false);
                            chat.openchatinlist(objFirstChat_id, objFirstChat_id1, objFirstChat_name1, objFirstChat_fullname1);
                        }

                        $('body').append('<input type="hidden" id="displayname_' + id1 + '" value="' + valueUser + '" />');

                        $(classchat).addClass('new');
                        $(classchat).addClass('newcount');
                        $(classchat).find('.countchat').show();
                        $(classchat).find('.countchat').html(count);
                        if (res.check == true) {
                            $(classchat + ' .discussion').html(res.viewContent);
                            $(classchat).find('.timecheck').val(res.timecheck);
                            updateScroll('#chat_' + id1); 
                        }
                    }); 
                    $(classchat).find(".message").emojioneArea({
                        autoHideFilters: true,
                        shortnames: true,
                        searchPlaceholder: "Tìm kiếm.."
                    });
                }); 
        }
    }
}

//Xóa text khi chat xong
$('.chat-inputbar').keydown(function (event) {
    var keypressed = event.keyCode || event.which;
    if (keypressed == 13) {
        $(this).next().find('.sendmessage').click();
    }
});
$('.chat-inputbar').keyup(function (event) {
    var keypressed = event.keyCode || event.which;
    if (keypressed == 13) {
        $(this).find('.emojionearea-editor').html('');
    } 
});
 
 //Lăn chuột xuống cuối cùng
function updateScroll(divId) {   
    var srollHeight = $(divId).prop("scrollHeight");
    $(divId).scrollTop(srollHeight);
}
 
//Cập nhật lại count
$('.chat-inputbar,.chatonline .panel-heading').click(function () {
    //debugger; 
    var $div = $(this).parents('.chatonline');
    $('.chatonline').removeClass('active');
    $div.addClass('active');
    var listid = $div.attr("class");
    var id = listid.split(' ')[1];
    var id1 = listid.split(' ')[2];
    if ($div.hasClass('new')) {
        AjaxService.POST("/Admin/Home/UpdateCountChat",
            { id: id, id1: id1 },
            function (res) {
                debugger;
                $div.removeClass('new');
                $('.chatonline').addClass('newcount');
                $div.removeClass('newcount');
                $div.find('.countchat').fadeOut();
                $("title").html(title);
                if ($('#countchat_' + id1 + id).size() > 0)
                    $('#countchat_' + id1 + id).hide(); 
            });
    }
    var valueUser = $('#displayname_' + id).val();
    var cookieId = chat.setcookie(valueUser);
    cookieId += valueUser + ",";
    $.cookie("listid", cookieId); 
    return;
});

//Ấn ESC để tắt cửa sổ chat
$('body').keyup(function (event) { 
    var keypressed = event.keyCode || event.which;
    if (keypressed == 27) {
        $('.chatonline.active').find('.closechat').click();
    }
});
 

//Load thêm dữ liệu khi lăn chuột lên đầu
$('.chatonline .discussion').scroll(function() {
    var $div = $(this).parents('.chatonline');
    var listid = $div.attr("class");
    var id1 = listid.split(' ')[1];
    var id = listid.split(' ')[2];
    var classchat = '.chatonline.' + id1 + '.' + id;
    var date = $(classchat).find('.timecheck').val();
    var hbefore = $(this).prop("scrollHeight"); 
    if ($(this).scrollTop() <= 0) {
        chat.loadData(id, id1, date, "top", hbefore);
    }
});

$('#listchat .listchathide').css("right", 280 * sl);
 
