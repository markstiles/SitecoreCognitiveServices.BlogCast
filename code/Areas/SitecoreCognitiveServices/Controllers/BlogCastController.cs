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

            var maleRelativePath = $"temp\\blogcast-{page.ID.Guid.ToString("N")}-male.mp3";
            var maleFilePath = $"{Request.PhysicalApplicationPath}{maleRelativePath}";
            var femaleRelativePath = $"temp\\blogcast-{page.ID.Guid.ToString("N")}-female.mp3";
            var femaleFilePath = $"{Request.PhysicalApplicationPath}{femaleRelativePath}";

            var locale = SpeechLocaleOptions.enUS;
            var gender = GenderOptions.Male;
            var audioFormat = AudioOutputFormatOptions.Audio24Khz160KBitRateMonoMp3;

            SpeechService.TextToFile(cleanText, maleFilePath, locale, VoiceName.EnUsGuyNeural, gender, audioFormat);
            SpeechService.TextToFile(cleanText, femaleFilePath, locale, VoiceName.EnUsJessaNeural, gender, audioFormat);

            return Json(new { Succeeded = true });
        }

        public ActionResult RemoveMediaFile(string id, string db, string language)
        {
            Guid g = Guid.Parse(id);          
            var maleRelativePath = $"temp\\blogcast-{g.ToString("N")}-male.mp3";
            var maleFilePath = $"{Request.PhysicalApplicationPath}{maleRelativePath}";
            var femaleRelativePath = $"temp\\blogcast-{g.ToString("N")}-female.mp3";
            var femaleFilePath = $"{Request.PhysicalApplicationPath}{femaleRelativePath}";

            if (System.IO.File.Exists(maleFilePath))
                System.IO.File.Delete(maleFilePath);
            if (System.IO.File.Exists(femaleFilePath))
                System.IO.File.Delete(femaleFilePath);

            return Json(new { Succeeded = true });
        }

        #endregion
    }
}