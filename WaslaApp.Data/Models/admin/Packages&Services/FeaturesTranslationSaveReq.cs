﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaslaApp.Data.Entities;

namespace WaslaApp.Data.Models.admin.Packages_Services
{
    public class FeaturesTranslationSaveReq : features_translation
    {
        public bool? delete { get; set; }
    }
}
