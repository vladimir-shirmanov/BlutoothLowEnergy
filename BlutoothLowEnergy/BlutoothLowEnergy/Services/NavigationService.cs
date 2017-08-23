using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace BlutoothLowEnergy.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Type> pagesByKey = new Dictionary<string, Type>();
        private NavigationPage navigationPage;

        public string CurrentPageKey
        {
            get
            {
                lock (this.pagesByKey)
                {
                    if (this.navigationPage.CurrentPage == null)
                    {
                        return null;
                    }

                    Type pageType = this.navigationPage.CurrentPage.GetType();

                    return this.pagesByKey.ContainsValue(pageType)
                        ? this.pagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        public void GoBack()
        {
            this.navigationPage.PopAsync();
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            lock (this.pagesByKey)
            {
                if (this.pagesByKey.ContainsKey(pageKey))
                {
                    Type type = this.pagesByKey[pageKey];
                    ConstructorInfo constructor;
                    object[] parameters;

                    if (parameter == null)
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c => !c.GetParameters().Any());

                        parameters = new object[]
                        {
                        };
                    }
                    else
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(
                                c =>
                                {
                                    var p = c.GetParameters();
                                    return p.Count() == 1
                                           && p[0].ParameterType == parameter.GetType();
                                });

                        parameters = new[]
                        {
                            parameter
                        };
                    }

                    if (constructor == null)
                    {
                        throw new InvalidOperationException(
                            "No suitable constructor found for page " + pageKey);
                    }

                    var page = constructor.Invoke(parameters) as Page;
                    this.navigationPage.PushAsync(page);
                }
                else
                {
                    throw new ArgumentException(
                        $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?",
                        nameof(pageKey));
                }
            }
        }

        public void Configure(string pageKey, Type pageType)
        {
            lock (this.pagesByKey)
            {
                if (this.pagesByKey.ContainsKey(pageKey))
                {
                    this.pagesByKey[pageKey] = pageType;
                }
                else
                {
                    this.pagesByKey.Add(pageKey, pageType);
                }
            }
        }

        public void Initialize(NavigationPage navigation)
        {
            this.navigationPage = navigation;
        }
    }
}
