

$(window).load(function () {
    /*Phần khung ảnh*/
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
            lazyLoad: true,
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
            lazyLoad: true,
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
            if (windowScrollTop >= 800) {
                $('#bttop').fadeIn();
            } else {
                $('#bttop').fadeOut();
            }
        });
        $('#bttop').click(function () {
            event.preventDefault();
            $('body,html').animate({ scrollTop: 0 }, 1600);
        });
    });

    $(function () {
        $("img").lazy({
            effect: "fadeIn",
            effectTime: 1000,
            placeholder: "../css/icon/logo.png"
        });

        $('.noidung img').attr("data-lightbox", "roadtrip");

        var countimg = 0;
        $('.noidung img').each(function () {
            var src = $(this).attr("src");
            if (!$(this).parent().is("a")) {
                $('<a class="example-image-link countimg' +
                    countimg +
                    '" href="' +
                    src +
                    '" data-lightbox="example-1"></a>').insertBefore($(this));
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

    if ($('#slide .wImage').size() > 1) {
        $('#slide').owlCarousel({
            items: 1,
            loop: true,
            animateIn: 'fadeIn',
            animateOut: 'fadeOut',
            autoplay: true,
            autoplayTimeout: 5000,
            dots: true,
            lazyLoad: true,
            autoplayHoverPause: true,
            smartSpeed: 1500
        });
    }

    if ($('#news.hp .group-right .parent .group').size() > 1) {
        $('#news.hp .group-right .parent').slick({
            infinite: true,
            slidesToShow: 1,
            slidesToScroll: 1,
            dots: true,
            lazyLoad: 'ondemand',
            speed: 1000
        });
    }


    //$('#product.detail .imgLarge').slick({
    //    slidesToShow: 1,
    //    slidesToScroll: 1,
    //    arrows: false,
    //    fade: true,
    //    asNavFor: '.imgSmall',
    //    lazyLoad: 'ondemand',
    //});
    //$('#product.detail .imgSmall').slick({
    //    slidesToShow: 6,
    //    slidesToScroll: 1,
    //    arrows: false,
    //    asNavFor: '.imgLarge',
    //    dots: false,
    //    centerMode: false,
    //    focusOnSelect: true,
    //    lazyLoad: 'ondemand',
    //    responsive: [
    //        {
    //            breakpoint: 1199,
    //            settings: {
    //                slidesToShow: 4,
    //            }
    //        },
    //        {
    //            breakpoint: 991,
    //            settings: {
    //                slidesToShow: 3,
    //            }
    //        },
    //        {
    //            breakpoint: 767,
    //            settings: {
    //                slidesToShow: 3,
    //            }
    //        },
    //        {
    //            breakpoint: 479,
    //            settings: {
    //                slidesToShow: 2,
    //            }
    //        }
    //    ]
    //}); 


});
/*end Phần cố định*/


//Bắt định dạng email
function validateEmail(email) {
    var re =
        /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email.toLowerCase());
}


$(window).resize(function () {
    $('.dotdotdot').height('auto');
    $('.dotdotdot').dotdotdot();
});
$(document).ready(function () {
    $('#csr.hp .row.logo .parent').owlCarousel(owlslide($('#csr.hp .row.logo .parent .group').size(), 20, false, true, true, 2, 3, 3, 4, 5));
    $('#csr.hp .row.video .parent .group').owlCarousel(owlslide($('#csr.hp .row.video .parent .group .item').size(), 30, false, true, true, 1, 2, 2, 2, 3));
    $('#csr.hp .row.ykienkhachhang .parent .group').owlCarousel(owlslide($('#csr.hp .row.ykienkhachhang .parent .group .item').size(), 27, false, true, false, 1, 1, 1, 2, 2));
    $('#service.hp .group').owlCarousel(owlslide($('#service.hp .group .item').size(), 30, false, true, false, 1, 1, 1, 2, 2));
    $('#doingunhansu .parent').owlCarousel(owlslide($('#doingunhansu .parent .group').size(), 80, false, true, true, 2, 3, 3, 4, 5));
    $('#hoatdonggiaitri .parent').owlCarousel(owlslide($('#hoatdonggiaitri .parent .group').size(), 30, false, true, true, 1, 2, 2, 3, 4));
    $('#showroom.hp .group').owlCarousel(owlslide($('#showroom.hp .group .item').size(), 30, false, true, true, 1, 2, 3, 3, 4));
    $('.relatedNews.pro .parent').owlCarousel(owlslide($('.relatedNews.pro .parent .group').size(), 20, false, true, true, 1, 1, 1, 1, 1));
    $('#news.list .other .group').owlCarousel(owlslide($('#news.list .other .group .item1').size(), 30, false, true, true, 1, 2, 2, 2, 3));
    $('#csr.details .row.video .other .group').owlCarousel(owlslide($('#csr.details .row.video .other .group .item').size(), 30, false, false, true, 1, 2, 2, 2, 3));
    $('#product.details .other .group').owlCarousel(owlslide($('#product.details .other .group .item').size(), 0, false, false, true, 2, 3, 3, 3, 4));

    // Select2
    if ($('body').find('.select2').size() > 0) $('.select2').select2();

    // Datetime Picker
    if ($('body').find('.datetimepicker').size() > 0) {
        $('.datetimepicker').datetimepicker({
            lang: 'vi',
            timepicker: false,
            format: 'd/m/Y',
        });
        $.datetimepicker.setLocale('vi');
    }

    $('#menu .openList').click(function () {
        $('#menu').toggleClass('ac');
        $('#menu .openList').toggleClass('ac');
        $('#overlay').fadeToggle();
    });
    $('#overlay').click(function () {
        $('#menu').toggleClass('ac');
        $('#menu .openList').toggleClass('ac');
        $('#overlay').fadeToggle();
    });

    $('#menu .openSub').click(function () {
        $(this).next('ul').slideToggle();
        $(this).toggleClass('ac');
    });

    $('.btnsearchOut').click(function () {
        $(this).prev('.iptsearch').addClass('ac');
    });

    $('.btncloseSearch').click(function () {
        $(this).parent('.iptsearch').removeClass('ac');
    });

    // Tab
    jQuery('.tabs .tab-links a').on('click',
        function (e) {
            var currentAttrValue = jQuery(this).attr('href');
            jQuery('.tabs ' + currentAttrValue).fadeIn(400).siblings().hide();
            jQuery(this).parent('li').addClass('active').siblings().removeClass('active');
            e.preventDefault();
        });

    $('.popup-gmaps').magnificPopup({
        type: 'iframe'
        // other options
    });


    $('#product.details .imgLarge').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: true,
        asNavFor: '.imgSmall',
        lazyLoad: 'ondemand',
    });
    $('#product.details .imgSmall').slick({
        slidesToShow: 4,
        slidesToScroll: 1,
        arrows: false,
        asNavFor: '.imgLarge',
        dots: false,
        centerMode: false,
        focusOnSelect: true,
        lazyLoad: 'ondemand',
        responsive: [
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 4,
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 479,
                settings: {
                    slidesToShow: 2,
                }
            }
        ]
    });

    $('.arrow-left').on('click', function () {
        window.clearTimeout(timeoutHandle);
        $(".text-slider").fadeIn("slow");
        showPrevSlides();
    });
    $('.arrow-right').on('click', function () {
        window.clearTimeout(timeoutHandle);
        $(".text-slider").fadeIn("slow");
        showNextSlides();
    });

    $('.text-search').on('click', function () {
        debugger;
        $('#overlay-search').fadeToggle();
        $('#search-box').fadeToggle();
        $('body').css('overflow-y','hidden');
    })
    $('#overlay-search').on('click', function () {
        debugger;
        $('#overlay-search').fadeToggle();
        $('#search-box').fadeToggle();
        $('body').css('overflow-y','scroll');
    });

    //branch-featured
    $('#branch-featured .list-tabs ul li').on('click',function(e){
        $('#branch-featured .list-tabs ul li').each(function (item,i ) {
            $(this).removeClass('active');
        })
        $(this).addClass('active');


    });
    $('#branch-featured .list-product .product-tab .product-item .list-images .show-image').on('mouseenter',function(e){
        var index = e.target.dataset.index;
        debugger;
        var imageHover = $('#branch-featured .list-product .product-tab .product-item .list-images .show-image-'+ index).data('src-hover');
        $('#branch-featured .list-product .product-tab .product-item .list-images .show-image-' + index).attr('src', imageHover)


    });
    $('#branch-featured .list-product .product-tab .product-item .list-images .show-image').on('mouseleave',function(e){
        var index = e.target.dataset.index;
        debugger;
        var image = $('#branch-featured .list-product .product-tab .product-item .list-images .show-image-'+ index).data('src-'+ index);
        $('#branch-featured .list-product .product-tab .product-item .list-images .show-image-' + index).attr('src', image)


    });
});

//Tăng giảm cỡ chữ
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
//Slide header
var slideIndex = 0;
var timeoutHandle;
showSlides();
function showSlides() {
    var i;
    var slides = document.getElementsByClassName("text-slider");
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    slideIndex++;
    if (slideIndex > slides.length) { slideIndex = 1 }
    slides[slideIndex - 1].style.display = "block";

    timeoutHandle = setTimeout(showSlides, 4000); // Change image every 2 seconds
};
function showNextSlides() {
    var i;
    var slides = document.getElementsByClassName("text-slider");
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    slideIndex++;
    if (slideIndex > slides.length) { slideIndex = 1 }
    slides[slideIndex - 1].style.display = "block";
    timeoutHandle = setTimeout(showSlides, 4000); // Change image every 2 seconds
};
function showPrevSlides() {
    var i;
    var slides = document.getElementsByClassName("text-slider");
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    slideIndex--;
    if (slideIndex <= 0) { slideIndex = slides.length }
    slides[slideIndex - 1].style.display = "block";
    timeoutHandle = setTimeout(showSlides, 4000); // Change image every 2 seconds
};
