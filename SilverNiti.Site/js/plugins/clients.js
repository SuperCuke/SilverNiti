////Sponsors Carousel
//if ($('.sponsors-carousel').length) {
//    $('.sponsors-carousel').owlCarousel({
//        loop: true,
//        margin: 30,
//        nav: false,
//        smartSpeed: 500,
//        autoplay: 4000,
//        navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
//        responsive: {
//            0: {
//                items: 1
//            },
//            600: {
//                items: $(this).attr("elements-number")
//            },
//            800: {
//                items: $(this).attr("elements-number")
//            },
//            1024: {
//                items: $(this).attr("elements-number")
//            },
//            1200: {
//                items: $(this).attr("elements-number")
//            }
//        }
//    });
//}

if ($('.sponsors-carousel').length) {
    $('.sponsors-carousel').each(function () {
        $(this).owlCarousel({
            loop: true,
            margin: 30,
            nav: false,
            smartSpeed: 500,
            autoplay: 4000,
            navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: $(this).attr("elements-number")
                },
                800: {
                    items: $(this).attr("elements-number")
                },
                1024: {
                    items: $(this).attr("elements-number")
                },
                1200: {
                    items: $(this).attr("elements-number")
                }
            }
        });
    });
};

