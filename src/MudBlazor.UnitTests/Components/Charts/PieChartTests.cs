﻿// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Bunit;
using FluentAssertions;
using MudBlazor.Charts;
using MudBlazor.UnitTests.Components;
using NUnit.Framework;

namespace MudBlazor.UnitTests.Charts
{
    public class PieChartTests : BunitTest
    {
        private readonly string[] _baseChartPalette =
        {
            "#2979FF", "#1DE9B6", "#FFC400", "#FF9100", "#651FFF", "#00E676", "#00B0FF", "#26A69A", "#FFCA28",
            "#FFA726", "#EF5350", "#EF5350", "#7E57C2", "#66BB6A", "#29B6F6", "#FFA000", "#F57C00", "#D32F2F",
            "#512DA8", "#616161"
        };

        private readonly string[] _modifiedPalette =
        {
            "#264653", "#2a9d8f", "#e9c46a", "#f4a261", "#e76f51"
        };

        private readonly string[] _customPalette =
        {
            "#015482", "#CC1512", "#FFE135", "#087830", "#D70040", "#B20931", "#202E54", "#F535AA", "#017B92",
            "#FA4224", "#062A78", "#56B4BE", "#207000", "#FF43A4", "#FB8989", "#5E9B8A", "#FFB7CE", "#C02B18",
            "#01153E", "#2EE8BB", "#EBDDE2"
        };

        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void PieChartEmptyData()
        {
            var comp = Context.RenderComponent<Pie>(parameters => parameters
                .Add(p => p.InputData, null));

            comp.Markup.Should().Contain("mud-chart-pie");
            comp.Instance.InputData.Should().BeNull();
        }

        [Theory]
        [TestCase(new double[] { 77, 25, 20, 5 })]
        [TestCase(new double[] { 77, 25, 20, 5, 8 })]
        public void PieChartExampleData(double[] data)
        {
            string[] labels = { "Uranium", "Plutonium", "Thorium", "Caesium", "Technetium", "Promethium",
                "Polonium", "Astatine", "Radon", "Francium", "Radium", "Actinium", "Protactinium",
                "Neptunium", "Americium", "Curium", "Berkelium", "Californium", "Einsteinium", "Mudblaznium" };

            var comp = Context.RenderComponent<MudChart>(parameters => parameters
                .Add(p => p.ChartType, ChartType.Pie)
                .Add(p => p.ChartOptions, new ChartOptions { ChartPalette = _baseChartPalette })
                .Add(p => p.Height, "300px")
                .Add(p => p.Width, "300px")
                .Add(p => p.InputData, data)
                .Add(p => p.InputLabels, labels));

            comp.Markup.Should().Contain("class=\"mud-chart-pie\"");
            comp.Markup.Should().Contain("class=\"mud-chart-serie\"");
            comp.Markup.Should().Contain("mud-chart-legend-item");

            if (data.Length <= 4)
            {
                comp.Markup.Should().
                    Contain("Uranium").And.Contain("Plutonium").And.Contain("Thorium").And.Contain("Caesium");
            }

            if (data.Length >= 5)
            {
                comp.Markup.Should()
                    .Contain("Technetium");
            }
            if (data.Length == 4 && data.Contains(77))
            {
                comp.Markup.Should()
                    .Contain("d=\"M 0 -140 A 140 140 0 1 1 -86.7071 109.9176 L 0 0 Z\"");
            }

            if (data.Length == 4 && data.Contains(5))
            {
                comp.Markup.Should()
                    .Contain("d=\"M -34.2796 -135.7384 A 140 140 0 0 1 -0 -140 L 0 0 Z\"");
            }

            comp.SetParametersAndRender(parameters => parameters
                .Add(p => p.ChartOptions, new ChartOptions() { ChartPalette = _modifiedPalette }));

            comp.Markup.Should().Contain(_modifiedPalette[0]);
        }

        [Test]
        public void PieChartColoring()
        {
            double[] data = { 50, 25, 20, 5, 16, 14, 8, 4, 2, 8, 10, 19, 8, 17, 6, 11, 19, 24, 35, 13, 20, 12 };

            var comp = Context.RenderComponent<MudChart>(parameters => parameters
                .Add(p => p.ChartType, ChartType.Pie)
                .Add(p => p.Height, "350px")
                .Add(p => p.Width, "100%")
                .Add(p => p.ChartOptions, new ChartOptions { ChartPalette = new string[] { "#1E9AB0" } })
                .Add(p => p.InputData, data));

            var paths1 = comp.FindAll("path");

            int count;
            count = paths1.Count(p => p.OuterHtml.Contains($"fill=\"{"#1E9AB0"}\""));
            count.Should().Be(22);

            comp.SetParametersAndRender(parameters => parameters
                .Add(p => p.ChartOptions, new ChartOptions() { ChartPalette = _customPalette }));

            var paths2 = comp.FindAll("path");

            foreach (var color in _customPalette)
            {
                count = paths2.Count(p => p.OuterHtml.Contains($"fill=\"{color}\""));
                if (color == _customPalette[0])
                {
                    count.Should().Be(2, because: "the number of data points defined exceeds the number of colors in the chart palette, thus, any new defined data point takes the color from the chart palette in the same fashion as the previous data points starting from the beginning");
                }
                else
                {
                    count.Should().Be(1);
                }
            }
        }

        [Test]
        public void PieChart100Percent()
        {
            double[] data = { 50, 0, 0 };

            var comp = Context.RenderComponent<MudChart>(parameters => parameters
                .Add(p => p.ChartType, ChartType.Pie)
                .Add(p => p.InputData, data));

            comp.Markup.Should().Contain("d=\"M 0 -140 A 140 140 0 1 1 0 140 A 140 140 0 1 1 -0 -140 L 0 0 Z\"");
        }
    }
}
