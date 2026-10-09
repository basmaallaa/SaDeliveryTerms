$(function () {
    var t = window.datatableTexts;
    var p = window.pageTexts;
    var isAr = window.appCulture === 'ar';

    function showToast(message, type) {
        if (!message) return;
        var el = $(
            '<div class="toast align-items-center text-bg-' + type + ' border-0" role="alert">' +
            '<div class="d-flex"><div class="toast-body"></div>' +
            '<button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>' +
            '</div></div>');
        el.find('.toast-body').text(message);
        $('#toastArea').append(el);
        new bootstrap.Toast(el[0], { delay: 4000 }).show();
    }

    showToast(window.flash.success, 'success');
    showToast(window.flash.error, 'danger');

    $('#dataTable').DataTable({
        bFilter: true,
        dom: '<"dt-search-row"f>' +
         '<"d-flex justify-content-between align-items-center mb-2"l>' +
         't' +
         '<"d-flex justify-content-between align-items-center mt-3"ip>',    
        columnDefs: [
                { targets: [0, 1, 2], className: isAr ? 'text-end' : 'text-start' },
                { targets: 3, className: 'text-center' },
                { targets: -1, orderable: false, className: 'text-nowrap' }
            ], 
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

    $('#dataTable').on('click', '.btn-edit', async function () {
        var code = $(this).data('code');
        try {
            var r = await fetch(location.pathname + '?handler=ByCode&code=' + code, {
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            });
            if (r.status === 401) {
                location.href = '/' + window.appCulture + '/Accounts/Login';
                return;
            }
            var res = await r.json();
            if (!res.status) {
                showToast(isAr ? res.messageAr : res.messageEn, 'danger');
                return;
            }
            var d = res.data;
            $('#edit_code').val(d.code);
            $('#edit_sname').val(d.sName);
            $('#edit_bname').val(d.bName);
            $('#edit_days').val(d.days);
            $('#edit_active').prop('checked', d.activeFlag);
            bootstrap.Modal.getOrCreateInstance(document.getElementById('editModal')).show();
        } catch (e) {
            showToast(p.serviceError, 'danger');
        }
    });

    $('#dataTable').on('click', '.btn-delete', function () {
        var code = $(this).data('code');
        Swal.fire({
            title: p.confirmTitle,
            text: p.confirmText,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: p.confirmYes,
            cancelButtonText: p.cancel,
            confirmButtonColor: '#dc3545'
        }).then(function (result) {
            if (result.isConfirmed) {
                $('#deleteCode').val(code);
                document.getElementById('deleteForm').submit();
            }
        });
    });

    document.getElementById('addModal').addEventListener('hidden.bs.modal', function () {
        $(this).find('form')[0].reset();
    });
});