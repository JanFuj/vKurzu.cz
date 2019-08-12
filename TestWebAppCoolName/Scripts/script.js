//quill
var toolbarOptions = [
    ['bold', 'italic', 'underilne', 'strike'],
    ['blockquote', 'code-block'],
    [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
    [{ 'sript': 'sub' }, { 'sript': 'super' }],
    [{ 'indent': '-1' }, { 'indent': '+1' }],
    [{ 'direction': 'rtl' }],
    [{ 'size': ['small', false, 'large', 'huge'] }],
    ['link', 'image', 'video', 'formula'],
    [ { 'background': [] }],
    [{ 'font': [] }],
    [{ 'align': [] }]
];

$(document).ready(function () {
    
    // Hide Header on on scroll down
    var didScroll;
    var lastScrollTop = 0;
    var delta = 10;
    var header = '#site-header';
    var headerHeight = $(header).outerHeight(true);

    $(window).scroll(function (_event) {
        didScroll = true;
    });

    setInterval(function () {
        if (didScroll) {
            hasScrolled();
            didScroll = false;
        }
    }, 250);

    function hasScrolled() {
        var st = $(this).scrollTop();

        // Make sure they scroll more than delta
        if (Math.abs(lastScrollTop - st) <= delta)
            return;

        if (st > headerHeight && lastScrollTop > headerHeight * 2) {
            $(header).removeClass('header--up-top');
        } else {
            $(header).addClass('header--up-top');
        }

        // If they scrolled down and are past the navbar, add class .nav-up.
        // This is necessary so you never see what is "behind" the navbar.
        if (st > lastScrollTop && st > headerHeight) {
            // Scroll Down
            $(header).removeClass('header--down').addClass('header--up');
        } else {
            // Scroll Up
            if (st + $(window).height() < $(document).height()) {
                $(header).removeClass('header--up').addClass('header--down');
            }
        }

        lastScrollTop = st;
    }


    //


});