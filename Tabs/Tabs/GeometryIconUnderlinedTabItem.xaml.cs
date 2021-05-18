﻿using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace Sharpnado.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeometryIconUnderlinedTabItem : UnderlinedTabItemBase
    {
        public static readonly BindableProperty GeometryIconProperty = BindableProperty.Create(
           nameof(GeometryIcon),
           typeof(Geometry),
           typeof(GeometryIconUnderlinedTabItem),
           null);

        public static readonly BindableProperty FillProperty = BindableProperty.Create(
           nameof(Fill),
           typeof(bool),
           typeof(GeometryIconUnderlinedTabItem),
           true);

        public static readonly BindableProperty StrokeThicknessProperty = BindableProperty.Create(
           nameof(StrokeThickness),
           typeof(double),
           typeof(GeometryIconUnderlinedTabItem),
           0.1);

        public static readonly BindableProperty IconOrientationProperty = BindableProperty.Create(
           nameof(Orientation),
           typeof(IconOrientation),
           typeof(GeometryIconUnderlinedTabItem),
           IconOrientation.Top);

        public GeometryIconUnderlinedTabItem()
        {
            InitializeComponent();

            InnerLabelImpl.PropertyChanged += InnerLabelPropertyChanged;
        }

        [TypeConverter(typeof(PathGeometryConverter))]
        public Geometry GeometryIcon
        {
            get => (Geometry)GetValue(GeometryIconProperty);
            set => SetValue(GeometryIconProperty, value);
        }

        public IconOrientation Orientation
        {
            get => (IconOrientation)GetValue(IconOrientationProperty);
            set => SetValue(IconOrientationProperty, value);
        }

        public bool Fill
        {
            get => (bool)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        protected override Label InnerLabelImpl => InnerLabel;

        protected override Grid GridImpl => Grid;

        protected override BoxView UnderlineImpl => Underline;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(UnselectedLabelColor):
                case nameof(SelectedTabColor):
                case nameof(IsSelected):
                case nameof(StrokeThickness):
                case nameof(Fill):
                case nameof(GeometryIcon):
                    UpdateGeometryIcon();
                    break;
                case nameof(Orientation):
                    UpdateIconAndTextLayout();
                    break;
            }
        }

        private void UpdateIconAndTextLayout()
        {
            if (Orientation == IconOrientation.None)
            {
                MainLayout.Spacing = 0;
                IconPath.IsVisible = false;
                InnerLabel.IsVisible = true;
            }
            else if (Orientation == IconOrientation.IconOnly)
            {
                MainLayout.Spacing = 0;
                IconPath.IsVisible = true;
                InnerLabel.IsVisible = false;
            }
            else
            {
                MainLayout.Spacing = 5;
                IconPath.IsVisible = true;
                InnerLabel.IsVisible = true;
                MainLayout.Orientation = Orientation == IconOrientation.Top
                    ? StackOrientation.Vertical : StackOrientation.Horizontal;
            }
        }

        private void UpdateGeometryIcon()
        {
            var brush = new SolidColorBrush(IsSelected ? SelectedTabColor : UnselectedLabelColor);
            if (Fill)
            {
                IconPath.Fill = brush;
            }

            IconPath.Stroke = brush;
            IconPath.StrokeThickness = StrokeThickness;
        }
    }
}