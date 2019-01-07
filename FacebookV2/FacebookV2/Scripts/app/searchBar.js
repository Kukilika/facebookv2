$("#searchBar").select2({
    ajax: {
        url: "/UserSearch/FindUsers",
        dataType: 'json',
        data: function (params) {
            var queryParameters = {
                q: params.term
            }
            return queryParameters;
        },
        processResults: function (data) {
            return {
                results: data
            };
        },
    },
    placeholder: 'Find people...',
    minimumInputLength: 1
});

$('#searchBar').on("select2:selecting", function (e) {
    var userId = e.params.args.data.id;
    window.location = "/Profile/Show?userId=" + userId.toString();
});
