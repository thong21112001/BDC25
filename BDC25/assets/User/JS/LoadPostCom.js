$(document).ready(function () {
    $(".content").slice(0, 5).show();
    $("#loadMore").on("click", function (e) {
        e.preventDefault();
        $(".content:hidden").slice(0, 5).slideDown();
        if ($(".content:hidden").length == 0) {
            $("#loadMore").text("Hết bài viết").addClass("noContent");
        }
    });
})