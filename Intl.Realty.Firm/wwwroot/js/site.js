// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
}

function updateUrlParameter(param, value) {
    var url = window.location.href;
    var newUrl = url.replace(new RegExp('([?&])' + param + '=[^&#]*'), '$1' + param + '=' + value);
    if (newUrl === url) {
        newUrl = url + (url.indexOf('?') > -1 ? '&' : '?') + param + '=' + value;
    }
    window.history.replaceState({}, '', newUrl);
}

function handleToastrNotification(param, message) {
    if (getUrlParameter(param) === 'True') {
        toastr.success(message, '', {
            timeOut: 10000,
            closeButton: true,
            onHidden: function () {
                updateUrlParameter(param, '');
            }
        });
    }
}