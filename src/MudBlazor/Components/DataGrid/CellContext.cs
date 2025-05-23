﻿// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;

namespace MudBlazor
{
#nullable enable
    /// <summary>
    /// Represents the current state of a cell in a <see cref="MudDataGrid{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of item displayed in the cell.</typeparam>
    public class CellContext<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] T>
    {
        private readonly HashSet<T> _selection;

        internal HashSet<T> OpenHierarchies { get; }

        /// <summary>
        /// The item displayed in the cell.
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// The behaviors which can be performed in the cell.
        /// </summary>
        public CellActions Actions { get; }

        /// <summary>
        /// Indicates if the cell is currently selected.
        /// </summary>
        public bool Selected => _selection.Contains(Item);

        /// <summary>
        /// Indicates if the cell is currently in an open hierarchy.
        /// </summary>
        public bool Open => OpenHierarchies.Contains(Item);

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dataGrid">The data grid which owns this context.</param>
        /// <param name="item">The item displayed in the cell.</param>
        public CellContext(MudDataGrid<T> dataGrid, T item)
        {
            _selection = dataGrid.Selection;
            OpenHierarchies = dataGrid._openHierarchies;
            Item = item;
            Actions = new CellActions
            {
                SetSelectedItemAsync = x => dataGrid.SetSelectedItemAsync(x, item),
                StartEditingItemAsync = () => dataGrid.SetEditingItemAsync(item),
                CancelEditingItemAsync = () => dataGrid.CancelEditingItemAsync(),
                ToggleHierarchyVisibilityForItemAsync = () => dataGrid.ToggleHierarchyVisibilityAsync(item),
                GetGroupIcon = (expanded, rightToLeft) => dataGrid.GetGroupIcon(expanded, rightToLeft),
            };
        }

        /// <summary>
        /// Represents behaviors which can be performed on a cell.
        /// </summary>
        public class CellActions
        {
            /// <summary>
            /// The function which selects the cell.
            /// </summary>
            public required Func<bool, Task> SetSelectedItemAsync { get; init; }

            /// <summary>
            /// The function which begins editing.
            /// </summary>
            public required Func<Task> StartEditingItemAsync { get; init; }

            /// <summary>
            /// The function which ends editing.
            /// </summary>
            public required Func<Task> CancelEditingItemAsync { get; init; }

            /// <summary>
            /// The function which toggles hierarchy visibility.
            /// </summary>
            public required Func<Task> ToggleHierarchyVisibilityForItemAsync { get; init; }

            /// <summary>
            /// The function which retrieves the GroupIcon.
            /// </summary>
            public Func<bool, bool, string>? GetGroupIcon { get; init; }
        }
    }
}
