using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.admin.Packages_Services
{
    public class FeaturesWithTranslationGrp
    {
        public string? feature_code { get; set; }
        public int? feature_id { get; set; }
        public string? feature_default_name { get; set; }
        public List<featureswithtranslation> features_Translations { get; set; }
    }
}
