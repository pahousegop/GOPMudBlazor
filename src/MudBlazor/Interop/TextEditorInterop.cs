// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MudBlazor.Interop
{
    public class TextEditorInterop
    {


        internal static ValueTask<object> CreateQuill(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            string Id,
            bool readOnly,
            string placeholder,
            string theme,
            string[] formats,
            string debugLevel)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.createQuill",
                quillElement, Id, readOnly,
                placeholder, theme, formats, debugLevel);
        }

        internal static ValueTask<string> GetText(
            IJSRuntime jsRuntime,
            ElementReference quillElement)
        {
            return jsRuntime.InvokeAsync<string>(
                "QuillFunctions.getQuillText",
                quillElement);
        }

        //internal static ValueTask<string> GetHTML(
        //    IJSRuntime jsRuntime,
        //    ElementReference quillElement)
        //{
        //    return jsRuntime.InvokeAsync<string>(
        //        "QuillFunctions.getQuillHTML",
        //        quillElement);
        //}

        internal async Task<string> GetHTML(IJSRuntime jsRuntime, ElementReference quillElement)
        {
            bool isReceiving =  true;

            int chunkSize = 20000; // Adjust for efficiency
            int i = 0;
            StringBuilder builingHTML = new();
            int n = 0;
            while(isReceiving && n < 50)
            {
                string receivedChunk = await jsRuntime.InvokeAsync<string>("QuillFunctions.getQuillHTMLChunk", quillElement, i, chunkSize);

                builingHTML.Append(receivedChunk);
                if (receivedChunk.Length < chunkSize)
                {
                    isReceiving = false;
                   
                }
                i += chunkSize;
                n++;
            }

            return builingHTML.ToString();
        }

        internal static ValueTask<string> GetContent(
            IJSRuntime jsRuntime,
            ElementReference quillElement)
        {
            return jsRuntime.InvokeAsync<string>(
                "QuillFunctions.getQuillContent",
                quillElement);
        }

        internal static ValueTask<object> LoadQuillContent(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            string Content)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.loadQuillContent",
                quillElement, Content);
        }

        internal static ValueTask<object> LoadQuillHTMLContent(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            string quillHTMLContent)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.loadQuillHTMLContent",
                quillElement, quillHTMLContent);
        }

        internal static ValueTask<object> EnableQuillEditor(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            bool mode)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.enableQuillEditor",
                quillElement, mode);
        }

        internal static ValueTask<object> InsertQuillImage(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            string imageURL)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.insertQuillImage",
                quillElement, imageURL);
        }

        internal static ValueTask<object> InsertQuillText(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            string text)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.insertQuillText",
                quillElement, text);
        }
    }
}

