using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;

namespace MudBlazor
{
    /// <summary>
    /// Represents an option of a select or multi-select. To be used inside MudSelectGOP.
    /// </summary>
    public partial class MudSelectItemGOP<T> : MudBaseSelectItem, IDisposable
    {
        private string GetCssClasses() => new CssBuilder()
            .AddClass(Class)
            .Build();

        private IMudSelectGOP _parent;
        internal string ItemId { get; } = Identifier.Create();

        /// <summary>
        /// The parent select component
        /// </summary>
        [CascadingParameter]
        internal IMudSelectGOP IMudSelectGOP
        {
            get => (IMudSelectGOP)_parent;
            set
            {
                _parent = value;
                if (_parent == null)
                    return;
                _parent.CheckGenericTypeMatch(this);
                if (MudSelectGOP == null)
                    return;
                var selected = MudSelectGOP.Add(this);
                if (_parent.MultiSelection)
                {
                    MudSelectGOP.SelectionChangedFromOutside += OnUpdateSelectionStateFromOutside;
                    InvokeAsync(() => OnUpdateSelectionStateFromOutside(MudSelectGOP.SelectedValues));
                }
                else
                {
                    Selected = selected;
                }
            }
        }

        private IMudShadowSelect _shadowParent;
        private bool _selected;

        [CascadingParameter]
        internal IMudShadowSelect IMudShadowSelect
        {
            get => _shadowParent;
            set
            {
                _shadowParent = value;
                ((MudSelectGOP<T>)_shadowParent)?.RegisterShadowItem(this);
            }
        }

        /// <summary>
        /// Select items with HideContent==true are only there to register their RenderFragment with the select but
        /// wont render and have no other purpose!
        /// </summary>
        [CascadingParameter(Name = "HideContent")]
        internal bool HideContent { get; set; }

        internal MudSelectGOP<T> MudSelectGOP => (MudSelectGOP<T>)IMudSelectGOP;

        private void OnUpdateSelectionStateFromOutside(IEnumerable<T> selection)
        {
            if (selection == null)
                return;
            var old_selected = Selected;
            Selected = selection.Contains(Value);
            if (old_selected != Selected)
                InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// A user-defined option that can be selected
        /// </summary>
        [Parameter]
        [Category(CategoryTypes.FormComponent.Behavior)]
        public T Value { get; set; }

        /// <summary>
        /// Mirrors the MultiSelection status of the parent select
        /// </summary>
        protected bool MultiSelection
        {
            get
            {
                if (MudSelectGOP == null)
                    return false;
                return MudSelectGOP.MultiSelection;
            }
        }

        /// <summary>
        /// Selected state of the option. Only works if the parent is a mulit-select
        /// </summary>
        internal bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
            }
        }

        /// <summary>
        /// The checkbox icon reflects the multi-select option's state
        /// </summary>
        protected string CheckBoxIcon
        {
            get
            {
                if (!MultiSelection)
                    return null;
                return Selected ? Icons.Material.Filled.CheckBox : Icons.Material.Filled.CheckBoxOutlineBlank;
            }
        }

        protected string DisplayString
        {
            get
            {
                var converter = MudSelectGOP?.Converter;
                if (converter == null)
                    return $"{Value}";
                return converter.Set(Value);
            }
        }

        private void OnClicked()
        {
            if (MultiSelection)
                Selected = !Selected;

            MudSelectGOP?.SelectOption(Value);
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            try
            {
                MudSelectGOP?.Remove(this);
                ((MudSelectGOP<T>)_shadowParent)?.UnregisterShadowItem(this);
            }
            catch (Exception) { }
        }
    }
}
