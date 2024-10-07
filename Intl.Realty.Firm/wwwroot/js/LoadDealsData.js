function loadTopBarPaginationCurrentMonth(action, pageIndex, pageSize, searchText, sortExpression) {
    debugger;
    $.get('@Url.Action("LoadDashboardData", "Home")', { action, pageIndex, pageSize, searchText, sortExpression }, function (data) {

        $("#currentMonthPartial").html(data);
        $("#currentMonthTableBody").html(data);
        $("#currentMonthTopBar").html(data);
        $("#currentMonthBottomBar").html(data);
    });
}
function loadTopBarPaginationAllDeals(action, pageIndex, pageSize, searchText, sortExpression) {
    debugger;
    $.get('@Url.Action("LoadDashboardData", "Home")', { action, pageIndex, pageSize, searchText, sortExpression }, function (data) {

        $("#allMonthsPartial").html(data);
        $("#allMonthsTableBody").html(data);
        $("#allMonthsTopBar").html(data);
        $("#allMonthsBottomBar").html(data);
    });
}

function loadDashboard(sortExpression, searchText, pageIndex, pageSize, tableName) {

    debugger;

    $.ajax({
        url: '/Home/LoadDashboardData', // Adjust this URL to match your handler
        type: 'GET',
        data: {
            sortExpression: sortExpression,
            searchText: searchText,
            pageIndex: pageIndex,
            pageSize: pageSize,
            tableName: tableName
        },
        async:false,
        success: function (result) {
            if (tableName === "currentMonth") {
                $("#currentMonthPartial").html(result.currentMonthPartial);
                $("#currentMonthTableBody").html(result.tableBodyHtml);
                 $("#currentMonthTopBar").html(result.topBarHtml);
                 $("#currentMonthBottomBar").html(result.bottomBarHtml);
            } else if (tableName === "allMonths") {
                $("#allMonthsPartial").html(result.allMonthsPartial);
                $("#allMonthsTableBody").html(result.tableBodyHtml);
                 $("#allMonthsTopBar").html(result.topBarHtml);
                 $("#allMonthsBottomBar").html(result.bottomBarHtml);
            }
        },
        error: function () {
            alert("Error loading data.");
        }
    });
}

