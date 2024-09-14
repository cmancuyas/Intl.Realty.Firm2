function mainPageLayout($) {
    feather.replace();
    $('[data-toggle="tooltip"]').tooltip();
    // $('.data-table').DataTable();
    $('[id^=dt-search-]').attr('placeholder', 'Search...').addClass('fst-italic');
    //$('.form-select').each(function () {
    //    if ($(this).closest('.table-responsive').length === 0) {
    //        $(this).select2();
    //    }
    //});
    $(".required").each(function () {
        $(this).append('<span class="text-danger ms-1">*</span>');
    });

    setActiveMenuItem($);

    $('.nav-item a').on('click', function () {
        $('.nav-item').removeClass('selected');
        $(this).closest('.nav-item').addClass('selected');
    });

    var navConfigs = [
        { itemId: '#administratorNavItem', subNavClass: '.administratorSubNavItem', caret: '#administratorNavLink .caret' },
        { itemId: '#transactionManagementNavItem', subNavClass: '.transactionManagementSubNavItem', caret: '#transactionManagementNavLink .caret' },
        { itemId: '#informationPackageManagementNavItem', subNavClass: '.informationPackageManagementSubNavItem', caret: '#informationPackageManagementNavLink .caret' },
        { itemId: '#irfProgramManagementNavItem', subNavClass: '.irfProgramManagementSubNavItem', caret: '#irfProgramPackageManagementNavLink .caret' },
        { itemId: '#irfBrandedWebsiteNavItem', subNavClass: '.irfBrandedWebsiteSubNavItem', caret: '#irfBrandedWebsiteNavLink .caret' },
        { itemId: '#purchaseIRFStocksNavItem', subNavClass: '.purchaseIRFStocksSubNavItem', caret: '#purchaseIRFStocksNavLink .caret' },
        { itemId: '#referAnAgentNavItem', subNavClass: '.referAnAgentSubNavItem', caret: '#referAnAgentNavLink .caret' },
    ];

    navConfigs.forEach(function (config) {
        $(config.itemId).click(function () {
            toggleNavItems($,config.itemId, config.subNavClass, config.caret);
        });
    });

    $('#sidebarToggle').click(function ($) {
        $('aside').toggleClass('aside-minimized');
        $('.nav-link').toggleClass('hide-text');

        if ($('aside').hasClass('aside-minimized')) {
            minimizeSidebar();
            toggleAllCarets();
        } else {
            expandSidebar();
        }

        $('.nav-link span').toggle();
    });


    $.get("/Profile/CurrentUserInfos", function (data) {
        console.log(data);
        // if (data.success) {
        //     $('#DisplayName').text(data.displayName);
        //     $('#DisplayRole').text(data.role);
        // }
        $('#DisplayName').text(data.displayName);
        $('#DisplayRole').text(data.role);
        $('#avatar').attr('src', data.profilePictureSource);
    });
}

function toggleNavItems($,clickedNavItem, clickedSubNavItem, clickedCaret) {
    $('.sub-nav-item').not(clickedSubNavItem).slideUp(100);
    $('.nav-link .caret').not(clickedCaret).removeClass('down');

    $(clickedCaret).toggleClass('down');
    $(clickedSubNavItem).slideToggle(100);

    if ($('aside').hasClass('aside-minimized')) {
        $('aside').removeClass('aside-minimized');
        $('.nav-link').removeClass('hide-text');
        $('.nav-link span').show();
        expandSidebar();
        $('.sub-nav-item').hide();
        $(clickedSubNavItem).show();
        $(clickedCaret).addClass('down');
    }
}

