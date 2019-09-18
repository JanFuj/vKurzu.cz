$(document).ready(function () {
    document.getElementById("SocialThumbnailImage").onchange = function () {
        var reader = new FileReader();

        reader.onload = function (e) {
            // get loaded data and render thumbnail.
            document.getElementById("SocialThumbnailImagePlaceHolder").src = e.target.result;
        };

        // read the image file as a data URL.
        reader.readAsDataURL(this.files[0]);
       
    };

   


});