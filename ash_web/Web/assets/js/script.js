// edit mobile menu
if ($(window).width() < 992) {
    $("#bs-example-navbar-collapse-1 li.dropdown>a").addClass("class","dropdown-toggle").attr("data-toggle","dropdown");
}
// end edit mobile menu

$(function () {
    $('#menu-btn').click(function(){
        $('.content-left').toggleClass('show-menu');
        $('#pagebody').addClass('move-body'); 
        $(".btn-common-close").show();
    })
});

function myFunction(){
	$('.content-left').removeClass('show-menu');
	$('#pagebody').removeClass('move-body'); 
	$(".btn-common-close").css("display","none");
	$('.menu-top > li > .menu-dropdown ').removeClass('active-menu');
}

jQuery(document).ready(function($) {
	var	$menu = $('#navmenu'),
			$menulink = $('.menu-link'),
			$menuTrigger = $('.has-subnav');


	$menuTrigger.click(function(e) {
		e.preventDefault();
		var $this = $(this);
		$this.toggleClass('active-menu').children('.menu-dropdown').toggleClass('active-menu');
		$this.toggleClass('active-menu').children('.menudropdown').toggleClass('active-menu');
		$this.toggleClass('activemenu').parent('.menu-dropdown').toggleClass('activemenu');
	});
});

$(function() {
     var pgurl = window.location.href.substr(window.location.href
.lastIndexOf("/")+1);
     $("#navmenu ul li ul li a").each(function(){
          if($(this).attr("href") == pgurl || $(this).attr("href") == '' )
          $(this).parent().addClass("activemenu");
     })
});

jQuery(document).ready(function($) {
	 $('.datetimepicker4').datetimepicker();
	 $('#datetimepicker7').datetimepicker();
});

$(document).ready(function() {
	$(".owl-carousel").owlCarousel({
	    responsiveClass:true,
        lazyLoad : false,
        dots : false,
        loop : true,
        nav  : true,
        margin :10,
        autoplayTimeout:5000,
        autoplay :true,
        navText : ['<i class="glyphicon glyphicon-chevron-left bottom-preview"></i>','<i class="glyphicon glyphicon-chevron-right bottom-next"></i>'],
        responsive:{
            0:{
                items:1,
            },
            568:{
                items:2,
            },
            600:{
                items:3,
            },
            1024:{
                items:3,
            },
            1200:{
                items:4,
            }
        }
	});
 });

jQuery(document).ready(function($) {
  $('#myTab a').click(function (e) {
  e.preventDefault()
  $(this).tab('show')
  })
});
$(window).scroll(function(){
    if($(window).scrollTop() > 90){ 
        $(".navbar-default").addClass("navbar-fixed-top");
        $('.navbar-default ').css('position','fixed');
    }else{ 
        $(".navbar-default").removeClass("navbar-fixed-top");
        $('.navbar-default ').css('position','relative');
    }
});
jQuery(document).ready(function($) {
	jQuery('#carousel2').carouFredSel({
		width: '100%',
		direction   : "up",
		scroll : {
		    duration: 2500,
		    pauseOnHover: true,
		    easing: "swing"
		},
		items: {
			visible: 3
		},
		auto: {
			items: 1,
			timeoutDuration : 1500
		},
		prev: {
			button: '#prev1',
			items: 1		},    
		next: {
			button: '#next1',
			items: 1		}
	});
});
$(document).ready(function () {
    $('.to-top').each(function () {
        var jelm = $(this);
        $(window).on('scroll', function () {
            if ($(window).scrollTop() > 100) {
                jelm.show();
            } else {
                jelm.hide();
            }
        });
        jelm.on('click', function (e) {
            e.preventDefault();
            $('html, body').animate({
                scrollTop: 0
            }, 500);
        });
    });
});