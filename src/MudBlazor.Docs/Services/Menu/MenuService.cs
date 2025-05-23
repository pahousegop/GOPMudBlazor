﻿// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MudBlazor.Charts;
using MudBlazor.Docs.Models;


namespace MudBlazor.Docs.Services
{
#nullable enable
    /// <summary>
    /// The aim of this class is to add new items to NavMenu
    /// </summary>
    public class MenuService : IMenuService
    {
        private IEnumerable<DocsLink>? _features;
        private IEnumerable<DocsLink>? _utilities;
        private DocsComponents? _docsComponentsApi; //cached property
        private IEnumerable<DocsLink>? _customization;
        private readonly Dictionary<Type, MudComponent> _parents = [];
        private readonly Dictionary<Type, MudComponent> _componentLookup = [];

        /// <summary>
        /// Here is where the links for the Components Menu in NavMenu are added
        /// Add here the new menu elements without caring about the order.
        /// They will be reordered automatically
        /// </summary>
        private readonly List<MudComponent> _docsComponents = new DocsComponents()
            //Individual elements
            .AddItem("Container", typeof(MudContainer))
            .AddItem("Grid", typeof(MudGrid), typeof(MudItem))
            .AddItem("Hidden", typeof(MudHidden))
            .AddItem("Breakpoint Provider", typeof(MudBreakpointProvider))
            .AddItem("Chips", typeof(MudChip<T>))
            .AddItem("Chip Set", typeof(MudChipSet<T>))
            .AddItem("Badge", typeof(MudBadge))
            .AddItem("App Bar", typeof(MudAppBar))
            .AddItem("Drawer", typeof(MudDrawer), typeof(MudDrawerHeader), typeof(MudDrawerContainer))
            .AddItem("Drop Zone", typeof(MudDropZone<T>), typeof(MudDropContainer<T>), typeof(MudDynamicDropItem<T>))
            .AddItem("Link", typeof(MudLink))
            .AddItem("Menu", typeof(MudMenu), typeof(MudMenuItem))
            .AddItem("Message Box", typeof(MudMessageBox))
            .AddItem("Nav Menu", typeof(MudNavMenu), typeof(MudNavLink), typeof(MudNavGroup))
            .AddItem("Tabs", typeof(MudTabs), typeof(MudTabPanel), typeof(MudDynamicTabs))
            .AddItem("Progress", typeof(MudProgressCircular), typeof(MudProgressLinear))
            .AddItem("Dialog", typeof(MudDialog), typeof(MudDialogContainer), typeof(MudDialogProvider))
            .AddItem("Snackbar", typeof(SnackbarService), typeof(MudSnackbarProvider), typeof(MudSnackbarElement))
            .AddItem("Avatar", typeof(MudAvatar), typeof(MudAvatarGroup))
            .AddItem("Alert", typeof(MudAlert))
            .AddItem("Card", typeof(MudCard), typeof(MudCardActions), typeof(MudCardContent), typeof(MudCardHeader), typeof(MudCardMedia))
            .AddItem("Chat", typeof(MudChat), typeof(MudChatHeader), typeof(MudChatBubble), typeof(MudChatFooter))
            .AddItem("Divider", typeof(MudDivider))
            .AddItem("Expansion Panels", typeof(MudExpansionPanels), typeof(MudExpansionPanel))
            .AddItem("Image", typeof(MudImage))
            .AddItem("Icons", typeof(MudIcon))
            .AddItem("List", typeof(MudList<T>), typeof(MudListItem<T>), typeof(MudListSubheader))
            .AddItem("Paper", typeof(MudPaper))
            .AddItem("Rating", typeof(MudRating), typeof(MudRatingItem))
            .AddItem("Skeleton", typeof(MudSkeleton))
            .AddItem("Table", typeof(MudTable<T>), typeof(MudTableBase), typeof(MudTablePager), typeof(MudTableGroupRow<T>), typeof(MudTableSortLabel<T>), typeof(MudTd), typeof(MudTh), typeof(MudTr), typeof(MudTFootRow), typeof(MudTHeadRow))
            .AddItem("Data Grid", typeof(MudDataGrid<T>), typeof(Column<T>), typeof(FilterHeaderCell<T>), typeof(FooterCell<T>), typeof(HeaderCell<T>), typeof(HierarchyColumn<T>), typeof(MudDataGridPager<T>), typeof(TemplateColumn<T>))
            .AddItem("Simple Table", typeof(MudSimpleTable))
            .AddItem("Tooltip", typeof(MudTooltip))
            .AddItem("Typography", typeof(MudText))
            .AddItem("Overlay", typeof(MudOverlay))
            .AddItem("Highlighter", typeof(MudHighlighter))
            .AddItem("Element", typeof(MudElement))
            .AddItem("Focus Trap", typeof(MudFocusTrap))
            .AddItem("Tree View", typeof(MudTreeView<T>), typeof(MudTreeViewItem<T>), typeof(MudTreeViewItemToggleButton))
            .AddItem("Breadcrumbs", typeof(MudBreadcrumbs))
            .AddItem("Scroll To Top", typeof(MudScrollToTop))
            .AddItem("Popover", typeof(MudPopover))
            .AddItem("Swipe Area", typeof(MudSwipeArea))
            .AddItem("Tool Bar", typeof(MudToolBar))
            .AddItem("Carousel", typeof(MudCarousel<T>), typeof(MudCarouselItem))
            .AddItem("Timeline", typeof(MudTimeline), typeof(MudTimelineItem))
            .AddItem("Pagination", typeof(MudPagination))
            .AddItem("Stack", typeof(MudStack))
            .AddItem("Spacer", typeof(MudSpacer))
            .AddItem("Collapse", typeof(MudCollapse))
            .AddItem("Stepper", typeof(MudStepper), typeof(MudStep))

