$(window).on('load', function () {
    $(".delete").click(function () {
        $(this).closest($(".albumPhoto")).hide();
        $(this).closest($(".albumPhoto")).find($(".data-delete-post-id")).eq(0).val(true);
    })
})