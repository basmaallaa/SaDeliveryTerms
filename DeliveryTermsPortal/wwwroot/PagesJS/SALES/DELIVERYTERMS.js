$(function () {
    var t = window.datatableTexts;
    $('#dataTable').DataTable({
        bFilter: true,
        language: {
            search: ' ',
            searchPlaceholder: t.searchPlaceholder,
            lengthMenu: t.lengthMenu,
            info: t.info,
            infoEmpty: t.infoEmpty,
            zeroRecords: t.zeroRecords,
            emptyTable: t.emptyTable,
            paginate: { next: t.next, previous: t.previous }
        }
    });
});