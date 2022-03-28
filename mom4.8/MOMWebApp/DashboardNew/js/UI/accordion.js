function collapseAll() {
    $(".accrd").removeClass(function () {
        return "active";
    });
    $('html, body').stop().animate({
        'scrollTop': 0
    }, 300, 'swing');
    $(".collapsible-accordion").collapsible({ accordion: true });
    $(".collapsible-accordion").collapsible({ accordion: false });

}

function expandAll() {
    $('html, body').stop().animate({
        'scrollTop': 0
    }, 300, 'swing');
    $(".accrd").addClass("active");
    $(".collapsible-accordion").collapsible({ accordion: false });
}

$('.collapse-expand').on('click', function (e) {
    if (this.classList.contains("opened") === true) {
        this.classList.remove("opened");
        this.innerHTML.replace(/mdi-action-open-in-browser/g, 'mdi-action-system-update-tv')
        collapseAll();
    }
    else {
        this.classList.add("is-active");
        this.classList.add("opened");
        this.innerHTML.replace(/mdi-action-system-update-tv/g, 'mdi-action-open-in-browser')
        expandAll();
    }
});


(function () {

    "use strict";

    var toggles = document.querySelectorAll(".c-hamburger");

    for (var i = toggles.length - 1; i >= 0; i--) {
        var toggle = toggles[i];
        toggleHandler(toggle);
    };

    function toggleHandler(toggle) {
        toggle.addEventListener("click", function (e) {
            e.preventDefault();
            if (this.classList.contains("is-active") === true) {
                this.classList.remove("is-active");
                collapseAll();
            }
            else {
                this.classList.add("is-active");
                expandAll();
            }
        });
    }

})();

$("a").click(function () {
    // If this isn't already active
    if (!$(this).hasClass("active")) {
        // Remove the class from anything that is active
        $("a.active").removeClass("active");
        // And make this active
        $(this).addClass("active");
    }
});

$(document).ready(function () {



    //$('.link-slide').on('click', function (e) {
    //    e.preventDefault();

    //    var target = this.hash;
    //    var $target = $(target);
    //    if ($(target).hasClass('active') || target == "") {

    //    }
    //    else {
    //        $(target).click();
    //    }


    //    var addinfo = document.getElementById('addinfo').style.display
    //    if (addinfo == "block") {
    //        $('html, body').stop().animate({
    //            'scrollTop': $target.offset().top - 420
    //        }, 900, 'swing');
    //    }
    //    else {
    //        $('html, body').stop().animate({
    //            'scrollTop': $target.offset().top - 125
    //        }, 900, 'swing');
    //    }


    //});

    $(window).scroll(function () {
        if ($(window).scrollTop() >= 0) {
            $('#divButtons').addClass('fixed-header');
        }
        if ($(window).scrollTop() <= 0) {
            $('#divButtons').removeClass('fixed-header');
        }
    });

    $(".dropdown-content.select-dropdown li").on("click", function () {
        var that = this;
        setTimeout(function () {
            if ($(that).parent().hasClass('active')) {
                $(that).parent().removeClass('active');
                $(that).parent().hide();
            }
        }, 100);
    });




});


