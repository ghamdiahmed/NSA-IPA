$(document).ready(function () {

    $('input[type=radio][name="Blast.ReferenceIsText"]').change(function () {

        if ($("input[name='Blast.ReferenceIsText']:checked").val() == 'true') {

            $('#reference-file').slideUp();
            $('#reference-text').slideDown();

        } else {
            $('#reference-text').slideUp();
            $('#reference-file').slideDown();
        }

    });


    $('input[type=radio][name="Blast.QueryIsText"]').change(function () {

        if ($("input[name='Blast.QueryIsText']:checked").val() == 'true') {

            $('#query-file').slideUp();
            $('#query-text').slideDown();

        } else {
            $('#query-text').slideUp();
            $('#query-file').slideDown();
        }

    });

    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });
});
