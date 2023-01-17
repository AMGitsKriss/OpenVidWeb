var editVideo = function () {
    // TODO - Autocomplete should be in here, not in _VideoDetails.cshtml
    var availableTags = [];

    function startAutocomplete() {
        $('#Tags').autocomplete({
            minLength: 0,
            source: function (request, response) {
                // delegate back to autocomplete, but extract the last term
                response($.ui.autocomplete.filter(
                    availableTags, extractLast(request.term)));
            },
            focus: function () {
                // prevent value inserted on focus
                return false;
            },
            select: function (event, ui) {
                var terms = split(this.value);
                // remove the current input
                terms.pop();
                // add the selected item
                terms.push(ui.item.value);
                // add placeholder to get the comma-and-space at the end
                terms.push("");
                this.value = terms.join(" ");
                return false;
            }
        });
    }

    function bindEvents() {
        $(document).on('click', '#updateVideoDetails', updateVideo);
        $(document).on('click', '#deleteVideoDetails', deleteVideo);
        $(document).on('change', '#Image', uploadThumbnail);
        $(document).on('click', '#deleteImage', deleteThumbnail);
    }

    function loadVideoDetails(id) {
        $.ajax({
            type: 'GET',
            url: '/Catalog/Edit/VideoDetails/' + id,
            success: function (data) {
                $('#VideoDetails').replaceWith(data);
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    }

    function loadVideoThumbnails(id) {
        $.ajax({
            type: 'GET',
            url: '/Catalog/Edit/VideoThumbnails/' + id,
            success: function (data) {
                $('#Thumbnails').replaceWith(data);
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    }

    function updateVideo() {
        $('#VideoDetails').hide();
        var request = {
            id: $('#Id').val(),
            name: $('#Name').val(),
            description: $('#Description').val(),
            tags: $('#Tags').val(),
            ratingId: $('#RatingId :selected').val()
        }

        $.ajax({
            type: 'POST',
            url: '/Catalog/Edit/VideoDetails',
            data: request,
            success: function (data) {
                loadVideoDetails(request.id);
                $('#VideoDetails').show();
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    }

    function deleteVideo() {
        $('#VideoDetails').hide();
        var request = {
            id: $('#Id').val()
        }

        $.ajax({
            type: 'POST',
            url: '/VideoManagement/FlagDelete',
            data: request,
            success: function (data) {
                loadVideoDetails(request.id);
                $('#VideoDetails').show();
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    }

    function split(val) {
        return val.split(/ \s*/);
    }
    function extractLast(term) {
        return split(term).pop();
    }

    function loadTags() {
        $.ajax({
            type: 'GET',
            url: '/playback/tag/GetTags',
            success: function (data) {
                availableTags = data;
            },
            error: function (error) {
                alert('Unable to get video tag list.');
            }
        });
    }

    function tabToSelect(event) {
        if (event.keyCode === $.ui.keyCode.TAB &&
            $(this).autocomplete("instance").menu.active) {
            event.preventDefault();
        }
    }

    function enterToSave(event) {
        if (event.which === 13 && !event.shiftKey) {
            event.preventDefault();

            updateVideo();
        }
    }

    function toggleTag() {
        var existingTags = $('#Tags').val();
        var thisTag = $(this).attr('data-tag');

        if (existingTags.indexOf(thisTag) == -1) {
            $('#Tags').val(existingTags + ' ' + thisTag + ' ');
            $('[data-tag=' + thisTag + ']').addClass('selected');
        }
        else {
            var regexExp = new RegExp(thisTag, 'g');
            $('#Tags').val(existingTags.replace(regexExp, ''));
            $('[data-tag=' + thisTag + ']').removeClass('selected');
        }
    }

    function uploadThumbnail() {
        var formData = new FormData($('#uploadThumbnail')[0]);

        $.ajax({
            type: 'POST',
            url: '/Catalog/Edit/SaveThumbnail',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.isSuccess) {
                    var id = $('#Id').val();
                    loadVideoThumbnails(id);
                }
                else {
                    alert(data.message);
                }
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    }

    function deleteThumbnail() {
        var id = $('#Id').val();

        $.ajax({
            type: 'POST',
            url: '/Catalog/Edit/DeleteThumbnail/' + id,
            success: function (data) {
                if (data.isSuccess) {
                    loadVideoThumbnails(id);
                }
                else {
                    alert(data.message);
                }
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    }

    return {
        init: function (id) {
            bindEvents();
            loadVideoDetails(id);
            loadVideoThumbnails(id);
            loadTags();
        },
        tags: function () {
            return availableTags;
        },
        updateVideo: function () {
            updateVideo();
        }
    };
}();