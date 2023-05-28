$(function(){
    if ( $(".leftmenu").length){
        $("#menu").prepend($(".leftmenu").html());
        $("#menu").removeClass("hide");
    }

    $(document).on("click",".qarrow",function(){
        // $(".left-block-menu .parent").removeClass("active");
        $(this).parent().toggleClass("active");
    });
});

// Toggle left menu
var slideout = new Slideout({
     'panel': document.getElementById('panel'),
     'menu': document.getElementById('menu'),
     'padding': 256,
     'tolerance': 70
 });
 $(document).on("click",".btn-toggle-left,.toggle-button",function(){
     slideout.toggle();
 });