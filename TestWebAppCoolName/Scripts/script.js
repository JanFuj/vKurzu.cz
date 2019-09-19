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
    ['link', 'video', 'formula'],
    [ { 'background': [] }],
    [{ 'font': [] }],
    [{ 'align': [] }],
    ['image']
];

var swallform = async function () {

    const { value: formValues } = await Swal.fire({
        title: 'Zadej url a popis obrázku',
        html:
            '<input id="swal-input1" class="swal2-input">' +
                '<input id="swal-input2" class="swal2-input">',
        focusConfirm: false,
        preConfirm: () => {
            var obj = new Object();
            obj.url = document.getElementById('swal-input1').value;
            obj.description = document.getElementById('swal-input2').value;
            return obj
        }
    })
    if (formValues) {
        let json = JSON.stringify(formValues);
        console.log("FormValues " + json)
        return formValues
    }
}

async function imageHandler() {
    let range = this.quill.getSelection();
    let formValues = await swallform();
    console.log("Forma data aync" + formValues)

    if (formValues) {
        this.quill.insertEmbed(range.index, 'image', {
            url: formValues.url,
            alt: formValues.description,
            className: "QuillImage"
        })
        console.log(JSON.stringify(this.quill.getContents()))
    }
};

$(document).ready(function () {

 

    let BlockEmbed = Quill.import('blots/block/embed');
    class ImageBlot extends BlockEmbed {
        static create(value) {
            let node = super.create();
            node.setAttribute('alt', value.alt);
            node.setAttribute('src', value.url);
            node.setAttribute('class', value.className);
            return node;
        }

        static value(node) {
            return {
                alt: node.getAttribute('alt'),
                url: node.getAttribute('src'),
                class: node.getAttribute('class')
            };
        }
    }
    ImageBlot.blotName = 'image';
    ImageBlot.tagName = 'img';
    Quill.register(ImageBlot);



    
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