using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
                    dataDir["Error"] = ex.Message;

                    return new ViewResult
                    {
                        ViewName = viewName,
                        ViewData = viewDataDir,
                    };
                });
        }
        public static IActionResult ToRedirectResult<TResult>(
        this Result<TResult> result,
        ITempDataDictionary dataDir,
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
                    dataDir["Error"] = ex.Message;

                    return new RedirectToActionResult(actionName, controllerName, null);
                });
        }
    }
}
