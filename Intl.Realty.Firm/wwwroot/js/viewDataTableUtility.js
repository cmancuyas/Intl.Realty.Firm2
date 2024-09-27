function TabSelections() {
    $("#users-tab").on("click", function () {
        window.location.href = '/User/Index';
    });
    $("#roles-tab").on("click", function () {
        window.location.href = '/Role/Index';
    });
    $("#employmentStatuses-tab").on("click", function () {
        window.location.href = '/EmploymentStatus/Index';
    });
    $("#departments-tab").on("click", function () {
        window.location.href = '/Department/Index';
    });
    $("#transactionTypes-tab").on("click", function () {
        window.location.href = '/TransactionType/Index';
    });
    $("#documentTypes-tab").on("click", function () {
        window.location.href = '/DocumentType/Index';
    });
    $("#dealStatuses-tab").on("click", function () {
        window.location.href = '/DealStatus/Index';
    });
    $("#documentTypeAssignments-tab").on("click", function () {
        window.location.href = '/DocumentTypeAssignment/Index';
    });
    $("#internalEmails-tab").on("click", function () {
        window.location.href = '/InternalEmail/Index';
    });
    $("#provinces-tab").on("click", function () {
        window.location.href = '/Province/Index';
    });
}
function SearchBoxSettings() {
    $('[id^=dt-search]').attr('placeholder', 'Search...').addClass('fst-italic');
}
function CheckboxSelect() {
    $('.column-checkbox').change(function () {
        var isChecked = $(this).prop('checked');
        $('.row-checkbox').prop('checked', isChecked);
    });

    $('#modelTable').on('change', '.column-checkbox', function () {
        var isChecked = $(this).prop('checked');
        $('.row-checkbox').prop('checked', isChecked);

    });

    $('#modelTable').on('change', '.row-checkbox', function () {
        var isChecked = $(this).prop('checked');

        var allChecked = $('.row-checkbox').length === $('.row-checkbox:checked').length;
        $('.column-checkbox').prop('checked', allChecked);

    });

    $('.row-checkbox').change(function () {
        var allChecked = $('.row-checkbox').length === $('.row-checkbox:checked').length;
        $('.column-checkbox').prop('checked', allChecked);
    });

}

function closeModal() {
    $("#deleteConfirmModal").modal("hide");
    let elem = document.querySelector('#deleteConfirmModal');
    elem.parentNode.removeChild(elem);
}

function getIdsFromSelectedRows(selectedRows) {
    var ids = [];
    for (var i = 0; i < selectedRows.length; ++i)
        ids.push(selectedRows[i].value);
    return ids;
}

function ClearSearch() {
    $("#dt-search-0").val("").keyup();
}

function getSelectedRows() {
    var table = $('#modelTable').dataTable();
    selectedRows = table.find('input[name="selectedRows"]:checked').serializeArray();

    return selectedRows;
}

function OpenMultipleDeleteModal(controllerName) {
    var selectedRows = getSelectedRows();
    var selectedIdsArray = getIdsFromSelectedRows(selectedRows);

    $('#DeleteMultipleModalContainer').load('/' + controllerName + '/DeleteMultipleModal/', { ids: selectedIdsArray }, function () {
        $("#deleteConfirmModal").modal("show");
    });
}
function OpenDeleteModal(id, controllerName) {

    $('#DeleteModalContainer').load('/' + controllerName + ' /DeleteModal/' + id, function () {

        $("#deleteConfirmModal").modal("show");
    });
}
function MultipleDelete() {
    $('#modelTable').on('change', '.row-checkbox, .column-checkbox', function () {
        var atLeastOneChecked = $('.row-checkbox:checked').length > 0;
        $('#btnMultipleDelete').toggleClass('d-none', !atLeastOneChecked);
        $('#btnMultipleDelete').toggleClass('d-flex', atLeastOneChecked);

        if ($(this).hasClass('column-checkbox')) {
            var isChecked = $(this).prop('checked');
            $('.row-checkbox').prop('checked', isChecked);
        }
    });
}
function formatNumber(number) {
    return number.toLocaleString();
}