            //GROUPS

            //Inputs
            .AddNavGroup("Form & Inputs", false, new DocsComponents()
                .AddItem("Radio", typeof(MudRadio<T>), typeof(MudRadioGroup<T>))
                .AddItem("Check Box", typeof(MudCheckBox<T>))
                .AddItem("Select", typeof(MudSelect<T>), typeof(MudSelectItem<T>))
                .AddItem("Slider", typeof(MudSlider<T>))
                .AddItem("Switch", typeof(MudSwitch<T>))
                .AddItem("Text Field", typeof(MudTextField<T>))
                .AddItem("Numeric Field", typeof(MudNumericField<T>))
                .AddItem("Form", typeof(MudForm))
                .AddItem("Autocomplete", typeof(MudAutocomplete<T>))
                .AddItem("Field", typeof(MudField))
                .AddItem("File Upload", typeof(MudFileUpload<T>))
                .AddItem("Toggle Group", typeof(MudToggleGroup<T>), typeof(MudToggleItem<T>))
            )

            //Pickers
            .AddNavGroup("Pickers", false, new DocsComponents()
                .AddItem("Date Picker", typeof(MudDatePicker))
                .AddItem("Date Range Picker", typeof(MudDateRangePicker))
                .AddItem("Time Picker", typeof(MudTimePicker))
                .AddItem("Color Picker", typeof(MudColorPicker))
            )

            //Buttons
            .AddNavGroup("Buttons", false, new DocsComponents()
                .AddItem("Button", typeof(MudButton))
                .AddItem("Button Group", typeof(MudButtonGroup))
                .AddItem("Icon Button", typeof(MudIconButton))
                .AddItem("Toggle Icon Button", typeof(MudToggleIconButton))
                .AddItem("Button FAB", typeof(MudFab))
            )

            //Charts
            .AddNavGroup("Charts", false, new DocsComponents()
                .AddItem("Donut Chart", typeof(Donut))
                .AddItem("Line Chart", typeof(Line), typeof(Legend))
                .AddItem("Pie Chart", typeof(Pie))
                .AddItem("Bar Chart", typeof(Bar), typeof(ChartOptions))
                .AddItem("Heat Map Chart", typeof(HeatMap))
                .AddItem("Stacked Bar Chart", typeof(StackedBar))
                .AddItem("Time Series Chart", typeof(TimeSeries), typeof(MudTimeSeriesChartBase), typeof(MudTimeSeriesChart))
            )
            // this must be last!
            .GetComponentsSortedByName();

