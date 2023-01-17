var playVideo = function () {
    var videoSources = {};

    function bindEvents() {
        document.addEventListener('shaka-ui-loaded', init);
    }

    async function getUrl() {
        const support = await shaka.Player.probeSupport();

        if (videoSources.mpd !== undefined && support.manifest.mpd)
            return videoSources.mpd;
        if (videoSources.m3u8 !== undefined && support.manifest.m3u8)
            return videoSources.m3u8;
        if (videoSources.mp4 !== undefined)
            return videoSources.mp4;
        if (videoSources.webm !== undefined)
            return videoSources.webm;
        return '';
    }

    async function init() {
        const video = document.getElementById('youtube-theme');
        const ui = video['ui'];
        const config = {
            'seekBarColors': {
                base: 'rgba(255,255,255,.2)',
                buffered: 'rgba(255,255,255,.4)',
                played: 'rgb(255,0,0)',
            },
            'castReceiverAppId': '07AEE832',
            'overflowMenuButtons': ['quality', 'captions', 'language', 'loop', 'playback_rate', 'cast']
        }
        ui.configure(config);

        const controls = ui.getControls();
        const player = controls.getPlayer();

        try {
            await player.load(await getUrl());
        } catch (error) {
        }

        $('.shaka-overflow-menu-button').html('settings');
        $('.shaka-back-to-overflow-button .material-icons-round').html('arrow_back_ios_new');
    }

    function loadVideoDetails(id) {
        $.ajax({
            type: 'GET',
            url: '/playback/Play/VideoDetails/' + id,
            success: function (data) {
                $('#VideoDetails').replaceWith(data);
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    }

    return {
        init: function (id) {
            bindEvents();
        },
        setSource: function (type, url) {
            videoSources[type] = url;
        },
        loadVideoDetails: function (id) {
            loadVideoDetails(id);
        }
    };
}();