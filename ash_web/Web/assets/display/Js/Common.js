$(window).load(function () {
    /*Phần khung ảnh*/
    CropImage(); 
    $('.dotdotdot').dotdotdot();
});


/*Phần cố định*/
/*Hàm cho Owlcarousel*/
function owlslide(num, margin, autoplay, dot, nav, mobile, mobilel, tablet, tabletl, pc) {
    var option;
    if (num > 1) {
        option = {
            items: num,
            autoplay: autoplay,
            autoplayTimeout: 5000,
            smartSpeed: 1500,
            loop: true,
            nav: nav,
            dots: dot,
            autoplayHoverPause: true,
            margin: margin,
            navText: [''],
            responsive: {
                0: {
                    items: mobile,
                    margin: margin
                },
                479: {
                    items: mobilel,
                    margin: margin
                },
                767: {
                    items: tablet,
                    margin: margin
                },
                991: {
                    items: tabletl,
                    margin: margin
                },
                1199: {
                    items: pc,
                    margin: margin
                }
            }
        }
    } else {
        option = {
            items: num,
            autoplay: autoplay,
            autoplayTimeout: 5000,
            smartSpeed: 1500,
            nav: nav,
            dots: dot,
            autoplayHoverPause: true,
            margin: margin,
            navText: [''],
            responsive: {
                0: {
                    items: mobile,
                    margin: margin
                },
                479: {
                    items: mobilel,
                    margin: margin
                },
                767: {
                    items: tablet,
                    margin: margin
                },
                991: {
                    items: tabletl,
                    margin: margin
                },
                1023: {
                    items: pc,
                    margin: margin
                },

            }
        }
    }
    return option;
}
function resizearr(num1199, numother, num478, namediv, namechild) {
    var name = $(namediv);
    if ($(window).width() > 1199) {
        if (name.find(namechild).size() < num1199) {
            name.find('.owl-nav').hide();
        } else {
            name.find('.owl-nav').show();
        }
    } else if ($(window).width() < 478) {
        if (name.find(namechild).size() < num478) {
            name.find('.owl-nav').hide();
        } else {
            name.find('.owl-nav').show();
        }
    }
    else {
        if (name.find(namechild).size() < numother) {
            name.find('.owl-nav').hide();
        } else {
            name.find('.owl-nav').show();
        }
    }
}
/*Hàm cho Owlcarousel*/
$(document).ready(function () { 
    $(function () {
        $(window).scroll(function () {
            var windowScrollTop = $(this).scrollTop();
            if (windowScrollTop >= 800) { $('#bttop').fadeIn(); }
            else { $('#bttop').fadeOut(); }

            
            var topcontainer = $('#container').offset().top;
            if (windowScrollTop > topcontainer) {
                $('.col-fix-left').css("top", windowScrollTop - topcontainer);
                $('.col-fix-right').css("top", windowScrollTop - topcontainer);
            } else {
                $('.col-fix-left').css("top", 0);
                $('.col-fix-right').css("top", 0);
            }

        });
        $('#bttop').click(function () {
            event.preventDefault();
            $('body,html').animate({ scrollTop: 0 }, 1600)
            ;
        }); 
    });
     
    $(function () {
        $("img").lazy({
            effect: "fadeIn",
            effectTime: 1000,
            afterLoad: function (element) {
                CropImage();
            },
            placeholder: "data:css/banner/loader.gif"
        });
    });

    $('#mygallery').justifiedGallery({
        rowHeight: 224,
        margins: 10,
        lastRow: 'nojustify'
    });
    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() == $(document).height()) {
            for (var i = 0; i < 5; i++) {
                $('#gallery').append('<a>' +
                    '<img src="http://path/to/image" />' +
                    '</a>');
            }
            $('#gallery').justifiedGallery('norewind');
        }
    });

    var hhead = $('#header .top').innerHeight();
    $('.btnmenures').innerHeight(hhead);
    //$('.btnmenures').innerWidth(hhead);

    if ($(window).innerWidth() > 767) {
        $('#new .newest .right').mCustomScrollbar();
        var hleft = $('#new .newest .left').innerHeight();
        $('#new .newest .right').innerHeight(hleft);

    } else {
        $('#new .newest .right').mCustomScrollbar('destroy');
        $('#new .newest .right').innerHeight('auto');
    }

    $('.btnmenures').click(function() {
        $('#menu').toggleClass("active");
        $('#menu').css("top", hhead);
        $('#overlay').fadeToggle();
        $('.btnmenures').toggleClass("active");
        $('#header .top').toggleClass("active");
    });
    $('#overlay').click(function () {
        $('#menu').toggleClass("active");
        $('#overlay').fadeToggle();
        $('.btnmenures').toggleClass("active");
        $('#header .top').toggleClass("active");
    });

    if ($(window).innerWidth() < 992) {
        var hkapta = (66.66 / 100) * $('.wrp').innerWidth();
        $('#lightgallery').height(hkapta);
    } else {
        $('#lightgallery').height('auto');
    }

    if ($(window).innerWidth() > 1023) {
        var wwrp = $('.wrp').innerWidth();
        wwrp = ((wwrp * 70) / 100 * 9) / 16;
        var hleft1 = $('#video .top .left').innerHeight();
        $('#video .top .right').innerHeight(hleft1 + wwrp);
        $('#video .top .right').mCustomScrollbar();
        $('#video .top .right').fadeIn();
    } else { 
        $('#video .top .right').fadeIn();
    }

    $('.noidung img').attr("data-lightbox", "roadtrip");

    var countimg = 0;
    $('.noidung img').each(function () {
        var src = $(this).attr("src");
        if (!$(this).parent().is("a")) { 
            $('<a class="example-image-link countimg' +countimg +'" href="' +src +'" data-lightbox="example-1"></a>').insertBefore($(this));
            $('.countimg' + countimg).append($(this));
        } else {
            var href = $(this).parent("a").attr("href");
            if (href === src)
                $(this).parent("a").attr({
                    "class": "example-image-link countimg" + countimg,
                    "data-lightbox": "example-1"
                });
        }
        countimg++;
    });

    lightbox.option({
        'resizeDuration': 200,
        'wrapAround': true
    });
      
});
/*end Phần cố định*/ 

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email.toLowerCase());
}

