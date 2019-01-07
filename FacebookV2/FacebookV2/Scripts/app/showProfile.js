$("#sendFriendRequest").click(function () {
    var sendBtn = this;
    var userId = $(this).attr("userId");
    $(this).prop('disabled', true);
    $.post("/Friend/SendRequest", { userId: userId })
        .done(function () {
            sendBtn.innerHTML = "Friend Request Sent";
        })
        .fail(function () {
            alert("An error has occured!");
        });
});

$(window).on('load', function () {
    $(".albumPhoto").click(function () {
        var hasPhotos = $(this).attr("data-no-photos");
        if (hasPhotos == 0) {
            return;
        }

        var albumId = $(this).attr("data-photo-post-id");
        var albumArea = this;
        $.post("/Album/GetAlbum", { albumId: albumId })
            .done(function (result) {
                $("body").append(result);
                document.getElementById('myModal').style.display = "block";
                $("#myModal").show();
            })
            .fail(function () {
                alert("An error has occured!");
            });
    });
}); 
