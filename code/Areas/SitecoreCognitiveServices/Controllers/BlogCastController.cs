using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

            var sb = new StringBuilder();
            var tags = new List<string> { "<sub", "</sub>", "<sup", "</sup>" };
            var arr = html.Split(tags.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var shouldRemove = false;
            for (var i = 0; i < arr.Length; i++)
            {
                var el = arr[i];
                if (!el.StartsWith(">") && !shouldRemove)
                    sb.Append(el);

                shouldRemove = el.Contains("/");
            }

            var cleanText = Regex.Replace(sb.ToString(), "<.*?>", " ").Replace("\"", "").Replace("&", " and ").Replace("   ", " ").Replace("  ", " ");

            var relativePath = $"temp\\blogcast-{page.ID.Guid.ToString("N")}.mp3";
            var filePath = $"{Request.PhysicalApplicationPath}{relativePath}";

            var locale = SpeechLocaleOptions.enUS;
            var voice = VoiceName.EnUsGuyNeural;
            var gender = GenderOptions.Male;
            var audioFormat = AudioOutputFormatOptions.Audio24Khz160KBitRateMonoMp3;

            SpeechService.TextToFile(cleanText, filePath, locale, voice, gender, audioFormat);

            return Json(new { Succeeded = true });
        }

        public ActionResult RemoveMediaFile(string id, string db, string language)
        {
            Guid g = Guid.Parse(id);          
            var relativePath = $"temp\\blogcast-{g.ToString("N")}.mp3";
            var filePath = $"{Request.PhysicalApplicationPath}{relativePath}";

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            return Json(new { Succeeded = true });
        }

        #endregion
    }
}