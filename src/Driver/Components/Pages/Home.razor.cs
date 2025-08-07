// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Driver;
using Driver.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuillConverter;
using static Microsoft.AspNetCore.Components.Web.RenderMode;

namespace Driver.Components.Pages
{
    public partial class Home
    {

        List<TestItem> items = new();

        protected override void OnInitialized()
        {
            for (int i = 1; i < 4; i++)
            {
                items.Add(new TestItem { Id = i, content = @"{""ops"":[{""insert"":""test content " + i + @"""}]}" });
            }
            //initialValue = Content;

            // JObject jsonObject = JObject.Parse(Content);

            // var htmlConverter = new HtmlConverter((JArray)jsonObject["ops"]);
            // quillHtml = htmlConverter.Convert();

            // var s = "";
            //var htmlConverter = new HtmlConverter(Content);
            //string html = htmlConverter.Convert();
            //MudGlobal.InputDefaults.Variant = Variant.Outlined;
        }

        private async Task AddNewItem()
        {
            items.Insert(0, new TestItem { Id = 0, content = null });

        }
        // private async Task SaveItem()
        // {

        //     string z = Content;

        //     var d1 = await quill.GetContent();

        //     var html = await quill.GetHTML();
        //     JObject jsonObject = JObject.Parse(d1);
        //     var htmlConverter = new HtmlConverter((JArray)jsonObject["ops"]);
        //     quillHtml = htmlConverter.Convert();
        //     var n = quillHtml.Length;

        //     var g = "";
        // }

        public class TestItem
        {
            public int Id { get; set; }
            public string content { get; set; }
        }
    }
    //public partial class Home
    //{



    //    //string quillHtml = "";

    //    ////string? MyDate;
    //    ////string  usDate = "";

    //    ////IMask mask2 = new DateMask("MM/dd/yyyy");

    //    //[Parameter] public string Content { get; set; } = @"{""ops"":[{""attributes"":{""bold"":true},""insert"":""Header ""},{""insert"":""-""},{""attributes"":{""italic"":true},""insert"":"" (member)""},{""attributes"":{""align"":""center""},""insert"":""\n""},{""insert"":""Header 2""},{""attributes"":{""indent"":2},""insert"":""\n""},{""insert"":""test content 1""},{""attributes"":{""list"":""bullet""},""insert"":""\n""},{""insert"":""test content 2""},{""attributes"":{""indent"":1,""list"":""bullet""},""insert"":""\n""},{""insert"":""test content 3""},{""attributes"":{""indent"":1,""list"":""bullet""},""insert"":""\n""},{""insert"":""test content 4""},{""attributes"":{""indent"":2,""list"":""bullet""},""insert"":""\n""},{""insert"":""test content 5""},{""attributes"":{""indent"":2,""list"":""bullet""},""insert"":""\n""},{""insert"":""test content 6""},{""attributes"":{""indent"":1,""list"":""bullet""},""insert"":""\n""},{""insert"":""test content 7""},{""attributes"":{""list"":""bullet""},""insert"":""\n""}]}";
    //    ////[Parameter] public string Content { get; set; } = @"{""ops"":[{""insert"":""labor &amp; industry\n""}]}";


    //    //[Parameter] public string Content2 { get; set; } = "test content 2";

    //    //public string initialValue { get; set; } = "";
    //    //public string textFieldValue { get; set; } = "This is the field";
    //    //public MudTextEditor quill = new();

    //    //protected override void OnInitialized()
    //    //{
    //    //    initialValue = Content;

    //    //    // JObject jsonObject = JObject.Parse(Content);

    //    //    // var htmlConverter = new HtmlConverter((JArray)jsonObject["ops"]);
    //    //    // quillHtml = htmlConverter.Convert();

    //    //    // var s = "";
    //    //    //var htmlConverter = new HtmlConverter(Content);
    //    //    //string html = htmlConverter.Convert();
    //    //    //MudGlobal.InputDefaults.Variant = Variant.Outlined;
    //    //}


    //    //private async Task SaveItem()
    //    //{

    //    //    string z = Content;

    //    //    var d1 = await quill.GetContent();

    //    //    var html = await quill.GetHTML();
    //    //    JObject jsonObject = JObject.Parse(d1);
    //    //    var htmlConverter = new HtmlConverter((JArray)jsonObject["ops"]);
    //    //    quillHtml = htmlConverter.Convert();
    //    //    var n = quillHtml.Length;

    //    //    var g = "";
    //    //}
    //}
}
