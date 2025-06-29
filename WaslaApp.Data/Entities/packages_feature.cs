using System;
using System.Collections.Generic;

namespace WaslaApp.Data.Entities;

public partial class packages_feature
{
    public int id { get; set; }

    public int? package_id { get; set; }

    public int feature_id { get; set; }
}
