$(document).ready(function () {
    $('#SocialThumbnailImage').on('change textInput input', function () {

        document.getElementById("SocialThumbnailImagePlaceHolder").src = this.value
    });
});