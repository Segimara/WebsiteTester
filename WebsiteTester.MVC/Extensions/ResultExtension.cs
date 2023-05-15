using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;

namespace WebsiteTester.MVC.Extensions
{
    public static class ResultExtension
    {
        public static IActionResult ToViewResult<TResult>(this Result<TResult> result,
            ITempDataDictionary dataDir,
            ViewDataDictionary viewDataDir,
            string viewName)
        {
            return result.Match<IActionResult>(r =>
                {
                    viewDataDir.Model = r;

                    return new ViewResult
                    {
                        ViewName = viewName,
                        ViewData = viewDataDir,
                    };
                }, ex =>
                {
                    return GetViewResultBasedOnError(viewName, dataDir, viewDataDir, ex);
                });
        }
        public static IActionResult ToRedirectResult<TResult>(
        this Result<TResult> result,
        ITempDataDictionary tempData,
        string actionName,
        string controllerName)
        {
            return result.Match<IActionResult>(
                _ =>
                {
                    return new RedirectToActionResult(actionName, controllerName, null);
                },
                ex =>
                {
                    return GetRedirectResultBasedOnError(tempData, actionName, controllerName, ex);
                });
        }
        public static IActionResult GetViewResultBasedOnError(string viewName,
            ITempDataDictionary tempData,
            ViewDataDictionary viewData
            , Exception exception)
        {
            switch (exception)
            {
                case ValidationException validationException:
                    tempData["Error"] = validationException.Message;

                    return new ViewResult
                    {
                        ViewName = viewName,
                        ViewData = viewData
                    };
                default:
                    {
                        tempData["Error"] = exception.Message;

                        return new StatusCodeResult(500);
                    }
            }

        }
        public static IActionResult GetRedirectResultBasedOnError(ITempDataDictionary tempData,
        string actionName,
        string controllerName,
        Exception exception)
        {
            switch (exception)
            {
                case ValidationException validationException:
                    tempData["Error"] = exception.Message;

                    return new RedirectToActionResult(actionName, controllerName, null);
                default:
                    {
                        tempData["Error"] = exception.Message;

                        return new StatusCodeResult(500);
                    }
            }

        }
    }
}
