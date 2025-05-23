﻿using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Docs.Pages.Api;
using MudBlazor.Docs.Services;
using MudBlazor.Services;
using MudBlazor.UnitTests.Mocks;
using NUnit.Framework;

namespace MudBlazor.UnitTests.Docs.Generated
{
    [TestFixture]
    public partial class ApiDocsTests
    {
        private Bunit.TestContext ctx;

        [SetUp]
        public void Setup()
        {
            ctx = new Bunit.TestContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            ctx.Services.AddSingleton(TimeProvider.System);
            ctx.Services.AddSingleton<IDialogService>(new DialogService());
            ctx.Services.AddSingleton<ISnackbar, SnackbarService>();
            ctx.Services.AddSingleton<IBrowserViewportService>(new MockBrowserViewportService());
            ctx.Services.AddTransient<IScrollManager, MockScrollManager>();
            ctx.Services.AddTransient<IScrollListenerFactory, MockScrollListenerFactory>();
            ctx.Services.AddTransient<IJsApiService, MockJsApiService>();
            ctx.Services.AddTransient<IDocsJsApiService, MockDocsJsApiService>();
            ctx.Services.AddTransient<IResizeObserverFactory, MockResizeObserverFactory>();
            ctx.Services.AddTransient<IScrollSpyFactory, MockScrollSpyFactory>();
            ctx.Services.AddTransient<IEventListenerFactory, MockEventListenerFactory>();
            ctx.Services.AddTransient<IEventListener, MockEventListener>();
            ctx.Services.AddSingleton<IDocsNavigationService, DocsNavigationService>();
            ctx.Services.AddSingleton<IMenuService, MenuService>();
            ctx.Services.AddSingleton<IPopoverService, MockPopoverService>();
            ctx.Services.AddSingleton<IKeyInterceptorService, MockKeyInterceptorService>();
            ctx.Services.AddTransient<IJsEventFactory, MockJsEventFactory>();
            ctx.Services.AddScoped<IRenderQueueService, RenderQueueService>();
            ctx.Services.AddScoped<IPointerEventsNoneService, MockPointerEventsNoneService>();
            ctx.Services.AddTransient<InternalMudLocalizer>();
            ctx.Services.AddTransient<ILocalizationInterceptor, DefaultLocalizationInterceptor>();
            ctx.Services.AddTransient<ILocalizationEnumInterceptor, DefaultLocalizationEnumInterceptor>();
            ctx.Services.AddScoped(sp => new HttpClient());
        }

        // This shows how to test a docs page with incremental rendering.
        // We are not (yet) testing all docs pages (just the examples), but if we wanted to, this would be the way.
        [Test]
        public async Task AlertPage_Test()
        {
            ctx.Services.AddSingleton<NavigationManager>(new MockNavigationManager("https://localhost:2112/", "https://localhost:2112/components/alert"));
            var comp = ctx.RenderComponent<MudBlazor.Docs.Pages.Components.Alert.AlertPage>();
            await ctx.Services.GetService<IRenderQueueService>().WaitUntilEmpty();
        }

        /// <summary>
        /// An example of a generated API test.
        /// </summary>
        [Test]
        public async Task MudAlert_API_Test_Example()
        {
            ctx.Services.AddSingleton<NavigationManager>(new MockNavigationManager("https://localhost:2112/", "https://localhost:2112/components/MudAlert"));
            var comp = ctx.RenderComponent<Api>(ComponentParameter.CreateParameter("TypeName", "MudAlert"));
            await ctx.Services.GetService<IRenderQueueService>().WaitUntilEmpty();
            comp.Markup.Should().NotContain("Sorry, the type").And.NotContain("could not be found");
            var exampleLink = comp.FindComponents<MudLink>().FirstOrDefault(link => link.Instance.Href.StartsWith("/component"));
            exampleLink.Should().NotBeNull();
        }

        [TearDown]
        public void TearDown() => ctx.Dispose();
    }
}
