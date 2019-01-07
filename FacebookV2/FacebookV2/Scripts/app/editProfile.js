(function () {
    $('#CountyId').on("change", function () {
        var countyId = $('#CountyId').val();
        $("#CountyId option[value='']").attr("disabled", "disabled");
        $.get("/City/GetAllCitiesFromCounty", { countyId: countyId })
            .done(manageCities)
            .fail(function () {
                alert("An error has occured!");
            });
    });


})();

$(window).on('load', function () {
    var countyId = $('#CountyId').val();

    if (countyId != '')
        $.get("/City/GetAllCitiesFromCounty", { countyId: countyId })
            .done(manageCities)
            .fail(function () {
                alert("An error has occured!");
            });
});

function manageCities(result) {
    var options = '';
    $('#CityDropDown').empty();
    for (let i = 0; i < result.length; i++) {
        options += '<option value="' + result[i].Id + '">' + result[i].Name + '</option>';
    }
    $('#CityDropDown').append(options);
}