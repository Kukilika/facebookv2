$(window).on('load', function () {
    $(".userName").click(function () {
        var userId = $(this).attr("userId");
        window.location = "/Profile/Show?userId=" + userId.toString();
    });
});