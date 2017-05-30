// Testimonial Carousel
if ($('.testimonial-content-carousel').length && $('.testimonial-thumbs-carousel').length) {

    var $sync1 = $(".testimonial-content-carousel"),
        $sync2 = $(".testimonial-thumbs-carousel"),
        flag = false,
        duration = 500;

    $sync1
        .owlCarousel({
            loop: false,
            items: 1,
            margin: 0,
            nav: true,
            navText: ['<span class="fa fa-angle-left"></span>', '<span class="fa fa-angle-right"></span>'],
            dots: false,
            autoplay: true,
            autoplayTimeout: 5000
        })
        .on('changed.owl.carousel', function (e) {
            if (!flag) {
                flag = false;
                $sync2.trigger('to.owl.carousel', [e.item.index, duration, true]);
                flag = false;
            }
        });

    $sync2
        .owlCarousel({
            loop: false,
            margin: 0,
            nav: false,
            navText: ['<span class="icon fa fa-long-arrow-left"></span>', '<span class="icon fa fa-long-arrow-right"></span>'],
            dots: false,
            center: false,
            autoplay: true,
            autoplayTimeout: 5000,
            responsive: {
                0: {
                    items: 2
                },
                400: {
                    items: 3
                },
                600: {
                    items: 3
                },
                1000: {
                    items: 3
                },

            },
        })

        .on('click', '.owl-item', function () {
            $sync1.trigger('to.owl.carousel', [$(this).index(), duration, true]);
        })
        .on('changed.owl.carousel', function (e) {
            if (!flag) {
                flag = true;
                $sync1.trigger('to.owl.carousel', [e.item.index, duration, true]);
                flag = false;
            }
        });

}