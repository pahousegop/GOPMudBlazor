﻿using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;

namespace MudBlazor;

#nullable enable

/// <summary>
/// A header cell which labels a column of data for a <see cref="MudTable{T}"/>.
/// </summary>
public partial class MudTh : MudComponentBase
{
    protected string Classname => new CssBuilder("mud-table-cell")
        .AddClass(Context?.Table?.CellClass)
        .AddClass(Class)
        .Build();

    /// <summary>
    /// The current state of the <see cref="MudTable{T}"/> containing this group.
    /// </summary>
    [CascadingParameter]
    public TableContext? Context { get; set; }

    /// <summary>
    /// The content within this header cell.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
