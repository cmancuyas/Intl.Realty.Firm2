function initializeSubmitHandlers({
    formId,
    dealStatusDropdownSelector,
    isAdminLike,
    validateTabs = defaultValidateAllTabs,
    validateFilesFn = defaultValidateFiles,
    fileErrorScrollTargetId
}) {
    const $form = $(`#${formId}`);

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
            const fileValidation = validateFilesFn();
            if (!fileValidation.success) {
                e.preventDefault();
                if (fileErrorScrollTargetId) {
                    document.getElementById(fileErrorScrollTargetId)?.scrollIntoView({ behavior: "smooth", block: "center" });
                }

                Swal.fire({
                    icon: "error",
                    title: "Missing Required Documents",
                    html: fileValidation.message,
                    confirmButtonColor: "#d33"
                });
                return false;
            }

            // ✅ Agent always defaults to status = 1
            if (!isAdminLike && dealStatusDropdownSelector) {
                // Fallback: manually inject hidden field if dropdown isn't present
                if ($(dealStatusDropdownSelector).length === 0) {
                    $('#ListFormId').append('<input type="hidden" name="EditLeaseCoopViewModel.DealStatusId" value="1" />');
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

    // Final confirmations
    $(document).on('click', '#btnConfirmSave', function () {
        $form.off('submit').submit();
    });

    $(document).on('click', '#btnConfirmSubmit', function () {
        $("#submitSpinner").show();
        $('#btnSubmit').prop('disabled', true).html('<span class="spinner-border spinner-border-sm me-2"></span>Submitting...');
        $form.off('submit').submit();
    });
}



// ✅ Validates required fields across all tabs
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


// ✅ Validates required document types (new or uploaded)
function defaultValidateFiles(documentTypes = []) {
    const entityType = detectEntityType();
    const entityId = detectEntityId();
    const uploadedDocumentTypes = getUploadedDocumentTypes(entityType, entityId);

    let missing = [];

    console.log("🧾 Starting file validation...");
    console.log("📎 Uploaded Document Types:", uploadedDocumentTypes);

    $("#tblApplicantFiles tbody tr").each(function () {
        const docTypeId = parseInt($(this).data("document-type-id"));
        const fileInput = $(this).find('input[type="file"]')[0];
        const hasNewFile = fileInput?.files?.length > 0;

        const docType = documentTypes.find(d => d.Id === docTypeId);
        if (!docType) {
            console.warn(`❓ Document type ID ${docTypeId} not found in provided documentTypes list.`);
            return;
        }

        const isRequired = docType.IsRequired;
        const alreadyUploaded = uploadedDocumentTypes.some(u => {
            const uploadedId = parseInt(u.id ?? u.Id); // defensive
            return uploadedId === docTypeId;
        });

        console.log(`📄 Checking: "${docType.Description}" (ID: ${docTypeId})`);
        console.log(`   ↪ Required: ${isRequired}, Already Uploaded: ${alreadyUploaded}, Has New File: ${hasNewFile}`);

        if (isRequired && alreadyUploaded) {
            $(fileInput).removeClass("border-danger");
            showAlreadyUploadedIndicator(fileInput);
            console.log(`✅ Already uploaded. Skipping validation.`);
            return;
        }

        if (isRequired && !hasNewFile) {
            const docLabel = docType.Description ?? `Document`;
            missing.push(`<li>${docLabel}</li>`);
            $(fileInput).addClass("border-danger");
            removeUploadIndicator(fileInput);
            console.log(`❌ Missing required document: ${docLabel}`);
        } else {
            $(fileInput).removeClass("border-danger");
            removeUploadIndicator(fileInput);
        }
    });

    if (missing.length > 0) {
        console.warn("🚫 Validation failed. Missing documents:", missing);
        return {
            success: false,
            message: `<b>The following required documents are missing:</b><ul>${missing.join("")}</ul>`
        };
    }

    console.log("✅ All required files are present.");
    return { success: true, message: "" };
}





// ✅ Fetch full document type objects that were uploaded
function getUploadedDocumentTypes(entityType, entityId) {
    let uploadedDocs = [];

    if (!entityType || !entityId) {
        console.warn("Missing entityType or entityId for uploaded doc check.");
        return uploadedDocs;
    }

    $.ajax({
        url: `/${entityType}/GetDocumentTypesThatHasFileUploads/`,
        type: 'GET',
        data: { [`${entityType.toLowerCase()}Id`]: entityId },
        async: false,
        success: function (data) {
            uploadedDocs = data; // full objects with Id, IsRequired, etc.
        },
        error: function () {
            console.error(`Failed to fetch uploaded document types for ${entityType}`);
        }
    });

    return uploadedDocs;
}

// ✅ Detect ViewModel prefix from form inputs
function detectViewModelPrefix() {
    const input = document.querySelector('input[name*="ViewModel"]');
    if (!input) return "";
    const match = input.name.match(/^(\w+ViewModel)/);
    return match ? match[1] : "";
}


// ✅ From prefix → get entity
function detectEntityType() {
    const prefix = detectViewModelPrefix();
    if (prefix.includes("SaleListing")) return "SaleListing";
    if (prefix.includes("SaleCoop")) return "SaleCoop";
    if (prefix.includes("LeaseListing")) return "LeaseListing";
    if (prefix.includes("LeaseCoop")) return "LeaseCoop";
    return "";
}


// ✅ Detect ID from hidden input
function detectEntityId() {
    const prefix = detectViewModelPrefix();
    const input = document.querySelector(`input[type="hidden"][name="${prefix}.Id"]`);
    return input ? input.value : null;
}

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