        /// <summary>
        /// Features menu links
        /// </summary>
        public IEnumerable<DocsLink> Features => _features ??= new List<DocsLink>
            {
                new DocsLink {Title = "Breakpoints", Href = "features/breakpoints"},
                new DocsLink {Title = "Colors", Href = "features/colors"},
                new DocsLink {Title = "Elevation", Href = "features/elevation"},
                new DocsLink {Title = "Converters", Href = "features/converters"},
                new DocsLink {Title = "Icon Reference", Href = "features/icons"}, // <-- note: title changed from "Icons" to "Icon Reference" to avoid confusion in Search box with the MudIcon page which is also called "Icons"
                new DocsLink {Title = "Parameter State", Href = "features/parameterstate"},
                new DocsLink {Title = "Masking", Href = "features/masking"},
                new DocsLink {Title = "RTL Languages", Href = "features/rtl-languages"},
                new DocsLink {Title = "Localization", Href = "features/localization"},
                new DocsLink {Title = "Analyzers", Href = "features/analyzers"}
            }.OrderBy(x => x.Title);

        /// <summary>
        /// Customization menu links
        /// </summary>
        public IEnumerable<DocsLink> Customization => _customization ??= new List<DocsLink>
        {
            new DocsLink {Title = "Default theme", Href="customization/default-theme"},
            new DocsLink {Title = "Overview", Href = "customization/overview"},
            new DocsLink {Title = "Palette", Href = "customization/palette"},
            new DocsLink {Title = "Typography", Href = "customization/typography"},
            new DocsLink {Title = "z-index", Href = "customization/z-index"},
            new DocsLink {Title = "Pseudo CSS", Href = "customization/pseudocss"},
            new DocsLink {Title = "Globals", Href="customization/globals", Order = 100},
        }.OrderBy(link => link.Order).ThenBy(x => x.Title);

        /// <summary>
        /// CSS Utilities menu links
        /// </summary>
        public IEnumerable<DocsLink> Utilities => _utilities ??= new List<DocsLink>
        {
            new DocsLink {Group = "Layout", Title = "Display", Href = "utilities/display"},
            new DocsLink {Group = "Layout", Title = "Z-Index", Href = "utilities/z-index"},
            new DocsLink {Group = "Layout", Title = "Overflow", Href = "utilities/overflow"},
            new DocsLink {Group = "Layout", Title = "Visibility", Href = "utilities/visibility"},
            new DocsLink {Group = "Layout", Title = "Object Fit", Href = "utilities/object-fit"},
            new DocsLink {Group = "Layout", Title = "Object Position", Href = "utilities/object-position"},
            new DocsLink {Group = "Layout", Title = "Position", Href = "utilities/position"},

            new DocsLink {Group = "Flexbox", Title = "Enable Flexbox", Href = "utilities/enable-flex"},
            new DocsLink {Group = "Flexbox", Title = "Flex Direction", Href = "utilities/flex-direction"},
            new DocsLink {Group = "Flexbox", Title = "Flex Wrap", Href = "utilities/flex-wrap"},
            new DocsLink {Group = "Flexbox", Title = "Flex", Href = "utilities/flex"},
            new DocsLink {Group = "Flexbox", Title = "Flex Grow", Href = "utilities/flex-grow"},
            new DocsLink {Group = "Flexbox", Title = "Flex Shrink", Href = "utilities/flex-shrink"},
            new DocsLink {Group = "Flexbox", Title = "Order", Href = "utilities/order"},
            new DocsLink {Group = "Flexbox", Title = "Gap", Href = "utilities/gap"},
            new DocsLink {Group = "Flexbox", Title = "Justify Content", Href = "utilities/justify-content"},
            new DocsLink {Group = "Flexbox", Title = "Align Content", Href = "utilities/align-content"},
            new DocsLink {Group = "Flexbox", Title = "Align Items", Href = "utilities/align-items"},
            new DocsLink {Group = "Flexbox", Title = "Align Self", Href = "utilities/align-self"},

            new DocsLink {Group = "Spacing", Title = "Spacing", Href = "utilities/spacing"},

            new DocsLink {Group = "Borders", Title = "Border Radius", Href = "utilities/border-radius"},
            new DocsLink {Group = "Borders", Title = "Border Style", Href = "utilities/border-style"},
            new DocsLink {Group = "Borders", Title = "Border Width", Href = "utilities/border-width"},

            new DocsLink {Group = "Interactivity", Title = "Cursor", Href = "utilities/cursor"},
            new DocsLink {Group = "Interactivity", Title = "Pointer Events", Href = "utilities/pointer-events"},
        };

