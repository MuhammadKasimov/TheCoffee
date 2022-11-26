﻿using System.IO;

namespace StarBucks.Service.Helpers
{
    public class EnvironmentHelper
    {
        public static string WebRootPath { get; set; }
        public static string AttachmentPath => Path.Combine(WebRootPath, "images");
        public static string FilePath => "images";
    }
}
