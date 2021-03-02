using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EnumExample
{
    class Program
    {
        public enum CallType
        {
            [Description("Entrante")]
            Inbound,
            [Description("Saliente")]
            Outbound,
            [Description("Perdida")]
            Missed,
            OnHold,
            [Description("Interna")]
            OnNet
        };

        public class CallLog
        {
            public string OriginNumber { get; set; }
            public string DestinationNumber { get; set; }
            public DateTime CallStartDate { get; set; }
            public TimeSpan CallDuration { get; set; }
            public CallType CallType { get; set; }

        };

        /// <summary>
        /// Gets enum description if Description is present
        /// </summary>
        /// <param name="enumValue">Value to get Description</param>
        /// <returns>Field Description or Empty if Description is not provided</returns>
        public static string GetEnumDescription(Enum enumValue)
        {
            string description = "";
            var memberDescription = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute((memberDescription), typeof(DescriptionAttribute));

            if (descriptionAttribute != null)
            {
                description = descriptionAttribute.Description;
            }
            else
            {
                description = enumValue.ToString();
            }

            return description;
        }

        /// <summary>
        /// Mock <see cref="CallLog"/> data
        /// </summary>
        /// <returns>Mocked List<<see cref="CallLog"/>></returns>
        static private List<CallLog> MockCallLogs()
        {
            List<CallLog> callLogList = new List<CallLog>();
            callLogList.Add(new CallLog()
            {
                CallDuration = TimeSpan.FromSeconds(80),
                CallStartDate = DateTime.Now,
                CallType = CallType.OnHold,
                DestinationNumber = "+511111111111",
                OriginNumber = "+50000000000"
            });

            callLogList.Add(new CallLog()
            {
                CallDuration = TimeSpan.FromSeconds(60),
                CallStartDate = DateTime.Now.AddDays(1),
                CallType = CallType.Missed,
                DestinationNumber = "+511111111111",
                OriginNumber = "+50000000000"
            });

            callLogList.Add(new CallLog()
            {
                CallDuration = TimeSpan.FromSeconds(98),
                CallStartDate = DateTime.Now.AddHours(3),
                CallType = CallType.OnNet,
                DestinationNumber = "+511111111111",
                OriginNumber = "+50000000000"
            });

            return callLogList;
        }

        /// <summary>
        /// Old fashion Enum Format
        /// </summary>
        /// <param name="callType"><see cref="CallType"/></param>
        /// <returns></returns>
        static public string FormatCallType(CallType callType)
        {
            var formattedCallType = "";
            switch (callType)
            {
                case CallType.Inbound:
                    formattedCallType = "Entrante";
                    break;
                case CallType.Missed:
                    formattedCallType = "Perdida";
                    break;
                case CallType.OnHold:
                    formattedCallType = "En Espera";
                    break;
                case CallType.Outbound:
                    formattedCallType = "Saliente";
                    break;
            }
            return formattedCallType;
        }

        static void Main(string[] args)
        {
            List<CallLog> callLogList = MockCallLogs();

            var formattedCallLogs = callLogList.Select(c => new {
                c.OriginNumber,
                c.DestinationNumber,
                CallDuration = c.CallDuration.ToString("hh':'mm':'ss"),
                CallStartDate = c.CallStartDate.ToString("dd/MM/yyyy"),
                CallType = GetEnumDescription(c.CallType)
            }).ToList();

            //You can use System
            formattedCallLogs.ForEach(c => {
                System.Console.WriteLine($"Origin Number: {c.OriginNumber}");
                System.Console.WriteLine($"Destination Number: {c.DestinationNumber}");
                System.Console.WriteLine($"Call Duration: {c.CallDuration}");
                System.Console.WriteLine($"Call Date: {c.CallStartDate}");
                System.Console.WriteLine($"Call Type: {c.CallType}");
                System.Console.WriteLine("-------------------------------");
            });

        }

    }
}
