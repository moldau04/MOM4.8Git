using System.Web;
using System.Web.Optimization;

namespace MOMWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            // create an object of ScriptBundle and 
            // specify bundle name (as virtual path) as constructor parameter 
            ScriptBundle WebFrom_scriptDesignjs = new ScriptBundle("~/WebFrom_bundles/js");
            //use Include() method to add all the script files with their paths 
            WebFrom_scriptDesignjs.Include(
                                "~/Design/js/UI/jquery-ui.js",
                                "~/Design/js/materialize-plugins/forms.js",
                                 "~/Design/js/Notifyjs/jquery.noty.js",
                                  "~/Design/js/Notifyjs/themes/default.js",
                                   "~/Design/js/Notifyjs/layouts/topCenter.js",
                                   "~/js/date.format.js",
                                "~/js/tether.min.js",
                                 "~/js/bootstrap.min.js",
                                  "~/js/bootstrap-notify.js",
                                   "~/js/datediff.js"
                              );
            ScriptBundle WebFrom_scriptDesignjs1 = new ScriptBundle("~/WebFrom_bundles/js1");
            //use Include() method to add all the script files with their paths 
            WebFrom_scriptDesignjs1.Include(
                                "~/Design/js/jquery.mCustomScrollbar.js",
                                "~/Design/js/materialize.js",
                                 "~/Design/js/plugins/prism/prism.js",
                                "~/Design/js/plugins/perfect-scrollbar/perfect-scrollbar.min.js",
                                   "~/Design/js/plugins.js",
                                   "~/Design/js/custom-script.js",
                                "~/Design/js/moment.js",
                                 "~/Design/js/pikaday.js",
                                  "~/Design/js/pikaday.jquery.js",
                                   "~/js/datediff.js"
                              );

            //Add the bundle into BundleCollection
            bundles.Add(WebFrom_scriptDesignjs);
            bundles.Add(WebFrom_scriptDesignjs1);
            bundles.Add(new StyleBundle("~/WebFrom_bundles/css").Include(
                                                   "~/Design/css/materialize.css",
                                                   "~/Design/css/style.css",
                                                    "~/Design/css/custom/custom.css",
                                                    "~/Design/js/plugins/prism/prism.css",
                                                    "~/Design/js/plugins/perfect-scrollbar/perfect-scrollbar.css",
                                                    "~/Design/css/pikaday.css",
                                                    "~/Design/css/grid.css"
                                               ));

            BundleTable.EnableOptimizations = true;


        }
    }
}
