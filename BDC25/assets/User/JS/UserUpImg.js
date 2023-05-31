function ShowImage(imageUp, previewImg) {
    if (imageUp.files && imageUp.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImg).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUp.files[0]);
    }
}