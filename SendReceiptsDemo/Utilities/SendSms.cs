﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrezSmsSampleCore3.Content
{
    public class SendSms
    {
        // انکد کردن به بیس 64
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public long GetTimeStamp(DateTime dt, DateTimeKind dtk)
        {
            Int64 unixTimestamp = (Int64)(dt.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, dtk))).TotalSeconds;
            return unixTimestamp;
        }
        public class SendMessageResult
        {
            public string Code { get; set; }
            public string Message { get; set; }
            public string Result { get; set; }
        }
    }
}