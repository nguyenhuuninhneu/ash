using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Recaptcha.Models
{
    [DataContract]
    public class RecaptchaResult
    {
        //attribute có tên trùng với field của json trả về
        [DataMember(Name = "success")]
        public bool Success { get; set; } 
        //attribute có tên trùng với field của json trả về
        [DataMember(Name = "error-codes")]
        public string[] ErrorCodes { get; set; } 
    }
}