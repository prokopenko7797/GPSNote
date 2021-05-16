﻿using Prism.Common;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GPSNote.Extensions
{
    public static class NavigationServiceExtensions
    {
        public static async Task<INavigationResult> SelectTabAsync(this INavigationService navigationService, string name, INavigationParameters parameters = null)
        {
            NavigationResult result = new NavigationResult();

            try
            {
                var currentPage = ((IPageAware)navigationService).Page;
                var canNavigate = await PageUtilities.CanNavigateAsync(currentPage, parameters);
                if (!canNavigate)
                {
                    throw new Exception($"IConfirmNavigation for { currentPage} returned false");
                }
                TabbedPage tabbedPage = null;
                if (currentPage is TabbedPage)
                {
                    tabbedPage = currentPage as TabbedPage;
                }
                if (currentPage.Parent is TabbedPage parent)
                {
                    tabbedPage = parent;
                }
                else if (currentPage.Parent is NavigationPage navPage)
                {
                    if (navPage.Parent != null && navPage.Parent is TabbedPage parent2)
                    {
                        tabbedPage = parent2;
                    }
                }
                if (tabbedPage == null)
                {
                    throw new Exception("No parent TabbedPage could be found");
                }
                var tabToSelectedType = PageNavigationRegistry.GetPageType(UriParsingHelper.GetSegmentName(name));
                if (tabToSelectedType is null)
                {
                    throw new Exception($"No View Type has been registered for ‘{ name}‘");
                }
                Page target = null;
                foreach (var child in tabbedPage.Children)
                {
                    if (child.GetType() == tabToSelectedType)
                    {
                        target = child;
                        break;
                    }
                    if (child is NavigationPage childNavPage)
                    {
                        if (childNavPage.CurrentPage.GetType() == tabToSelectedType ||
                            childNavPage.RootPage.GetType() == tabToSelectedType)
                        {
                            target = child;
                            break;
                        }
                    }
                }
                if (target is null)
                {
                    throw new Exception($"Could not find a Child Tab for ‘{ name}’");
                }
                var tabParameters = UriParsingHelper.GetSegmentParameters(name, parameters);
                tabbedPage.CurrentPage = target;
                PageUtilities.OnNavigatedFrom(currentPage, tabParameters);
                PageUtilities.OnNavigatedTo(target, tabParameters);
            }
            catch (Exception ex)
            {
                result = new NavigationResult { Exception = ex };
            }

            if (result.Exception == null) 
            {
                result.Success = true;
            }

            return result; 
        }
    }
}
