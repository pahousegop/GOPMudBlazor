using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor.Interop;
using MudBlazor.State;
using MudBlazor.Utilities;


namespace MudBlazor
{
    public partial class MudTextEditor : MudBaseInput<string>
    {
        #region "Parameters"

        private Timer debounceTimer;
        
        [Inject] protected IJSRuntime jsRuntime { get; set; } = default!;

        [Parameter]
        public RenderFragment EditButtons { get; set; }

        [Parameter]
        public string UniqueID { get; set; }

        [Parameter]
        public bool OuptDelta { get; set; } = true;

        [Parameter]
        public RenderFragment ExtraToolbarContent { get; set; }

        public RenderFragment DefaultToolbarContent { get; set; }
        private RenderFragment ToolbarContent => builder =>
        {
            if (!HideDefaultToolbar)
                DefaultToolbarContent(builder); // Render the default toolbar
            
            if(ExtraToolbarContent != null)
                ExtraToolbarContent(builder); // Append extra toolbar items
        };
        [Parameter]
        public bool ToolbarLimited { get; set; } = false;

        [Parameter]
        public bool HideDefaultToolbar { get; set; } = false;

        protected string Classname =>
           new CssBuilder("mud-input-control mud-input-input-control")
               .AddClass($"mud-input-{Variant.ToDescriptionString()}-with-label", !string.IsNullOrEmpty(Label))
               .AddClass(Class)
               .Build();

        protected string InputClassname =>
           new CssBuilder(
               MudInputCssHelper.GetClassname(this,
                   () => !string.IsNullOrEmpty(Text) || Adornment == Adornment.Start || !string.IsNullOrWhiteSpace(Placeholder) || ShrinkLabel))
            .Build();

        [Parameter]
        public string Theme { get; set; }
            = "snow";

        [Parameter]
        public string[] Formats { get; set; }
            = null;

        [Parameter]
        public string DebugLevel { get; set; }
            = "info";

        /// <summary>
        /// Support for normal css classes
        /// </summary>
        [Parameter]
        public string EditorCssClass { get; set; }
            = string.Empty;

        /// <summary>
        /// Support for normal css styles
        /// </summary>
        [Parameter]
        public string EditorCssStyle { get; set; }
            = string.Empty;

        /// <summary>
        /// Support for normal css classes
        /// </summary>
        [Parameter]
        public string ToolbarCSSClass { get; set; }
            = string.Empty;

        /// <summary>
        /// Support for normal css styles
        /// </summary>
        [Parameter]
        public string ToolbarCssStyle { get; set; }
            = string.Empty;

        [Parameter]
        public bool BottomToolbar { get; set; } = false;

        private ElementReference QuillElement;
        private ElementReference ToolBar;
        private bool IsValueHTML;
        #endregion

        protected override Task OnInitializedAsync()
        {
            IsValueHTML = !Value.ToLower().Contains("\"ops\"");

            SetToolbarContent();
            return Task.CompletedTask;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
                await TextEditorInterop.CreateQuill(
                    jsRuntime,
                    QuillElement,
                    UniqueID,
                    ReadOnly,
                    Placeholder,
                    Theme,
                    Formats,
                    DebugLevel,
                    DotNetObjectReference.Create(this));

                if (IsValueHTML)
                {
                    await TextEditorInterop.LoadQuillHTMLContent(
                    jsRuntime,
                    QuillElement,
                    Value.Replace("&amp;", "&"));
                }
                else
                {
                    await TextEditorInterop.LoadQuillContent(
                    jsRuntime,
                    QuillElement,
                    Value);
                }
                
            }
        }

        [JSInvokable]
        public async Task HandleContentChange()
        {
            debounceTimer?.Dispose(); // Dispose any previous timer
            if (OuptDelta)
            {
                debounceTimer = new Timer(_ => SetValueFromContent(), null, 750, Timeout.Infinite);
            }
            else
            {
                debounceTimer = new Timer(_ => SetValueFromHTML(), null, 750, Timeout.Infinite);
            }

        }

        private async Task SetValueFromHTML()
        {
            TextEditorInterop TextEditorInterop = new TextEditorInterop();
            Value = await TextEditorInterop.GetHTML(
                jsRuntime, QuillElement);
            await ValueChanged.InvokeAsync(Value);
        }

        private async Task SetValueFromContent()
        {
            TextEditorInterop TextEditorInterop = new TextEditorInterop();
            Value = await TextEditorInterop.GetContentChunkStyle(
                jsRuntime, QuillElement);
            Value = Value.Replace("&", "&amp;");
            await ValueChanged.InvokeAsync(Value);
        }

        private void SetToolbarContent()
        {
            if (!HideDefaultToolbar)
            {
                if (ToolbarLimited)
                {
                    SetLimited();
                }
                else
                {
                    SetDefault();
                }
            }
            
        }

        //public async Task<string> GetText()
        //{
        //    //throw new NotImplementedException();
        //    return await TextEditorInterop.GetText(
        //        jsRuntime, QuillElement);
        //}

