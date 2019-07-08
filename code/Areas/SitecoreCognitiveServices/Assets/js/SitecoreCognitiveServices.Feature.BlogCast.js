jQuery.noConflict();

//blog cast
jQuery(document).ready(function ()
{
    var container = ".blog-cast";
    var form = container + " .form";
    var formSubmit = form + " .create-submit";
    var progressIndicator = ".progress-indicator";
    var controlButtonMale = container + " .control-button-male";
    var playButtonMale = controlButtonMale + " .play";
    var pauseButtonMale = controlButtonMale + " .pause";
    var controlButtonFemale = container + " .control-button-female";
    var playButtonFemale = controlButtonFemale + " .play";
    var pauseButtonFemale = controlButtonFemale + " .pause";
    var show = "show";
    var removeButton = ".remove-button";
    var audioPlayerMale = "#audio-player-male";
    var audioPlayerFemale = "#audio-player-female";

    //play male audio
    jQuery(playButtonMale + ", " + pauseButtonMale).click(function (e)
    {
        var audio = jQuery(audioPlayerMale);
        if (jQuery(playButtonMale).hasClass(show))
        {
            jQuery(playButtonMale).removeClass(show);
            jQuery(pauseButtonMale).addClass(show);
            try
            {
                audio[0].load();
                audio[0].play();
            } catch (error) {
                console.log(error);
            }
        }
        else
        {
            jQuery(pauseButtonMale).removeClass(show);
            jQuery(playButtonMale).addClass(show);

            try {
                audio[0].pause();
            } catch (error) {
                console.log(error);
            }
        }
    });

    jQuery(playButtonFemale + ", " + pauseButtonFemale).click(function (e)
    {
        var audio = jQuery(audioPlayerFemale);
        if (jQuery(playButtonFemale).hasClass(show))
        {
            jQuery(playButtonFemale).removeClass(show);
            jQuery(pauseButtonFemale).addClass(show);
            try {
                audio[0].load();
                audio[0].play();
            } catch (error) {
                console.log(error);
            }
        }
        else {
            jQuery(pauseButtonFemale).removeClass(show);
            jQuery(playButtonFemale).addClass(show);

            try {
                audio[0].pause();
            } catch (error) {
                console.log(error);
            }
        }
    });

    jQuery(formSubmit).click(function (e) {
        e.preventDefault();

        CreateMediaItem();
    });
    
    function CreateMediaItem()
    {
        var idValue = jQuery(form + " #id").val();
        var dbValue = jQuery(form + " #db").val();
        var langValue = jQuery(form + " #language").val();

        jQuery(progressIndicator).show();

        jQuery.post(
            jQuery(form).attr("action"),
            {
                id: idValue,
                db: dbValue,
                language: langValue
            }
        ).done(function (r) {
            jQuery(progressIndicator).hide();
            jQuery(controlButtonMale).addClass(show);
            jQuery(controlButtonFemale).addClass(show);
            jQuery(removeButton).addClass(show);
            jQuery(form).removeClass(show);            
        });
    } 

    jQuery(removeButton).click(function (e) {
        e.preventDefault();

        RemoveMediaItem();
    });

    function RemoveMediaItem()
    {
        var idValue = jQuery(form + " #id").val();
        var dbValue = jQuery(form + " #db").val();
        var langValue = jQuery(form + " #language").val();

        jQuery(progressIndicator).show();

        jQuery.post(
            jQuery(form).attr("remove-action"),
            {
                id: idValue,
                db: dbValue,
                language: langValue
            }
        ).done(function (r)
        {
            jQuery(progressIndicator).hide();
            jQuery(form).addClass(show);
            jQuery(controlButtonMale).removeClass(show);
            jQuery(controlButtonFemale).removeClass(show);
            jQuery(removeButton).removeClass(show);
        });
    } 
});