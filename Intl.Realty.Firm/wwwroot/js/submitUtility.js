// ✅ Field Validation (Tabs)
function defaultValidateAllTabs() {
    let hasErrors = false;
    let firstErrorTab = null;

    $(".tab-pane").each(function () {
        const tabId = $(this).attr("id");

        $(this).find("input, textarea, select").each(function () {
            const isRequired = $(this).attr("data-val-required") !== undefined;
            const isEmpty = !$(this).val()?.trim();

            if (isRequired && isEmpty) {
                $(this).addClass("border-danger");
                hasErrors = true;
                if (!firstErrorTab) firstErrorTab = tabId;
            } else {
                $(this).removeClass("border-danger");
            }
        });
    });

    if (firstErrorTab) {
        $('.nav-tabs button[data-bs-target="#' + firstErrorTab + '"]').tab("show");
    }

    return hasErrors;
}

// ✅ Async File Validation
async function defaultValidateFilesAsync(documentTypeAssignment = []) {
    const entityType = detectEntityType();
    const entityId = detectEntityId();
    const uploadedDocumentTypes = await getUploadedDocumentTypesAsync(entityType, entityId);

    let missing = [];

    $("#tblApplicantFiles tbody tr").each(function () {
        const docTypeId = parseInt($(this).data("document-type-id"));
        const fileInput = $(this).find('input[type="file"]')[0];
        const hasNewFile = fileInput?.files?.length > 0;

        const docType = documentTypeAssignment.find(d => d.DocumentType?.Id === docTypeId);
        const isRequired = docType?.IsRequired === true || docType?.IsRequired === "true";
        const alreadyUploaded = uploadedDocumentTypes.some(u => parseInt(u.id ?? u.Id) === docTypeId);

        if (isRequired) {
            if (alreadyUploaded || hasNewFile) {
                $(fileInput).removeClass("border-danger");
                alreadyUploaded ? showAlreadyUploadedIndicator(fileInput) : removeUploadIndicator(fileInput);
            } else {
                const docLabel = docType?.DocumentType?.Description ?? `Document`;
                missing.push(`<li>${docLabel}</li>`);
                $(fileInput).addClass("border-danger");
                removeUploadIndicator(fileInput);
            }
        } else {
            $(fileInput).removeClass("border-danger");
            removeUploadIndicator(fileInput);
        }
    });

    if (missing.length > 0) {
        return {
            success: false,
            message: `<b>The following required documents are missing:</b><ul>${missing.join("")}</ul>`
        };
    }

    return { success: true, message: "" };
}

// ✅ AJAX: Get uploaded document types
async function getUploadedDocumentTypesAsync(entityType, entityId) {
    if (!entityType || !entityId) {
        return [];
    }

    try {
        const response = await $.ajax({
            url: `/${entityType}/GetDocumentTypesThatHasFileUploads/`,
            type: 'GET',
            data: { [`${entityType.toLowerCase()}Id`]: entityId }
        });
        return response;
    } catch (err) {
        console.error(`❌ Failed to load uploaded files for ${entityType}`, err);
        return [];
    }
}

// ✅ ViewModel prefix
function detectViewModelPrefix() {
    const input = document.querySelector('input[name*="ViewModel"]');
    if (!input) return "";
    const match = input.name.match(/^(\w+ViewModel)/);
    return match ? match[1] : "";
}

// ✅ Entity type
function detectEntityType() {
    const prefix = detectViewModelPrefix();
    if (prefix.includes("SaleListing")) return "SaleListing";
    if (prefix.includes("SaleCoop")) return "SaleCoop";
    if (prefix.includes("LeaseListing")) return "LeaseListing";
    if (prefix.includes("LeaseCoop")) return "LeaseCoop";
    return "";
}

// ✅ Entity ID
function detectEntityId() {
    const prefix = detectViewModelPrefix();
    const input = document.querySelector(`input[type="hidden"][name="${prefix}.Id"]`);
    return input ? input.value : null;
}

// ✅ Upload Status Indicator
function showAlreadyUploadedIndicator(fileInput) {
    const $fileInput = $(fileInput);
    const $existing = $fileInput.siblings(".upload-status");

    if ($existing.length === 0) {
        $fileInput.after(`
            <span class="badge bg-success upload-status ms-2" title="This required document has already been uploaded.">
                Uploaded
            </span>
        `);
    }
}

function removeUploadIndicator(fileInput) {
    $(fileInput).siblings(".upload-status").remove();
}

// ✅ Reset Button Utilities
function resetSaveButton() {
    $('#btnSave')
        .prop('disabled', false)
        .html('Save as Draft');
}

function resetSubmitButton() {
    $('#btnSubmit')
        .prop('disabled', false)
        .html('Submit');
    $("#submitSpinner").hide();
}

// ✅ Main Submit Handler
function initializeSubmitHandlers({
    formId,
    dealStatusDropdownSelector,
    isAdminLike,
    fileErrorScrollTargetId,
    validateTabs = defaultValidateAllTabs,
    validateFilesFn = defaultValidateFilesAsync,
    documentTypeAssignment = []
}) {
    const $form = $(`#${formId}`);
    let isSubmitting = false;

    // Cancel Save resets state
    $('#confirmSaveModal').on('hidden.bs.modal', resetSaveButton);
    $('#confirmSubmitModal').on('hidden.bs.modal', resetSubmitButton);

    $('#btnSave').on('click', () => {
        $('#ActionTypeHidden').val('save');
        $('#confirmSaveModal').modal('show');
    });

    $('#btnSubmit').on('click', async () => {
        $('#ActionTypeHidden').val('submit');

        const hasFieldErrors = validateTabs();
        if (hasFieldErrors) {
            Swal.fire({
                icon: "error",
                title: "Form Incomplete",
                text: "Please complete all required fields.",
                confirmButtonColor: "#d33"
            });
            resetSubmitButton(); // Reset if error
            return;
        }

        const result = await validateFilesFn(documentTypeAssignment);
        if (!result.success) {
            if (fileErrorScrollTargetId) {
                document.getElementById(fileErrorScrollTargetId)?.scrollIntoView({ behavior: "smooth", block: "center" });
            }

            Swal.fire({
                icon: "error",
                title: "Missing Required Documents",
                html: result.message,
                confirmButtonColor: "#d33"
            });
            resetSubmitButton(); // Reset if file error
            return;
        }

        if (!isAdminLike && dealStatusDropdownSelector) {
            const prefix = detectViewModelPrefix();
            const fallbackName = `${prefix}.DealStatusId`;

            if ($(dealStatusDropdownSelector).length === 0) {
                $form.append(`<input type="hidden" name="${fallbackName}" value="1" />`);
            } else {
                $(dealStatusDropdownSelector).val("1");
            }
        }

        $('#confirmSubmitModal').modal('show');
    });

    // ✅ Confirm Save
    $(document).on('click', '#btnConfirmSave', () => {
        if (isSubmitting) return;
        isSubmitting = true;

        $('#btnSave')
            .prop('disabled', true)
            .html('<span class="spinner-border spinner-border-sm me-2"></span>Saving...');

        $form.submit();
    });

    // ✅ Confirm Submit
    $(document).on('click', '#btnConfirmSubmit', () => {
        if (isSubmitting) return;
        isSubmitting = true;

        $("#submitSpinner").show();
        $('#btnSubmit')
            .prop('disabled', true)
            .html('<span class="spinner-border spinner-border-sm me-2"></span>Submitting...');

        $form.submit();
    });
}
