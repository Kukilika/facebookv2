$(window).on('load', function () {
    $(".edit").click(function () {
        var albumId = parseInt($(this).attr("data-edit-post-id"));
        window.location = "/Album/Edit?albumId=" + albumId.toString();
    });

    $(".delete").click(function (e) {
        var albumId = parseInt($(this).attr("data-delete-post-id"));
        if (confirm("Are you sure you want to delete this album?")) {
            $.post("/Album/Delete?albumId=" + albumId.toString())
                .done(function () {
                    location.reload(true);
                })
                .fail(function () {
                    alert("An error has occured!");
                });
        }
    });
});