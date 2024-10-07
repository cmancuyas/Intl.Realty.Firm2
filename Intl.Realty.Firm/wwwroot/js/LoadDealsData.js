function loadDashboard(sortExpression, searchText, pageIndex, pageSize, tableName) {
    $.ajax({
        url: '/Home/LoadDashboardData', // Adjust this URL to match your handler
        data: {
            sortExpression: sortExpression,
            searchText: searchText,
            pageIndex: pageIndex,
            pageSize: pageSize,
            tableName: tableName
        },
        async: false,
        success: function (data) {

            if (tableName === "CurrentMonth") {
                $('#currentMonthPartial').html(data.tableHtml); // Update the table content
                //$('#current-month-table-body').html(data.tableBodyHtml); // Update the table content
                //$('#current-month-top-bar').html(data.topBarHtml); // Update the top pagination bar
                //$('#current-month-bottom-bar').html(data.bottomBarHtml); // Update the bottom pagination bar


                //$('#dashboardContent').html(data.dashboardViewModel);
            } else if (tableName === "AllMonths") {
                $('#allMonthsPartial').html(data.tableHtml);
                //$('#all-months-table-body').html(data.tableBodyHtml);
                //$('#all-months-top-bar').html(data.topBarHtml);
                //$('#all-months-bottom-bar').html(data.bottomBarHtml);

                //$('#dashboardContent').html(data.dashboardViewModel);
            }
        },
        error: function () {
            alert("Error loading data.");
        }
    });
}

function loadTopBarPaginationCurrentMonth(action, pageIndex, pageSize, searchText, sortExpression, tableName) {
    debugger;

    $.ajax({
        url: '/Home/LoadDashboardData', // Adjust this URL to match your handler
        data: {
            sortExpression: sortExpression,
            searchText: searchText,
            pageIndex: pageIndex,
            pageSize: pageSize,
            tableName: tableName
        },
        async: false,
        success: function (data) {

            if (tableName === "CurrentMonth") {
                //$('#currentMonthPartial').html(data.tableHtml); // Update the table content
                $('#current-month-table-body').html(data.tableBodyHtml); // Update the table content
                $('#current-month-top-bar').html(data.topBarHtml); // Update the top pagination bar
                $('#current-month-bottom-bar').html(data.bottomBarHtml); // Update the bottom pagination bar


                //$('#dashboardContent').html(data.dashboardViewModel);
            } else if (tableName === "AllMonths") {
                //$('#allMonthsPartial').html(data.tableHtml);
                $('#all-months-table-body').html(data.tableBodyHtml);
                $('#all-months-top-bar').html(data.topBarHtml);
                $('#all-months-bottom-bar').html(data.bottomBarHtml);

                //$('#dashboardContent').html(data.dashboardViewModel);
            }
        },
        error: function () {
            alert("Error loading data.");
        }
    });
}

function loadTopBarPaginationAllMonths(action, pageIndex, pageSize, searchText, sortExpression, tableName) {
    $.ajax({
        url: '/Home/ViewAllDeals', // Adjust this URL to match your handler
        data: {
            sortExpression: sortExpression,
            searchText: searchText,
            pageIndex: pageIndex,
            pageSize: pageSize,
            tableName: tableName
        },
        async: false,
        success: function (data) {

            if (tableName === "CurrentMonth") {
                //$('#currentMonthPartial').html(data.tableHtml); // Update the table content
                //$('#current-month-table-body').html(data.tableBodyHtml); // Update the table content
                //$('#current-month-top-bar').html(data.topBarHtml); // Update the top pagination bar
                //$('#current-month-bottom-bar').html(data.bottomBarHtml); // Update the bottom pagination bar


                //$('#dashboardContent').html(data.dashboardViewModel);
            } else if (tableName === "AllMonths") {
                //$('#allMonthsPartial').html(data.tableHtml);
                //$('#all-months-table-body').html(data.tableBodyHtml);
                //$('#all-months-top-bar').html(data.topBarHtml);
                //$('#all-months-bottom-bar').html(data.bottomBarHtml);

                //$('#dashboardContent').html(data.dashboardViewModel);
            }
        },
        error: function () {
            alert("Error loading data.");
        }
    });
}