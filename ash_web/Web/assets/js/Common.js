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
                1199: {
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
            if ($(this).scrollTop() >= 800) { $('#bttop').fadeIn(); }
            else { $('#bttop').fadeOut(); }
        });
        $('#bttop').click(function () {
            event.preventDefault();
            $('body,html').animate({ scrollTop: 0 }, 1600)
            ;
        })
        ;
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

    $('#new .newest .right').mCustomScrollbar();
    $('.btnmenures').click(function() {
        $('#menu').toggleClass("active");
        $('#overlay').fadeToggle();
    });
    $('#overlay').click(function () {
        $('#menu').toggleClass("active");
        $('#overlay').fadeToggle();
    });

    var hhead = $('#header .top').innerHeight();
    $('.btnmenures').innerHeight(hhead);
    $('.btnmenures').innerWidth(hhead);
});
/*end Phần cố định*/ 

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
    $('.btnmenures').innerWidth(hhead);
});

$(document).ready(function () { 
    $(window).resize(function() {
        resizearr(4, 3, 1, '#new .newest .list', '.item');
        resizearr(1, 1, 1, '#thuvienanh .group', '.item');
    }); 
    $('#new .newest .list').owlCarousel(owlslide(40, 20, true, false, true, 2, 3, 3, 4, 4));
    resizearr(4, 3, 2, '#new.hp .group', '.item'); 
    $('#thuvienanh .group').owlCarousel(owlslide(1, 10, true, false, true, 1, 1, 1, 1, 1));
    resizearr(1, 1, 1, '#thuvienanh .group', '.item');
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

 
  