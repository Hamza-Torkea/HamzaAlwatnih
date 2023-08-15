using System.Web.Optimization;

namespace alwatnia
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
			
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/BasicJs").Include("~/assets/home/js/jquery-1.12.2.min.js",
                "~/assets/global/plugins/bootstrap/js/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/HomeJs").Include("~/assets/home/js/owl.carousel.min.js",
                "~/assets/home/js/scroller1.js",
                "~/assets/home/js/wow.js",
                "~/assets/home/js/script.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/assets/global/plugins/js.cookie.min.js",
                "~/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                "~/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js",
                "~/assets/global/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js",
                "~/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/assets/global/plugins/jquery.blockui.min.js",
                "~/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                "~/assets/global/plugins/bootstrap-confirmation/bootstrap-confirmation.min.js",
                "~/assets/pages/scripts/ui-confirmations.min.js",
                "~/assets/global/plugins/select2/js/select2.full.min.js",
                "~/assets/pages/scripts/bootstrap-tagsinput.js",
                "~/assets/global/plugins/morris/morris.min.js",
                "~/assets/global/plugins/morris/raphael-min.js",
                "~/assets/global/scripts/app.min.js",
                "~/assets/layouts/layout5/scripts/layout.min.js",
                "~/assets/layouts/global/scripts/quick-sidebar.min.js",
                "~/assets/global/plugins/icheck/icheck.min.js",
                "~/assets/global/plugins/jcrop/js/jquery.Jcrop.min.js",
                "~/assets/dropzone/dropzone.min.js",
                "~/assets/jquery.mCustomScrollbar.concat.min.js",
                "~/scripts/tinymce/tinymce.min.js",
				"~/assets/global/plugins/moment.min.js",
                "~/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js",
                "~/assets/global/plugins/cubeportfolio/js/jquery.cubeportfolio.min.js",
                "~/assets/pages/scripts/portfolio-3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include("~/Scripts/angular.js",
                "~/Scripts/app/core.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjsui").Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/angular-ui/ui-bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/validate").Include(
                "~/assets/global/plugins/jquery-validation/js/jquery.validate.min.js",
                "~/assets/global/plugins/jquery-validation/js/additional-methods.min.js",
                "~/assets/pages/scripts/form-validation.js",
                "~/assets/global/plugins/counterup/jquery.waypoints.min.js",
                "~/assets/global/plugins/counterup/jquery.counterup.min.js",
                "~/assets/pages/scripts/dashboard.min.js",
                "~/assets/global/plugins/bootstrap-modal/js/bootstrap-modal.js",
                "~/assets/global/plugins/bootstrap-modal/js/bootstrap-modalmanager.js",
                "~/assets/pages/scripts/jquery.jscroll.js"));
			
            bundles.Add(new StyleBundle("~/Content/BasicCss").Include("~/assets/home/css/bootstrap.css",
                "~/assets/home/css/bootstrap-rtl.min.css",
	            "~/assets/home/css/style.css",
				"~/assets/home/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/BasicCss-ltr").Include("~/assets/home/css/bootstrap.css",
                "~/assets/home/css/bootstrap.min.css",
	            "~/assets/home/css/style-ltr.css",
				"~/assets/home/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/HomeCss").Include("~/assets/home/css/owl.carousel.min.css",
                "~/assets/home/css/owl.theme.default.min.css",
                "~/assets/home/css/animate.css",
                "~/assets/home/css/responsive.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
                "~/assets/global/plugins/bootstrap-switch/css/bootstrap-switch-rtl.min.css",
                "~/assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css",
                "~/assets/global/plugins/bootstrap-timepicker/css/bootstrap-timepicker.min.css",
                "~/assets/global/plugins/select2/css/select2.min.css",
                "~/assets/global/plugins/select2/css/select2-bootstrap.min.css",
                "~/assets/global/plugins/bootstrap-daterangepicker/daterangepicker.min.css",
                "~/assets/global/plugins/morris/morris.css",
                "~/assets/global/plugins/fullcalendar/fullcalendar.min.css",
                "~/assets/global/plugins/jqvmap/jqvmap/jqvmap.css",
                "~/assets/global/plugins/bootstrap-modal/css/bootstrap-modal-bs3patch.css",
                "~/assets/global/plugins/bootstrap-modal/css/bootstrap-modal.css",
                "~/assets/layouts/layout/css/bootstrap-tagsinput.css",
                "~/assets/global/css/components-rounded-rtl.min.css",
                "~/assets/global/css/plugins-rtl.min.css",
                "~/assets/layouts/layout5/css/layout-rtl.min.css",
                "~/assets/layouts/layout5/css/custom-rtl.css",
                "~/assets/global/plugins/icheck/skins/all.css",
                "~/assets/global/plugins/jcrop/css/jquery.Jcrop.min.css",
                "~/assets/global/plugins/nouislider/nouislider.min.css",
                "~/assets/global/plugins/nouislider/nouislider.pips.css",
                "~/assets/pages/css/profile-rtl.min.css",
                "~/assets/dropzone/css/basic.css",
                "~/assets/dropzone/css/dropzone.css",
                "~/assets/layouts/layout5/css/jquery.mCustomScrollbar.css",
                "~/assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.css",
                "~/assets/global/plugins/cubeportfolio/css/cubeportfolio.css",
                "~/assets/pages/css/portfolio-rtl.min.css"));
        }
    }
}