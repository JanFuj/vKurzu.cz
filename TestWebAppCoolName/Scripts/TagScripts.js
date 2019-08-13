

$(document).ready(function () {
    updateTagsTextBox();

    (function () {

        $("#btnRight").click(function (e) {
            var selectedOpts = $('#lstBox1 option:selected');
            if (selectedOpts.length == 0) {
                e.preventDefault();
            }

            $('#lstBox2').append($(selectedOpts).clone());
            $(selectedOpts).remove();
            $('#tags').val("")
            e.preventDefault();
            updateTagsTextBox();
        });

        $('#btnLeft').click(function (e) {
            var selectedOpts = $('#lstBox2 option:selected');
            if (selectedOpts.length == 0) {
                e.preventDefault();
            }

            $('#lstBox1').append($(selectedOpts).clone());
            $(selectedOpts).remove();
            e.preventDefault();
            updateTagsTextBox();
        });


    }(jQuery));



    function updateTagsTextBox() {
        $('#tags').val("")
        let tags = "";
        $("#lstBox1 > option").each(function () {
            tags = tags + ("&" + $(this).val() + " ")
            $('#tags').val(tags)
            $('#tags').change()
            console.log($(this).val())
        });
    }
});