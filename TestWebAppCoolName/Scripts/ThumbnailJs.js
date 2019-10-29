$(document).ready(function () {
    $('#SocialThumbnailImage').on('change textInput input', function () {

        document.getElementById("SocialThumbnailImagePlaceHolder").src = this.value
    });
    $('#HeaderImage').on('change textInput input', function () {

        document.getElementById("HeaderImagePlaceHolder").src = this.value
    });
});