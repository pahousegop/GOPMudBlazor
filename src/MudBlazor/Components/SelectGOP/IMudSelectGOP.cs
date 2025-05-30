﻿// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace MudBlazor
{
    internal interface IMudSelectGOP
    {
        void CheckGenericTypeMatch(object select_item);
        bool MultiSelection { get; set; }
    }

    internal interface IMudShadowSelectGOP
    {
    }
}
