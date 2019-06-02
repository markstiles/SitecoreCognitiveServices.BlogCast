jQuery.noConflict();

//blog cast
jQuery(document).ready(function ()
{
    var container = ".blog-cast";
    var form = container + " .form";
    var formSubmit = form + " .create-submit";
    var progressIndicator = ".progress-indicator";
    var controlButton = container + " .control-button";
    var playButton = controlButton + " .play";
    var pauseButton = controlButton + " .pause";
    var show = "show";
    var audio;

    jQuery(playButton + ", " + pauseButton).click(function (e)
    {
        if(audio == null)
            audio = new Audio(jQuery(controlButton).data("file"));
        if (jQuery(playButton).hasClass(show))
        {
            jQuery(playButton).removeClass(show);
            jQuery(pauseButton).addClass(show);
            try
            {
                audio.play();
            } catch (error) {
                console.log(error);
            }
        }
        else
        {
            jQuery(pauseButton).removeClass(show);
            jQuery(playButton).addClass(show);

            try {
                audio.pause();
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
            jQuery(controlButton).addClass(show);
            jQuery(form).removeClass(show);
        });
    }    
});