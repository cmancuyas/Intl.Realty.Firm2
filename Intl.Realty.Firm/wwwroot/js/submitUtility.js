function initializeSubmitHandlers({
    formId,
    dealStatusDropdownSelector,
    isAdminLike,
    fileErrorScrollTargetId,
    validateTabs = defaultValidateAllTabs,
    validateFilesFn = defaultValidateFiles
}) {
    const $form = $(`#${formId}`);

    // Handle Save/Submit Button Clicks
    $('#btnSave').click(() => $('#ActionTypeHidden').val('save'));
    $('#btnSubmit').click(() => $('#ActionTypeHidden').val('submit'));

    $form.on('submit', function (e) {
        const actionType = $('#ActionTypeHidden').val();
        const hasFieldErrors = validateTabs();

        if (hasFieldErrors) {
            e.preventDefault();
            Swal.fire({
                icon: "error",
                title: "Form Incomplete",
                text: "Please complete all required fields.",
                confirmButtonColor: "#d33"
            });
            return false;
        }

        if (actionType === 'submit') {
            const result = validateFilesFn();

            if (!result.success) {
                e.preventDefault();
                if (fileErrorScrollTargetId) {
                    document.getElementById(fileErrorScrollTargetId)?.scrollIntoView({ behavior: "smooth", block: "center" });
                }

                Swal.fire({
                    icon: "error",
                    title: "Missing Required Documents",
                    html: result.message,
                    confirmButtonColor: "#d33"
                });
                return false;
            }

            // Default DealStatusId = 1 on submit if user is not admin-like
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
            e.preventDefault();
        }

        if (actionType === 'save') {
            if (dealStatusDropdownSelector) {
                $(dealStatusDropdownSelector).val("2"); // Draft
            }

            $('#confirmSaveModal').modal('show');
            e.preventDefault();
        }
    });

    $(document).on('click', '#btnConfirmSave', () => $form.off('submit').submit());

    $(document).on('click', '#btnConfirmSubmit', () => {
        $("#submitSpinner").show();
        $('#btnSubmit').prop('disabled', true).html('<span class="spinner-border spinner-border-sm me-2"></span>Submitting...');
        $form.off('submit').submit();
    });
}


// ✅ Tab Validation (Highlights missing required fields)
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


// ✅ File Validation (Checks new and already uploaded files)
function defaultValidateFiles(documentTypes = []) {
    const entityType = detectEntityType();
    const entityId = detectEntityId();
    const uploadedDocumentTypes = getUploadedDocumentTypes(entityType, entityId);

    let missing = [];

    console.log("🧾 Validating uploaded files...");
    console.log("📎 Server-uploaded document types:", uploadedDocumentTypes);

    $("#tblApplicantFiles tbody tr").each(function () {
        const docTypeId = parseInt($(this).data("document-type-id"));
        const fileInput = $(this).find('input[type="file"]')[0];
        const hasNewFile = fileInput?.files?.length > 0;

        const docType = documentTypes.find(d => d.Id === docTypeId);
        if (!docType) {
            console.warn(`❓ Document type ID ${docTypeId} not in list.`);
            return;
        }

        const isRequired = docType.IsRequired;
        const alreadyUploaded = uploadedDocumentTypes.some(u => {
            const uploadedId = parseInt(u.id ?? u.Id); // Defensive fallback
            return uploadedId === docTypeId;
        });

        if (isRequired) {
            if (alreadyUploaded || hasNewFile) {
                $(fileInput).removeClass("border-danger");
                alreadyUploaded ? showAlreadyUploadedIndicator(fileInput) : removeUploadIndicator(fileInput);
            } else {
                const docLabel = docType.Description ?? `Document`;
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


// ✅ Gets uploaded document types via AJAX (synchronous)
function getUploadedDocumentTypes(entityType, entityId) {
    let uploadedDocs = [];

    if (!entityType || !entityId) {
        console.warn("⚠️ No entity type or ID — skipping file validation for existing uploads.");
        return uploadedDocs;
    }

    $.ajax({
        url: `/${entityType}/GetDocumentTypesThatHasFileUploads/`,
        type: 'GET',
        data: { [`${entityType.toLowerCase()}Id`]: entityId },
        async: false,
        success: function (data) {
            uploadedDocs = data;
        },
        error: function () {
            console.error(`❌ Failed to load uploaded files for ${entityType}`);
        }
    });

    return uploadedDocs;
}


// ✅ Determines ViewModel prefix (e.g., EditSaleListingViewModel)
function detectViewModelPrefix() {
    const input = document.querySelector('input[name*="ViewModel"]');
    if (!input) return "";
    const match = input.name.match(/^(\w+ViewModel)/);
    return match ? match[1] : "";
}


// ✅ Maps ViewModel prefix to entity/controller
function detectEntityType() {
    const prefix = detectViewModelPrefix();
    if (prefix.includes("SaleListing")) return "SaleListing";
    if (prefix.includes("SaleCoop")) return "SaleCoop";
    if (prefix.includes("LeaseListing")) return "LeaseListing";
    if (prefix.includes("LeaseCoop")) return "LeaseCoop";
    return "";
}


// ✅ Extracts entity ID for Edit pages
function detectEntityId() {
    const prefix = detectViewModelPrefix();
    const input = document.querySelector(`input[type="hidden"][name="${prefix}.Id"]`);
    return input ? input.value : null;
}


// ✅ UI helper to show "Uploaded" badge next to file input
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
