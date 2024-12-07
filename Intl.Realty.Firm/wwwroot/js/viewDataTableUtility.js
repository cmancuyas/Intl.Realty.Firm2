function TabSelections() {
    const tabRoutes = {
        "users-tab": "/User/Index",
        "roles-tab": "/Role/Index",
        "employmentStatuses-tab": "/EmploymentStatus/Index",
        "departments-tab": "/Department/Index",
        "transactionTypes-tab": "/TransactionType/Index",
        "documentTypes-tab": "/DocumentType/Index",
        "dealStatuses-tab": "/DealStatus/Index",
        "documentTypeAssignments-tab": "/DocumentTypeAssignment/Index",
        "internalEmails-tab": "/InternalEmail/Index",
        "provinces-tab": "/Province/Index",
        "dealTransactionNumberRanges-tab": "/DealTransactionNumberRange/Index"
    };

    Object.keys(tabRoutes).forEach((id) => {
        $(`#${id}`).on("click", function () {
            window.location.href = tabRoutes[id];
        });
    });
}

function SearchBoxSettings() {
    $('[id^=dt-search]').attr('placeholder', 'Search...').addClass('fst-italic');
}

function CheckboxSelect() {
    $('#modelTable').on('change', '.column-checkbox', function () {
        const isChecked = $(this).prop('checked');
        $('.row-checkbox').prop('checked', isChecked);
    });

    $('#modelTable').on('change', '.row-checkbox', function () {
        const allChecked = $('.row-checkbox').length === $('.row-checkbox:checked').length;
        $('.column-checkbox').prop('checked', allChecked);
    });
}

function closeModal() {
    $("#deleteConfirmModal").modal("hide").remove();
}

function getIdsFromSelectedRows(selectedRows) {
    return selectedRows.map(row => row.value);
}

function ClearSearch() {
    $("#dt-search-0").val("").keyup();
}

function getSelectedRows() {
    return $('#modelTable').find('input[name="selectedRows"]:checked').serializeArray();
}

function OpenMultipleDeleteModal(controllerName) {
    const selectedRows = getSelectedRows();
    const selectedIds = getIdsFromSelectedRows(selectedRows);

    $('#DeleteMultipleModalContainer').load(`/${controllerName}/DeleteMultipleModal/`, { ids: selectedIds }, function () {
        $("#deleteConfirmModal").modal("show");
    });
}

function OpenDeleteModal(id, controllerName) {
    $('#DeleteModalContainer').load(`/${controllerName}/DeleteModal/${id}`, function () {
        $("#deleteConfirmModal").modal("show");
    });
}

function MultipleDelete() {
    $('#modelTable').on('change', '.row-checkbox, .column-checkbox', function () {
        const atLeastOneChecked = $('.row-checkbox:checked').length > 0;
        $('#btnMultipleDelete').toggleClass('d-none', !atLeastOneChecked).toggleClass('d-flex', atLeastOneChecked);
    });
}

function formatNumber(number) {
    return number.toLocaleString();
}

function AddSearchBoxPerColumn() {
    const searchBoxHide = ['select-checkbox', 'CreatedAt', 'Action'];

    $('#modelTable tfoot th').each(function () {
        if (!searchBoxHide.includes(this.innerText)) {
            $(this).html(`<input type="text" placeholder="${this.innerText} Search" />`);
        }
    });

    const table = $('#modelTable').DataTable();
    table.columns().every(function () {
        const that = this;
        $('input', this.footer()).on('keyup change', function () {
            if (that.search() !== this.value) {
                that.search(this.value).draw();
            }
        });
    });
}

function renderDownloadForm(format, controllerName) {
    const form = $('#ListFormId');
    const datatableParams = $('#modelTable').DataTable().ajax.params();

    form.attr('action', `/${controllerName}/ExportTable?format=${format}`);

    let input = form.find('input[name=dtParametersJson]');
    if (input.length) {
        input.val(datatableParams);
    } else {
        input = $('<input>', {
            type: 'hidden',
            name: 'dtParametersJson',
            value: datatableParams
        });
        form.append(input);
    }
}

function exportToExcel(controllerName) { renderDownloadForm("excel", controllerName); $("#ListFormId").submit(); }
function exportToCsv(controllerName) { renderDownloadForm("csv", controllerName); $("#ListFormId").submit(); }
function exportToHtml(controllerName) { renderDownloadForm("html", controllerName); $("#ListFormId").submit(); }
function exportToJson(controllerName) { renderDownloadForm("json", controllerName); $("#ListFormId").submit(); }
function exportToXml(controllerName) { renderDownloadForm("xml", controllerName); $("#ListFormId").submit(); }
function exportToYaml(controllerName) { renderDownloadForm("yaml", controllerName); $("#ListFormId").submit(); }

function getFileUploads(saleListingId) {
    return $.ajax({
        url: '/SaleListing/GetDocumentTypesThatHasFileUploads/',
        type: 'GET',
        data: { saleListingId },
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8'
    }).then(data => data).catch(() => []);
}

function toggleStatusLabels() {
    const isActive = $('#isActiveCheckbox').is(':checked');
    $('#lblStatus').toggleClass('text-primary', isActive).toggleClass('text-muted', !isActive).text(isActive ? "Active" : "Inactive");
}

function toggleStatusLabelSoftDelete() {
    const isDeleted = $('#isDeletedCheckbox').is(':checked');
    $('#lblStatusSoftDelete').toggleClass('text-primary', isDeleted).toggleClass('text-muted', !isDeleted).text(isDeleted ? "IsDeleted" : "NotDeleted");
}