        public IEnumerable<MudComponent> Components => _docsComponents;

        public IEnumerable<MudComponent> Api => DocsComponentsApi.Components;

        public MenuService()
        {
            foreach (var component in Components)
            {
                if (component.IsNavGroup)
                {
                    foreach (var groupComponent in component.GroupComponents)
                    {
                        _componentLookup.Add(groupComponent.Type, groupComponent);
                        _parents.Add(groupComponent.Type, component);
                    }
                }
                else
                {
                    _componentLookup.Add(component.Type, component);
                    // top-level types refer to themself as parent ;)
                    _parents.Add(component.Type, component);
                    if (component.ChildTypes is not null)
                    {
                        foreach (var childType in component.ChildTypes)
                        {
                            _parents.Add(childType, component);
                        }
                    }
                }
            }
        }

        public MudComponent? GetParent(Type? child)
        {
            return child is not null
                ? _parents.GetValueOrDefault(child)
                : null;
        }

        public MudComponent? GetComponent(Type? type)
        {
            if (type is null)
            {
                return null;
            }

            return _componentLookup.TryGetValue(type, out var component)
                ? component
                : _parents.GetValueOrDefault(type);
        }

        /// <inheritdoc />
        public string? GetComponentName(string typeName)
        {
            var cleanName = typeName.Replace("`1", "<T>").Replace("`2", "<T, U>");
            foreach (var component in _docsComponents)
            {
                if (component.ComponentName != null && component.ComponentName.Equals(cleanName, StringComparison.OrdinalIgnoreCase))
                {
                    return component.Name;
                }
                if (component.GroupComponents != null)
                {
                    foreach (var groupComponent in component.GroupComponents)
                    {
                        if (groupComponent.ComponentName.Equals(cleanName, StringComparison.OrdinalIgnoreCase))
                        {
                            return groupComponent.Name;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// This autogenerates the Menu for the API
        /// </summary>
        private DocsComponents DocsComponentsApi
        {
            get
            {
                //caching property
                if (_docsComponentsApi is not null)
                {
                    return _docsComponentsApi;
                }

                _docsComponentsApi = new DocsComponents();
                foreach (var item in Components)
                {
                    if (item.IsNavGroup)
                    {
                        foreach (var groupComponent in item.GroupComponents)
                        {
                            _docsComponentsApi.AddItem(groupComponent.Name, groupComponent.Type);
                        }
                    }
                    else
                    {
                        _docsComponentsApi.AddItem(item.Name, item.Type);
                    }
                }

                return _docsComponentsApi;
            }
        }

        /// <summary>
        /// Gets the sub-menu, if any, matching the specified type.
        /// </summary>
        /// <param name="parent">The parent to start searching from.</param>
        /// <param name="type">The type to find.</param>
        /// <returns>The menu whose link, child type, or group component matches the type.</returns>
        public MudComponent? GetExample(DocumentedType type)
        {
            // Go through each menu...
            foreach (var menu in Components)
            {
                // Is there a menu for this type?  If so, return it.
                var component = GetExample(menu, type);
                if (component != null)
                {
                    return component;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the sub-menu, if any, matching the specified type.
        /// </summary>
        /// <param name="parent">The parent to start searching from.</param>
        /// <param name="type">The type to find.</param>
        /// <returns>The menu whose link, child type, or group component matches the type.</returns>
        public static MudComponent? GetExample(MudComponent parent, DocumentedType type)
        {
            // Does the name match the menu link?
            if (parent.ComponentName == type.NameFriendly.Replace("<TData>", "<T>"))
            {
                return parent;
            }
            // Are there child types to search?
            if (parent.ChildTypes != null)
            {
                foreach (var childType in parent.ChildTypes)
                {
                    // Does the child type's name match?
                    if (childType.Name == type.Name)
                    {
                        return parent;
                    }
                }
            }
            // Are there sub-menus to search?
            if (parent.IsNavGroup && parent.GroupComponents != null)
            {
                foreach (var subMenu in parent.GroupComponents)
                {
                    // Search one level deeper
                    var component = GetExample(subMenu, type);
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
            return null;
        }
    }
}
