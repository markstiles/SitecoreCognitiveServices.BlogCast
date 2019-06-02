using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SitecoreCognitiveServices.Foundation.MSSDK.Enums;
using SitecoreCognitiveServices.Foundation.SCSDK.Services.MSSDK.Speech;
using SitecoreCognitiveServices.Foundation.SCSDK.Wrappers;

namespace SitecoreCognitiveServices.Feature.BlogCast.Areas.SitecoreCognitiveServices.Controllers
{
    public class BlogCastController : Controller
    {
        #region Constructor
        
        protected readonly ISitecoreDataWrapper DataWrapper;
        protected readonly ISpeechService SpeechService;

        public BlogCastController(
            ISitecoreDataWrapper dataWrapper,
            ISpeechService speechService)
        {
            DataWrapper = dataWrapper;
            SpeechService = speechService;
        }

        #endregion
        
        #region BlogCast

        public ActionResult CreateMediaFile(string id, string db, string language)
        {
            var page = DataWrapper.GetItemByIdValue(id, db);
            var html = page.Fields["Content"].Value;

            var cleanText = Regex.Replace(html, "<.*?>", " ").Replace("\"", "").Replace("&", " and ").Replace("   ", " ").Replace("  ", " ");

            var relativePath = $"temp\\blogcast-{page.ID.Guid.ToString("N")}.mp3";
            var filePath = $"{Request.PhysicalApplicationPath}{relativePath}";

            var locale = SpeechLocaleOptions.enUS;
            var voice = VoiceName.EnUsGuyNeural;
            var gender = GenderOptions.Male;
            var audioFormat = AudioOutputFormatOptions.Audio24Khz160KBitRateMonoMp3;

            SpeechService.TextToFile(cleanText, filePath, locale, voice, gender, audioFormat);

            return Json(new { Succeeded = true });
        }

        #endregion
    }
}