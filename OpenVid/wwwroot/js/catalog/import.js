var importVideo = function () {

    function bindEvents() {
        $(document).on('click', '#uploadBtn', function (e) {
            $('#uploadFile').click();
        });

        $(document).on('change', '#uploadFile', function (e) {
            var fileUpload = $("#uploadFile").get(0);
            var files = fileUpload.files;
            var multipleFiles = files.length > 1;
            uploadVideos(files, multipleFiles, 0)
        });

        $(document).on('click', '#queueBtn', queueVideos);
        $(document).on('click', '.componentsButton', getComponentsModal);
        $(document).on('click', '#submitSubtitles', uploadSubtitle);
    }

    function uploadVideos(files, multipleFiles, i) {
        var formData = new FormData();
        formData.append('file', files[i]);
        formData.append('multipleFiles', multipleFiles);

        $.ajax({
            type: 'POST',
            url: uploadUrl,
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                $('#updateForm').append(data);
                if (i < files.length - 1) {
                    uploadVideos(files, multipleFiles, i + 1);
                }
                else {
                    location.reload(true);
                }
            },
            error: function (error) {
                $('#updateForm').append("<p>AJAX Error!</p>");
            }
        });
    }

    function queueVideos() {
        $(this).prop('disabled', true);
        $(this).tooltip('hide');

        $.ajax({
            type: 'POST',
            url: queueUrl,
            success: function (data) {
                location.reload(true);
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    }

    function getComponentsModal() {
        var id = $(this).attr('data-id');

        $.ajax({
            type: 'GET',
            url: '/Catalog/Import/VideoSegmentModal/' + id,
            success: function (data) {
                largeModal(data);
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    }

    function uploadSubtitle() {
        var formData = new FormData($('#addSubtites')[0]);

        $.ajax({
            type: 'POST',
            url: '/Catalog/Import/SaveSubtitles',
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


    return {
        init: function () {
            bindEvents();
        }
    };
}();

function largeModal(data) {
    $('#modalLarge .modal-dialog').html(data);
    $('#modalLarge').modal();
}