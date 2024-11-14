using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Client.Helpers
{
    public static class ToastHelper
    {
        public static void ShowInfo(ITempDataDictionary tempData, string message)
        {
            AddToastMessage(tempData, "info", message);
        }

        public static void ShowSuccess(ITempDataDictionary tempData, string message)
        {
            AddToastMessage(tempData, "success", message);
        }

        public static void ShowError(ITempDataDictionary tempData, string message)
        {
            AddToastMessage(tempData, "error", message);
        } 
        
        public static void ShowWarning(ITempDataDictionary tempData, string message)
        {
            AddToastMessage(tempData, "warning", message);
        }

        private static void AddToastMessage(ITempDataDictionary tempData, string type, string message)
        {
            tempData["ToastMessage"] = message;
            tempData["ToastType"] = type;
        }
    }
}
