using Factor.Models;
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

        public static long GenerateCode()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }

        public static bool PhoneValidator(string phone)
        {
            return Regex.IsMatch(phone, PhoneRegex);
        }

        public static bool DateChecker(DateTime date, DateTime startDate, DateTime endDate)
        {
            return (date >= startDate && date <= endDate);
        }

        public static List<Contact> Contacts(this User user)
        {
            return user.PreFactors.Select(f => f.SubmittedFactor.Contact).Distinct().ToList();
        }
    }
}