function CropImage() {
    $(".khungAnhCrop img").each(function () {
        $(this).removeClass("wide tall").addClass((this.width / this.height > $(this).parent().width() / $(this).parent().height()) ? "wide" : "tall");
        $(this).fadeIn();
    });
}

$(window).resize(function () {
    $('.dotdotdot').height('auto');
    $('.dotdotdot').dotdotdot();
    var hhead = $('#header .top').innerHeight();
    $('.btnmenures').innerHeight(hhead);
    //$('.btnmenures').innerWidth(hhead);
     

    if ($(window).innerWidth() > 767) {
        $('#new .newest .right').mCustomScrollbar();
        var hleft = $('#new .newest .left').innerHeight();
        $('#new .newest .right').innerHeight(hleft);
       
    } else {
        $('#new .newest .right').mCustomScrollbar('destroy'); 
        $('#new .newest .right').innerHeight('auto');
    } 

    if ($(window).innerWidth() < 992) {
        var hkapta = (66.66 / 100) * $('.wrp').innerWidth();
        $('#lightgallery').height(hkapta);
    } else {
        $('#lightgallery').height('auto');
    } 
});

$(document).ready(function () { 
    $(window).resize(function() {
        resizearr(4, 3, 1, '#new .newest .list', '.item');
        resizearr(1, 1, 1, '#thuvienanh .group', '.item');
    }); 
    $('#new .newest .list').owlCarousel(owlslide(6, 20, true, false, true, 2, 3, 3, 4, 4));
    resizearr(4, 3, 2, '#new.hp .group', '.item'); 
    $('#thuvienanh .group').owlCarousel(owlslide(1, 10, true, false, true, 1, 1, 1, 1, 1));
    resizearr(1, 1, 1, '#thuvienanh .group', '.item');

    if ($('#topic .slidetopic .khungAnh').size() > 1) {
        $('#topic .slidetopic').owlCarousel({
            items: 1,
            loop: true,
            nav: true,
            navText: [""],
            animateIn: "fadeIn",
            animateOut: "fadeOut",
            autoplay: true,
            autoplayTimeout: 4000,
            smartSpeed:1500
    });
    }

    if ($('#truyenhinhright .group .item').size() > 1) {
        $('#truyenhinhright .group').owlCarousel({
            items: 1,
            loop: true,
            nav: true,
            navText: [""], 
            autoplay: true,
            autoplayTimeout: 4000,
            smartSpeed:1500
    });
    }
}); 
 

var size = parseInt(jQuery(".TextSize").css("font-size"));
var lineheight = parseInt(jQuery(".TextSize").css("line-height"));
if (!size)
    size = 14;
if (!lineheight)
    lineheight = 22;
function IncreaseTextSize() {
    size++;
    lineheight += 2;

    jQuery(".TextSize")
        .css('cssText',
            'font-size:' +
            size +
            'px !important; line-height:' +
            lineheight +
            'px !important');
    jQuery(".TextSize")
        .find("*")
        .css('cssText',
            'font-size:' +
            size +
            'px !important; line-height:' +
            lineheight +
            'px !important');
} 
function DecreaseTextSize() {
    size--;
    lineheight -= 2;

    jQuery(".TextSize")
        .css('cssText',
            'font-size:' +
            size +
            'px !important; line-height:' +
            lineheight +
            'px !important');
    jQuery(".TextSize")
        .find("*")
        .css('cssText',
            'font-size:' +
            size +
            'px !important; line-height:' +
            lineheight +
            'px !important');
} 
function ResetTextSize() {
    size = 14;
    lineheight = 22;

    jQuery(".TextSize")
        .css('cssText',
            'font-size:' +
            size +
            'px !important; line-height:' +
            lineheight +
            'px !important');
    jQuery(".TextSize")
        .find("*")
        .css('cssText',
            'font-size:' +
            size +
            'px !important; line-height:' +
            lineheight +
            'px !important');
}

$(".select2").select2();
$("#newdetail table").each(function (index) {
    var w150 = $(this).attr("style");
    if (w150 == "width:150px") {
        $(this).attr("style","width: 100%;");
    }
});