        public async Task<string> GetHTML()
        {
            TextEditorInterop TextEditorInterop = new TextEditorInterop();
            return await TextEditorInterop.GetHTML(
                jsRuntime, QuillElement);
        }

        public async Task<string> GetContent()
        {
            //throw new NotImplementedException();
            return await TextEditorInterop.GetContent(
                jsRuntime, QuillElement);
        }

        //public async Task LoadContent(string Content)
        //{
        //    var QuillDelta =
        //        await TextEditorInterop.LoadQuillContent(
        //            jsRuntime, QuillElement, Content);
        //}

        //public async Task LoadHTMLContent(string quillHTMLContent)
        //{
        //    var QuillDelta =
        //        await TextEditorInterop.LoadQuillHTMLContent(
        //            jsRuntime, QuillElement, quillHTMLContent);
        //}

        //public async Task InsertImage(string ImageURL)
        //{
        //    var QuillDelta =
        //        await TextEditorInterop.InsertQuillImage(
        //            jsRuntime, QuillElement, ImageURL);
        //}

        //public async Task InsertText(string text)
        //{
        //    var QuillDelta =
        //        await TextEditorInterop.InsertQuillText(
        //            jsRuntime, QuillElement, text);
        //}

        //public async Task EnableEditor(bool mode)
        //{
        //    var QuillDelta =
        //        await TextEditorInterop.EnableQuillEditor(
        //            jsRuntime, QuillElement, mode);
        //}

        private void SetLimited()
        {
            DefaultToolbarContent = builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", "ql-formats");

                builder.OpenElement(2, "button");
                builder.AddAttribute(3, "class", "ql-bold");
                builder.CloseElement();

                builder.OpenElement(4, "button");
                builder.AddAttribute(5, "class", "ql-italic");
                builder.CloseElement();

                builder.OpenElement(6, "button");
                builder.AddAttribute(7, "class", "ql-underline");
                builder.CloseElement();

                builder.OpenElement(8, "button");
                builder.AddAttribute(9, "class", "ql-strike");
                builder.CloseElement();

                builder.CloseElement();
            };
        }

        private void SetDefault()
        {
            DefaultToolbarContent = builder =>
            {
                builder.OpenElement(0, "span");
                    builder.AddAttribute(1, "class", "ql-formats");

                    builder.OpenElement(2, "button");
                    builder.AddAttribute(3, "class", "ql-bold");
                    builder.CloseElement();

                    builder.OpenElement(4, "button");
                    builder.AddAttribute(5, "class", "ql-italic");
                    builder.CloseElement();

                    builder.OpenElement(6, "button");
                    builder.AddAttribute(7, "class", "ql-underline");
                    builder.CloseElement();

                    builder.OpenElement(8, "button");
                    builder.AddAttribute(9, "class", "ql-strike");
                    builder.CloseElement();

                builder.CloseElement();

                builder.OpenElement(10, "span");
                    builder.AddAttribute(11, "class", "ql-formats");

                    builder.OpenElement(12, "select");
                    builder.AddAttribute(13, "class", "ql-color");
                    builder.CloseElement();

                    builder.OpenElement(14, "select");
                    builder.AddAttribute(15, "class", "ql-background");
                    builder.CloseElement();

                builder.CloseElement();

                builder.OpenElement(16, "span");
                    builder.AddAttribute(17, "class", "ql-formats");

                    builder.OpenElement(18, "button");
                    builder.AddAttribute(19, "class", "ql-list");
                    builder.AddAttribute(20, "value", "ordered");
                    builder.CloseElement();

                    builder.OpenElement(21, "button");
                    builder.AddAttribute(22, "class", "ql-list");
                    builder.AddAttribute(23, "value", "bullet");
                    builder.CloseElement();

                builder.CloseElement();

                builder.OpenElement(24, "span");
                    builder.AddAttribute(25, "class", "ql-formats");

                    builder.OpenElement(26, "button");
                    builder.AddAttribute(27, "class", "ql-indent");
                    builder.AddAttribute(28, "value", "-1");
                    builder.CloseElement();

                    builder.OpenElement(29, "button");
                    builder.AddAttribute(30, "class", "ql-indent");
                    builder.AddAttribute(31, "value", "+1");
                    builder.CloseElement();

                builder.CloseElement();

                builder.OpenElement(32, "span");
                    builder.AddAttribute(33, "class", "ql-formats");

                    builder.OpenElement(34, "button");
                    builder.AddAttribute(35, "class", "ql-align");
                    builder.AddAttribute(36, "value", "");
                    builder.CloseElement();

                    builder.OpenElement(37, "button");
                    builder.AddAttribute(38, "class", "ql-align");
                    builder.AddAttribute(39, "value", "center");
                    builder.CloseElement();

                    builder.OpenElement(40, "button");
                    builder.AddAttribute(41, "class", "ql-align");
                    builder.AddAttribute(42, "value", "right");
                    builder.CloseElement();

                    builder.OpenElement(43, "button");
                    builder.AddAttribute(44, "class", "ql-align");
                    builder.AddAttribute(45, "value", "justify");
                    builder.CloseElement();
                builder.CloseElement();
            };
        }
        
    }
}
