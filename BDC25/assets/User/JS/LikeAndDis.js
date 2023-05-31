$("#Like").click(function () {
    debugger;
    ajaxGet("@Url.Content("+ ~/Community/Like + ")/?id=" + @Model.Threads.ThreadID + "&status=" + $(this).data("status"), function (data) {
        var counters = data.split('/');
        $("#likecount").text(counters[0]);
        $("#dislikecount").text(counters[1]);
        $("#Like").attr('disabled', 'disabled');
        $("#Dislike").attr('disabled', false);
    });  
});

$("#Dislike").click(function () {
    debugger;
    ajaxGet("@Url.Content(" + ~/Community/Like + ")/?id=" + @Model.Threads.ThreadID + "&status=" + $(this).data("status"), function (data) {
        var counters = data.split('/');
        $("#likecount").text(counters[0]);
        $("#dislikecount").text(counters[1]);
        $("#Dislike").attr('disabled', 'disabled');
        $("#Like").attr('disabled', false);
    });  
}); 