function AddSearchBoxPerColumn() {

    const searchBoxHide = ['select-checkbox', 'CreatedAt', 'Action'];

    $('#modelTable tfoot th').each(function () {
        if ($.inArray(this.innerText, searchBoxHide) == -1) {
            var title = $(this).text();
            $(this).html('<input type="text" placeholder="' + title + ' Search" />');
        }
    });
    // DataTable
    var table = $('#modelTable').DataTable();
    // Apply the search
    table.columns().every(function () {
        var that = this;

        $('input', this.footer()).on('keyup change', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });
}

function renderDownloadForm(format, controllerName) {
    $('#ListFormId').attr('action', '/' + controllerName + '/ExportTable?format=' + format);
    // Get jQuery DataTables AJAX params
    var datatableParams = $('#modelTable').DataTable().ajax.params();

    // If the input exists, replace value, if not create the input and append to form
    if ($("#ListFormId input[name=dtParametersJson]").val()) {
        $('#ListFormId input[name=dtParametersJson]').val(datatableParams);
    } else {
        var searchModelInput = $("<input>")
            .attr("type", "hidden")
            .attr("name", "dtParametersJson")
            .val(datatableParams);

        $('#ListFormId').append(searchModelInput);
    }
}
function exportToExcel(controllerName) {
    renderDownloadForm("excel", controllerName);

    $("#ListFormId").submit();
}

function exportToCsv(controllerName) {
    renderDownloadForm("csv", controllerName);

    $("#ListFormId").submit();
}

function exportToHtml(controllerName) {
    renderDownloadForm("html", controllerName);

    $("#ListFormId").submit();
}

function exportToJson(controllerName) {
    renderDownloadForm("json", controllerName);

    $("#ListFormId").submit();
}

function exportToXml(controllerName) {
    renderDownloadForm("xml", controllerName);

    $("#ListFormId").submit();
}

function exportToYaml(controllerName) {
    renderDownloadForm("yaml", controllerName);

    $("#ListFormId").submit();
}
function onBtnSubmitCreate() {
    $("#btnSubmit").click(function () {

        $("#confirmSubmitModal").modal("show");
        sendEmail();
    });
}
function onBtnSubmit() {
    $("#btnSubmit").click(function () {

        var openModalDialog = filesValidation();

        if (openModalDialog == true) {
            $("#confirmSubmitModal").modal("show");
            sendEmail();
        }
    });
}
function onBtnSave() {
    $("#btnSave").click(function () {

        $("#confirmSaveModal").modal("show");
    });
}

function confirmSaveModal() {

    const ListFormId = document.getElementById('ListFormId');

    $("#btnConfirmSave").click(function () {

        $("#confirmSaveModal").modal("hide");
        $('input[name=DealStatusId]').val('2'); // Saved

        ListFormId.submit();
    });
}
function confirmSubmitModal() {
    const ListFormId = document.getElementById('ListFormId');
    $("#btnConfirmSubmit").click(function () {

        $("#confirmSubmitModal").modal("hide");
        $('input[name=DealStatusId]').val('1'); // Submitted
        ListFormId.submit();
    });
}
function getFileUploads(saleListingId) {

    let parameters = { saleListingId: saleListingId };
    var documentTypes = [];
    $.ajax({
        url: '/SaleListing/GetDocumentTypesThatHasFileUploads/',
        type: 'GET',
        data: parameters,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        async: false,
        success: function (data) {
            documentTypes = data;
        }
    });
    return documentTypes;
}


function fileSelected(that) {
    var curval = that.dataset.curval;
    var id = that.id;
    document.getElementById(that.id).setAttribute("data-prevVal", curval);
    document.getElementById(that.id).setAttribute("data-curVal", that.value);
}
function sendEmail() {

}

function toggleStatusLabels() {
    if ($('#isActiveCheckbox').is(':checked')) {
        $('#lblStatus').css('color', '#0d6efd').text("Active");
    } else {
        $('#lblStatus').css('color', '#bfbfbf').text("Inactive");
    }
}
function toggleStatusLabelSoftDelete() {

    if ($('#isDeletedCheckbox').is(':checked')) {
        $('#lblStatusSoftDelete').css('color', '#0d6efd').text("IsDeleted");
    } else {
        $('#lblStatusSoftDelete').css('color', '#bfbfbf').text("NotDeleted");
    }
}