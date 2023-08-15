using System;
using System.Web.Mvc;

namespace alwatnia.Views.Shared
{
    public static class Nav
    {
        public static string ActivePageKey => "ActivePage";
        public static string ActiveMenuKey => "ActiveMenu";

        #region Main

        public static string Main => "Main";
        public static string MainAbout => "Main_About";
        public static string MainVision => "Main_Vision";
        public static string MainVision1 => "Main_Vision";
        public static string MainBc => "Main_BC";
        public static string MainStaff => "Main_Staff";
        public static string MainCeo => "Main_CEO";
        public static string MainCert => "Main_Cert";
        public static string MainExecutive => "executivemanagement";

        public static string MainPolicies => "policiesandgovernance";

        public static string MainStrategy => "corporatestrategy";

        public static string About => "About";

        public static string AboutNavClass(ViewContext viewContext) => MainNavClass(viewContext, About);
        public static string MainNavClass(ViewContext viewContext) => PageNavClass(viewContext, Main);
        public static string MainAboutNavClass(ViewContext viewContext) => PageNavClass(viewContext, MainAbout);
        public static string MainVisionNavClass(ViewContext viewContext) => PageNavClass(viewContext, MainVision);
        public static string MainVision1NavClass(ViewContext viewContext) => PageNavClass(viewContext, MainVision1);
        public static string MainBcNavClass(ViewContext viewContext) => PageNavClass(viewContext, MainBc);
        public static string MainStaffNavClass(ViewContext viewContext) => PageNavClass(viewContext, MainStaff);
        public static string MainCeoNavClass(ViewContext viewContext) => PageNavClass(viewContext, MainCeo);
        public static string MainCeryNavClass(ViewContext viewContext) => PageNavClass(viewContext, MainCert);
        public static string MainExecutiveManagementNavClass(ViewContext viewContext) => PageNavClass(viewContext, MainExecutive);
        public static string MainGovernanceNavClass(ViewContext viewContext) => PageNavClass(viewContext, MainPolicies);
        public static string MainCorporateStrategyNavClass(ViewContext viewContext) => PageNavClass(viewContext, MainStrategy);

        #endregion

        #region MediaCenter

        public static string Mc => "MC";
        public static string McNews => "MC_News";
        public static string McExhibtion => "MC_Exhibtion";
        public static string McSocial => "MC_Social";
        public static string McReports => "MC_Reports";
        public static string McActivities => "MC_Activities";
       
        public static string MediaContact => "MC_ Media_Contact";
        public static string McMediaVideos => "MC_Media_Videos";
        public static string McMediaImages => "MC_Media_Images";

        public static string McNavClass(ViewContext viewContext) => MainNavClass(viewContext, Mc);
        public static string McNewsNavClass(ViewContext viewContext) => PageNavClass(viewContext, McNews);
        public static string McExhibtionNavClass(ViewContext viewContext) => PageNavClass(viewContext, McExhibtion);
        public static string McSocialNavClass(ViewContext viewContext) => PageNavClass(viewContext, McSocial);
        public static string McReportsNavClass(ViewContext viewContext) => PageNavClass(viewContext, McReports);
        public static string McActivitiesNavClass(ViewContext viewContext) => PageNavClass(viewContext, McActivities);
        public static string McMediaContactNavClass(ViewContext viewContext) => PageNavClass(viewContext, MediaContact);
        public static string McMediaImagesNavClass(ViewContext viewContext) => PageNavClass(viewContext, McMediaImages);
        public static string McMediaVideosNavClass(ViewContext viewContext) => PageNavClass(viewContext, McMediaVideos);

        #endregion

        #region Company

        public static string Compay => "Company";

        public static string CompayNavClass(ViewContext viewContext) => MainNavClass(viewContext, Compay);
        public static string CompanySNavClass(ViewContext viewContext, string title) => PageNavClass(viewContext, title);

        #endregion

        #region InvestorRelations

        public static string InvestorRelations => "InvestorRelations";

        public static string InvestorRelationsNavClass(ViewContext viewContext) => MainNavClass(viewContext, InvestorRelations);
       
        #endregion


        #region Jobs

        public static string Jobs => "Jobs";
        public static string JobsEnvironment => "Jobs_Environment";
        public static string JobsFeatures => "Jobs_Features";
        public static string JobsTraning => "Jobs_Traning";
        public static string JobsFillApplication => "Jobs_FillApplication";
        public static string JobsCareer => "Jobs_Career";
        public static string JobsContact => "Jobs_Contact";

        public static string JobsNavClass(ViewContext viewContext) => MainNavClass(viewContext, Jobs);
        public static string JobsEnvironmentNavClass(ViewContext viewContext) => PageNavClass(viewContext, JobsEnvironment);
        public static string JobsFeaturesNavClass(ViewContext viewContext) => PageNavClass(viewContext, JobsFeatures);
        public static string JobsTraningNavClass(ViewContext viewContext) => PageNavClass(viewContext, JobsTraning);
        public static string JobsFillApplicationNavClass(ViewContext viewContext) => PageNavClass(viewContext, JobsFillApplication);
        public static string JobsCareerNavClass(ViewContext viewContext) => PageNavClass(viewContext, JobsCareer);
        public static string JobsContactNavClass(ViewContext viewContext) => PageNavClass(viewContext, JobsContact);

        #endregion

        #region ContactUs

        public static string Contact => "Contact";
        public static string ContactContacts => "Contact_Contact";
        public static string ContactSupport => "Contact_Support";

        public static string ContactNavClass(ViewContext viewContext) => MainNavClass(viewContext, Contact);
        public static string ContactContactsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ContactContacts);
        public static string ContactSupportNavClass(ViewContext viewContext) => PageNavClass(viewContext, ContactSupport);

        #endregion

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData[ActivePageKey] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase)
                ? "active"
                : null;
        }

        public static string MainNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData[ActiveMenuKey] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase)
                ? "bold"
                : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage)
            => viewData[ActivePageKey] = activePage;

        public static void AddActiveMenu(this ViewDataDictionary viewData, string activePage)
            => viewData[ActiveMenuKey] = activePage;
    }
}