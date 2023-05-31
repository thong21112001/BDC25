$(document).ready(function () {
    $(".content").slice(0, 2).show();
    $(".loadPost").on("click", function (e) {
        e.preventDefault();
        $(".content:hidden").slice(0, 5).slideDown();
        if ($(".content:hidden").length == 0) {
            $(".loadPost").text("Không còn bài viết nào").addClass("noContent");
        } else {
            $(".loadPost").text("Không còn bài viết nào").addClass("noContent");
        }
    });
})

$(document).ready(function () {
    $(".content").slice(0, 2).show();
    $(".loadPostFL").on("click", function (e) {
        e.preventDefault();
        $(".content:hidden").slice(0, 5).slideDown();
        if ($(".content:hidden").length == 0) {
            $(".loadPost").text("Không còn bài viết nào").addClass("noContent");
        } else {
            $(".loadPostFL").text("Không còn bài viết nào").addClass("noContent");
        }
    });
})