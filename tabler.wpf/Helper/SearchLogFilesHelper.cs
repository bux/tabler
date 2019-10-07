using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using ayondo.Base.ExtensionMethods;
using AyondoWinFormHelper.BusinessLogic;
using AyondoWinFormHelper.Enums;
using AyondoWinFormHelper.LogFileApi;
using AyondoWinFormHelper.Wcf;

namespace AyondoWinFormHelper.Helper
{
    public static class SearchLogFilesHelper
    {
        //private const string ServerUrlAdd = ".rz.ayondo.com";

        public static List<RowContainer> GetMessagesFromServiceWithRetries(string serverDns, LogFileType logFileType, DateTime from, DateTime to, List<string> messagesToSearch, List<string> messagesToIgnore)
        {
            if (messagesToSearch == null || messagesToSearch.Any() == false)
            {
                return new List<RowContainer>() { new RowContainer() { Date = DateTime.MinValue, Value = "connection error", FileName = "error.xml", ValueUpperCase = "CONNECTION ERROR" } };

            }
            var include = messagesToSearch != null ? string.Join(",", messagesToSearch) : "";
            var exclude = messagesToIgnore != null ? string.Join(",", messagesToIgnore) : "";

            var bRetry = true;
            int tries = 1;
            while (bRetry)
            {
                try
                {
                   
                    OperationManager.LogGeneral($"Calling logs try:{tries} {serverDns} {logFileType} " +
                                                $"{from.GetDateTimeString_yyyyMMddhhmmss()} - {to.GetDateTimeString_yyyyMMddhhmmss()} " +
                                                $"params to include:{include} " +
                                                $"params exclude: {exclude } ");

                    if (AyondoServiceClients.WcfServiceAst(serverDns).LogfileAndSearch != null)
                    {
                        var result = AyondoServiceClients.WcfServiceAst(serverDns)
                                .LogfileAndSearch.Interface.Search(logFileType,
                                    from,
                                    to,
                                    messagesToSearch,
                                    messagesToIgnore, true, true, false, 1000, false);

                        OperationManager.LogGeneral($"Results: {result.Count}");


                        return result;
                    }
                    else
                    {
                        OperationManager.LogError($"{nameof(SearchLogFilesHelper)}.{nameof(GetMessagesFromServiceWithRetries)} LogfileAndSearch is null");
                    }
                }
                catch (TimeoutException tex)
                {
                    tries++;
                    if (tries > 6)
                    {
                        bRetry = false;
                        OperationManager.LogError($"{nameof(SearchLogFilesHelper)}.{nameof(GetMessagesFromServiceWithRetries)} Exception: {tex}");
                    }
                }
                catch (CommunicationException cex)
                {
                    tries++;
                    if (tries > 6)
                    {
                        bRetry = false;
                        OperationManager.LogError($"{nameof(SearchLogFilesHelper)}.{nameof(GetMessagesFromServiceWithRetries)} Exception: {cex}");
                    }
                }
                catch (Exception ex)
                {
                    bRetry = false;
                    OperationManager.LogError($"{nameof(SearchLogFilesHelper)}.{nameof(GetMessagesFromServiceWithRetries)} {serverDns} - {logFileType} - {from.GetDateTimeString_yyyyMMddhhmmss()} - {to.GetDateTimeString_yyyyMMddhhmmss()} Exception: {ex}");
                }
            }

            return new List<RowContainer>() { new RowContainer() { Date = DateTime.MinValue, Value = "connection error", FileName = "error.xml", ValueUpperCase = "CONNECTION ERROR" } };

        }
    }
}
