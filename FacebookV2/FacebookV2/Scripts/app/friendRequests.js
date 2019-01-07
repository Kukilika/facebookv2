$(".approve").on("click", function () {
    var approveBtn = this;
    var requesterId = $(this).attr("requesterId");
    $.post("/Friend/ApproveRequest", { requesterId: requesterId })
        .done(function (result) {
            addFriend(result, approveBtn)
        })
        .fail(function () {
            alert("An error has occured!");
        });
});

function addFriend(result, approveBtn) {
    $(approveBtn).closest(".user-box").remove();

    var resultUserName = $(result).find($(".userName"));

    $(resultUserName).click(function () {
        var userId = $(resultUserName).attr("userId");
        window.location = "/Profile/Show?userId=" + userId.toString();
    });

    $("#friends").append(result);
}


$(".reject").on("click", function () {
    var rejectBtn = this;
    var requesterId = $(this).attr("requesterId");
    $.post("/Friend/RejectRequest", { requesterId: requesterId })
        .done(function () {
            rejectFriend(rejectBtn)
        })
        .fail(function () {
            alert("An error has occured!");
        });
});

function rejectFriend(rejectBtn) {
    $(rejectBtn).closest(".user-box").remove();
}