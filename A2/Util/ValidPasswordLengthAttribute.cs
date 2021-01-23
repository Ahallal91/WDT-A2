using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A2.Util
{
    /*
     Reference for creating own annotation: https://www.infoworld.com/article/3543302/how-to-use-data-annotations-in-csharp.html
     */
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ValidPasswordLengthAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = value as string;
            return inputValue.Length >= 64;
        }
    }
}