function setActiveMenuItem($) {
    var url = window.location.href;
    var navItems = [
        {
            id: 'administratorNavItem',
            links: ["Administrator",
                "User",
                "Role",
                "EmploymentStatus",
                "Department",
                "Province",
                "TransactionType",
                "DocumentType",
                "DocumentTypeAssignment",
                "DealStatus"
            ],
            subNavClass: 'administratorSubNavItem',
            caretId: 'administratorNavLink'
        },
        {
            id: 'transactionManagementNavItem',
            links: [
                "SaleListing",
                "SaleCoop",
                "LeaseListing",
                "LeaseCoop",
            ],
            subNavClass: 'transactionManagementSubNavItem',
            caretId: 'transactionManagementNavLink'
        },
        {
            id: 'informationPackageManagementNavItem',
            links: [
                "BusinessCard",
                "CommissionAdvance"
            ],
            subNavClass: 'informationPackageManagementSubNavItem',
            caretId: 'informationPackageManagementNavLink'
        },
        {
            id: 'irfProgramManagementNavItem',
            links: [
                "IRFMortgage",
                "IRFLead"
            ],
            subNavClass: 'irfProgramManagementSubNavItem',
            caretId: 'irfProgramManagementNavLink'
        },
        {
            id: 'irfBrandedWebsiteNavItem',
            links: [
                "IRFBrandedWebsite"
            ],
            subNavClass: 'irfBrandedWebsiteSubNavItem',
            caretId: 'irfBrandedWebsiteNavLink'
        },
        {
            id: 'purchaseIRFStocksNavItem',
            links: [
                "PurchaseIRFStocks"
            ],
            subNavClass: 'purchaseIRFStocksSubNavItem',
            caretId: 'purchaseIRFStocksNavLink'
        },
        {
            id: 'referAnAgentNavItem',
            links: [
                "ReferAnAgent"
            ],
            subNavClass: 'referAnAgentSubNavItem',
            caretId: 'referAnAgentNavLink'
        }
    ];

    $('.nav-item').removeClass('selected');

    $('.nav-item a').each(function () {
        var itemUrl = $(this).attr('href');
        if (url.includes(itemUrl)) {
            $(this).closest('.nav-item').addClass('selected');
        }
    });

    navItems.forEach(function (navItem) {
        navItem.links.forEach(function (link) {
            var regex = new RegExp('\\b' + link + '\\b');
            if (regex.test(url)) {
                $('#' + navItem.id).addClass('selected');
                if (!$('aside').hasClass('aside-minimized')) {
                    $('.' + navItem.subNavClass).show();
                    $('#' + navItem.caretId + ' .caret').addClass('down');
                }
            }
        });
    });
}

function minimizeSidebar() {
    $('.nav-link').addClass('justify-content-center').removeClass('ms-1 ms-2');
    $('.nav-item').not('#workflowsTab .nav-item').addClass('mx-4');
    $('.logo-container').removeClass('me-1');
    $('.logo-container img').removeClass('w-25').addClass('w-50');
    $('.top-bar-container').css('margin-left', '100px');
    $('.main').css('padding-left', '120px');
    $('#sidebarToggle').css({
        'right': '34.5px',
        'top': '84.5px'
    });
    $('.sub-nav-item').css('display', 'none');
    $('.caret').css('display', 'none');
    $('footer').css('padding-left', '60px');
}

function expandSidebar() {
    $('.nav-link').removeClass('justify-content-center').addClass('ms-1');
    $('.nav-item').not('#workflowsTab .nav-item').removeClass('mx-4').addClass('mx-2');
    $('.logo-container').addClass('me-1');
    $('.logo-container img').removeClass('w-50').addClass('w-25');
    $('.top-bar-container').css('margin-left', '300px');
    $('.main').css('padding-left', '320px');
    $('#sidebarToggle').css({
        'right': '25px',
        'top': '28px'
    });
    $('.caret').css('display', 'block');
    $('.sub-nav-item').hide();
    $('footer').css('padding-left', '180px');
    $('[data-toggle="tooltip"]').tooltip('hide');
}

function toggleAllCarets() {
    $('#administratorNavLink .caret').toggleClass('down');
    $('#transactionManagementNavLink .caret').toggleClass('down');
    $('#informationPackageManagementNavLink .caret').toggleClass('down');
    $('#irfProgramManagementNavLink .caret').toggleClass('down');
    $('#irfBrandedWebsiteNavLink .caret').toggleClass('down');
    $('#purchaseIRFStocksNavLink .caret').toggleClass('down');
    $('#referAnAgentNavLink .caret').toggleClass('down');

}