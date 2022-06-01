using IO.ClickSend.ClickSend.Api;
using IO.ClickSend.ClickSend.Model;
using IO.ClickSend.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartAppWS.Utilities
{
    public class SMS
    {
        public static bool SendSMS(string user, string pwd, string from, string To, string sms)
        {

            var configuration = new Configuration()
            {
                Username = user,
                Password = pwd
            };
            var smsApi = new SMSApi(configuration);

            var listOfSms = new List<SmsMessage>
            {
                new SmsMessage(
                    to: To.Replace("-",""),
                    body: sms,
                    from: from,
                    source:"sdk"
                )
            };

            var smsCollection = new SmsMessageCollection(listOfSms);
            var response = smsApi.SmsSendPost(smsCollection);
            return response.Contains("SUCCESS");
        }
    }
}
