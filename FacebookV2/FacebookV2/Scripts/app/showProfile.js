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
            alert("Albumul nu are nicio poza !");
            return;
        }

        var albumId = $(this).attr("data-photo-post-id");
        var albumArea = this;
        window.location = "/Album/ShowPhotosFromAlbum?albumId=" + albumId.toString();

        //$.post("/Album/ShowPhotosFromAlbum?albumId=" + albumId.toString())
        //    .done(function (result) {
        //        $("body").append(result);
        //        document.getElementById('myModal').style.display = "block";
        //        $("#myModal").show();
        //    })
        //    .fail(function () {
        //        alert("An error has occured!");
        //    });
    });
}); 
