// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).on('click', '.loadMore', function () {
    loadMore($(this).attr('data-page'), $('#searchPages').attr('data-search-query'));
    if ($('.pagination').length > 0) {
        updateUrlWithPage($(this).attr('data-page'));
    }
});

function loadMore(page, searchQuery) {
    $.ajax({
        type: 'GET',
        url: '/Playback/VideoList?searchString=' + searchQuery + '&pageNo=' + page,
        success: function (data) {
            $('.newPage').replaceWith(data);
        },
        error: function (error) {
            $('.newPage').replaceWith("<p>There was an error.</p> <p>"+error.responseText+"</p>");
        }
    });
}

function updateUrlWithPage(page) {
    if (history.pushState) {
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?page=' + page;
        window.history.pushState({ path: newurl }, '', newurl);
    }
}

function getPageId() {
    var url = window.location.href;

    if (url.lastIndexOf('#') > 0)
        url = url.substring(0, url.lastIndexOf('#') + 1);

    if (url.lastIndexOf('?') > 0)
        url = url.substring(0, url.lastIndexOf('?') + 1);

    var id = url.substring(url.lastIndexOf('/') + 1);

    return parseInt(id);
}

// TODO - This will always reload on page 1. Not nice.
$(window).on("popstate", function (e) {
    if (e.originalEvent.state !== null) {
        var urlParams = new URLSearchParams(window.location.search);
        if (urlParams.has('page'))
            loadMore(urlParams.get('page'), $('#SearchString').val());
    }
    else {
        location.reload();
    }
});

$(document).on('submit', '#searchBar', function (e) {
    e.preventDefault();
    searchString = $('#SearchString').val();
    if (searchString != "")
        window.location.href = $(this).attr('data-action') + searchString;
});