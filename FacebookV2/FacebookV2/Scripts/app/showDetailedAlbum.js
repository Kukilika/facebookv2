$(".addComment").focusin(function () {
    if ($(this).hasClass("notChanged")) {
        $(this).val("");
        $(this).removeClass("notChanged");
        $(this).addClass("changed");
    }
});

$(".addComment").focusout(function () {
    if ($(this).val().length == 0) {
        $(this).val("Add comment...");
        $(this).removeClass("changed");
        $(this).addClass("notChanged");
    }
});

$(".addComment").keypress(function (e) {
    var code = (e.keyCode ? e.keyCode : e.which);
    if (code == 13) {
        var inputArea = this;
        var body = $(this).val();
        var postId = $(this).attr("postId");

        if (body.length > 0) {
            $.post("/Comment/AddComment", { photoId: postId, body: body })
                .done(function (result) {
                    addComment(result, postId, inputArea);
                }).fail(function () {
                    alert("An error has occured!");
                });
        }
    }
});

function addComment(result, postId, inputArea) {
    $("[data-comment-post-id=" + postId + "]").append(result);
    $(inputArea).blur();
    $(inputArea).removeClass("changed");
    $(inputArea).addClass("notChanged");
    $(inputArea).val("Add comment...");
    var currentCommentsNumber = parseInt($("[data-commentLabel-post-id=" + postId + "]").text());
    $("[data-commentLabel-post-id=" + postId + "]").text((currentCommentsNumber + 1).toString());
}

$(window).on('load', function () {
    var x = Math.floor(currentPage / maxButtons);
    if (currentPage % maxButtons == 0)
        x--;

    var minPage = x * maxButtons + 1;
    var maxPage = Math.min(((x + 1) * maxButtons), numberOfPages);

    $('<button/>', {
        text: '\u00AB',
        id: 'previous',
        disabled: isPreviousButtonDisabled(minPage),
        click: function () {
            var newMinPage = minPage - maxButtons;
            window.location = "/Album/ShowPhotosFromAlbum?albumId=" + idAlbum.toString() + "&page=" + newMinPage.toString();
        }
    }).appendTo("#pages");

    for (let i = minPage; i <= maxPage; i++) {
        $('<button/>', {
            text: i,
            id: 'btn_' + i,
            disabled: isButtonDisabled(currentPage, i),
            click: function () {
                window.location = "/Album/ShowPhotosFromAlbum?albumId=" + idAlbum.toString() + "&page=" + i.toString();
            }
        }).appendTo("#pages");
    }

    $('<button/>', {
        text: '\u00BB',
        id: 'next',
        disabled: isNextButtonDisabled(maxPage, numberOfPages),
        click: function () {
            var newMaxPage = Math.min((maxPage + maxButtons), numberOfPages);
            window.location = "/Album/ShowPhotosFromAlbum?albumId=" + idAlbum.toString() + "&page=" + newMaxPage.toString();
        }
    }).appendTo("#pages");

    $(".userName").click(function () {
        var userId = $(this).attr("userId");
        window.location = "/Profile/Show?userId=" + userId.toString();
    })

    $(".moreComments").click(function () {
        var moreCommentsBtn = this;
        var postId = parseInt($(moreCommentsBtn).attr("data-showComments-post-id"));

        var showedComments = parseInt($("[data-numberOfComments-post-id=" + postId + "]").val());

        var totalComments = parseInt($("[data-commentLabel-post-id=" + postId + "]").text());

        if (totalComments > showedComments) {
            $.get("/Comment/ShowMore", {
                photoId: postId,
                showedComments: showedComments,
                totalComments: totalComments
            })
                .done(function (result) {
                    showedComments = showedComments + parseInt(result.length);

                    $("[data-numberOfComments-post-id=" + postId + "]").val(showedComments);

                    for (var i = 0; i < result.length; i++) {
                        $(`<div class="comment-box">
                                <img src="${result[i].profilePhotoUrl}">
                                <label class="userName" userid="${result[i].userId}">${result[i].lastName} ${result[i].firstName}</label >

                                <span>${result[i].body}</span >
                            </div > `).appendTo($("[data-comment-post-id=" + postId + "]"));
                    }

                    if (totalComments == showedComments) {
                        $(moreCommentsBtn).remove();
                    }
                })
                .fail(function () {
                    alert("An error has occured!");
                });
        }
    });
});

function isButtonDisabled(currentPage, generatedButtonNumber) {
    if (currentPage == generatedButtonNumber)
        return true;
    return false;
}

function isPreviousButtonDisabled(minPage) {
    if (minPage == 1)
        return true;
    return false;
}

function isNextButtonDisabled(maxPage, numberOfPages) {
    if (numberOfPages == maxPage)
        return true;
    return false;
}

$(".likeButton").on("click", function () {
    var likeBtn = this;
    var postId = $(this).attr("data-like-post-id");

    $.post("/Like/RatePhoto", { photoId: postId })
        .done(function (result) {
            modifyLikeButton(likeBtn, result, postId)
        })
        .fail(function () {
            alert("An error has occured!");
        });
});

function modifyLikeButton(likeBtn, result, postId) {
    if (result == true) {
        var currentLikesNumber = parseInt($("[data-likeLabel-post-id=" + postId + "]").text());
        var newLikesNumber = (currentLikesNumber - 1).toString();
        $("[data-likeLabel-post-id=" + postId + "]").text(newLikesNumber);
        $(likeBtn).find(".like-text").text("Like");
        $(likeBtn).find(".like-icon").removeClass("glyphicon glyphicon-thumbs-down");
        $(likeBtn).find(".like-icon").addClass("glyphicon glyphicon-thumbs-up");
    }
    else {
        var currentLikesNumber = parseInt($("[data-likeLabel-post-id=" + postId + "]").text());
        var newLikesNumber = (currentLikesNumber + 1).toString();
        $("[data-likeLabel-post-id=" + postId + "]").text(newLikesNumber);
        $(likeBtn).find(".like-text").text("Dislike");
        $(likeBtn).find(".like-icon").removeClass("glyphicon glyphicon-thumbs-up");
        $(likeBtn).find(".like-icon").addClass("glyphicon glyphicon-thumbs-down");
    }
}

