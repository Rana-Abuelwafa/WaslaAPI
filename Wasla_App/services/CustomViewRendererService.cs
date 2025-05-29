using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Wasla_App.services
{
    public class CustomViewRendererService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public CustomViewRendererService(
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderViewToStringAsync(string viewPath, object model, ControllerContext actionContext)
        {
            var viewEngineResult = _razorViewEngine.GetView(viewPath, viewPath, false);

            if (viewEngineResult.View == null || (!viewEngineResult.Success))
            {
                throw new ArgumentNullException($"Unable to find view '{viewPath}'");
            }

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), actionContext.ModelState);
            viewDictionary.Model = model;

            var view = viewEngineResult.View;
            var tempData = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);

            using var sw = new StringWriter();
            var viewContext = new ViewContext(actionContext, view, viewDictionary, tempData, sw, new HtmlHelperOptions());
            await view.RenderAsync(viewContext);
            return sw.ToString();
        }
    //    public async Task<string> RenderViewToStringAsync(
    //string viewName, object model,
    //ControllerContext controllerContext,
    //bool isPartial = false)
    //    {
    //        var actionContext = controllerContext as ActionContext;

    //        var serviceProvider = controllerContext.HttpContext.RequestServices;
    //        var razorViewEngine = serviceProvider.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
    //        var tempDataProvider = serviceProvider.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

    //        using (var sw = new StringWriter())
    //        {
    //            var viewResult = razorViewEngine.FindView(actionContext, viewName, !isPartial);

    //            if (viewResult?.View == null)
    //                throw new ArgumentException($"{viewName} does not match any available view");

    //            var viewDictionary =
    //                new ViewDataDictionary(new EmptyModelMetadataProvider(),
    //                    new ModelStateDictionary())
    //                { Model = model };

    //            var viewContext = new ViewContext(
    //                actionContext,
    //                viewResult.View,
    //                viewDictionary,
    //                new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
    //                sw,
    //                new HtmlHelperOptions()
    //            );

    //            await viewResult.View.RenderAsync(viewContext);
    //            return sw.ToString();
    //        }
        //}
    }
}
