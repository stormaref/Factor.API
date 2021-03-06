﻿using Factor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Factor.Tools
{
    public static class StaticTools
    {
        public const string PhoneRegex = "^(0)?9([0-9]{9})$";
        internal const string PhoneValidationError = "Phone format is invalid";

        public static long GenerateCode() => new Random().Next(1000, 9999);

        public static bool PhoneValidator(string phone) => Regex.IsMatch(phone, PhoneRegex);

        public static bool DateChecker(DateTime date, DateTime startDate, DateTime endDate) => (date >= startDate && date <= endDate);

        public static bool IsVerified(this User user) => user.Verification.IsVerified;

        public static bool IsEmpty(this string str) => str == null || string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);

        public static List<string> GetImages(List<Image> images, string url) => (from image in images select string.Format("{0}/api/ImageHandler/GetImage?id={1}", url, image.Id)).ToList();
    }
}
