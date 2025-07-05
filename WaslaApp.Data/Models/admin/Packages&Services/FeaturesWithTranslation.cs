using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.admin.Packages_Services
{
    public class FeaturesWithTranslation 
    {
        public string? feature_code { get; set; }

        public string? feature_default_name { get; set; }
        public int id { get; set; }

        public int? feature_id { get; set; }

        public string? feature_name { get; set; }

        public string? feature_description { get; set; }

        public string? lang_code { get; set; }
    }

    public class FeaturesWithTranslationGrp
    {
        public string? feature_code { get; set; }
        public int? feature_id { get; set; }
        public string? feature_default_name { get; set; }
        public List<FeaturesWithTranslation> features_Translations { get; set; }
    }
}
