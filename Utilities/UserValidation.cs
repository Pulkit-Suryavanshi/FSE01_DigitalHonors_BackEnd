using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace com.tweetapp.Utilities
{
    public class UserValidation
    {
        public static bool PhoneNo(long contactNo)
        {
            string pattern = @"^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}";
            return Regex.IsMatch(contactNo.ToString(), pattern);
        }
        public static bool Email(string email)
        {
            string pattern = "^\\S+@\\S+\\.\\S+$";
            bool resultMatchMail = Regex.IsMatch(email,pattern);
            return resultMatchMail;
        }
        public static bool UserName(string userName)
        {
            string pattern = @"^[a-zA-Z0-9.]{8,20}$";
            return Regex.IsMatch(userName, pattern);
        }
        public static bool Password(string password)
        {
            string pattern = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
            bool resultMatchPassword = Regex.IsMatch(password, pattern);
            return resultMatchPassword;
        }
        public static string PasswordErrorMessage()
        {
            return "Your password must meet the following:\n*Atleast one digit [0-9]\n*Atleast one lowercase character [a-z]\n*Atleast one uppercase character [A-Z]\n*Atleast 8 characters in length";
        }
    }
}
