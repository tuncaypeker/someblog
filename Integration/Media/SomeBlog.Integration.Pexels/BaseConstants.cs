﻿namespace SomeBlog.Integration.Pexels
{
    public static class BaseConstants
    {
        public const string API_URL = "https://api.pexels.com/";
        public const string API_URL_VERSION = "v1/";
        public const int REQUEST_TIMEOUT_SECS = 30;
        public const string VERSION = "1.0.8";
        public static readonly string[] SIZES = { "small", "medium", "large" };
        public static readonly string[] ORIENTATIONS = { "landscape", "portrait", "square" };
        public static readonly string[] COLORS = { "red", "orange", "yellow", "green", "turquoise", "blue", "violet", "pink", "brown", "black", "gray", "white" };
    }
}
