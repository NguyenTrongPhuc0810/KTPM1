using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;

namespace Vst.Controls
{
    [ContentProperty(nameof(Children))]
    public class Element<T> : UserControl
        where T : UIElement, new()
    {
        static UIElement mouse_down_element;
        public event EventHandler Click;
        protected virtual void RaiseClicked()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        public static readonly DependencyProperty ChildrenProperty =
            DependencyProperty.RegisterAttached(nameof(Children),
            typeof(UIElementCollection),
            typeof(Element<T>),
            null);
        public UIElementCollection Children
        {
            get { return (UIElementCollection)GetValue(ChildrenProperty); }
            protected set { SetValue(ChildrenProperty, value); }
        }

        public T Layout { get; private set; }
        public Element()
        {
            PreviewMouseLeftButtonDown += (s, e) => mouse_down_element = this;
            PreviewMouseLeftButtonUp += (s, e) =>
            {
                if (mouse_down_element == this)
                {
                    RaiseClicked();
                }
                mouse_down_element = null;
            };

            var panel = new T();

            Content = panel;
            Layout = panel;
            Children = (panel as Panel).Children;
        }
    }

    public class Grid : Element<System.Windows.Controls.Grid>
    {
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached(nameof(Columns),
            typeof(ColumnDefinitionCollection),
            typeof(Grid),
            null);
        public ColumnDefinitionCollection Columns
        {
            get { return (ColumnDefinitionCollection)GetValue(ColumnsProperty); }
            private set { SetValue(ColumnsProperty, value); }
        }
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.RegisterAttached(nameof(Rows),
            typeof(RowDefinitionCollection),
            typeof(Grid),
            null);
        public RowDefinitionCollection Rows
        {
            get { return (RowDefinitionCollection)GetValue(RowsProperty); }
            private set { SetValue(RowsProperty, value); }
        }
        public Grid()
        {
            Rows = Layout.RowDefinitions;
            Columns = Layout.ColumnDefinitions;
        }
    }
    public class TextBase : Grid
    {
        public static readonly DependencyProperty HorizontalTextAlignmentProperty =
            DependencyProperty.RegisterAttached(
                nameof(HorizontalTextAlignment),
                typeof(HorizontalAlignment),
                typeof(TextBase),
                new PropertyMetadata(default(HorizontalAlignment)));
        public HorizontalAlignment HorizontalTextAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalTextAlignmentProperty);
            set => SetValue(HorizontalTextAlignmentProperty, value);
        }
        public static readonly DependencyProperty VerticalTextAlignmentProperty =
            DependencyProperty.RegisterAttached(nameof(VerticalTextAlignment),
                typeof(VerticalAlignment), typeof(TextBase), null);
        public VerticalAlignment VerticalTextAlignment
        {
            get => (VerticalAlignment)GetValue(VerticalTextAlignmentProperty);
            set => SetValue(VerticalTextAlignmentProperty, value);
        }
        public string Text
        {
            get => _caption.Text;
            set => _caption.Text = value;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _caption.HorizontalAlignment = HorizontalTextAlignment;
            _caption.VerticalAlignment = VerticalTextAlignment;
            return base.MeasureOverride(constraint);
        }
        protected TextBlock _caption;
        public TextBase()
        {
            Children.Add(_caption = new TextBlock { });
        }
    }
    public class ButtonBase : TextBase
    {
        protected override void RaiseClicked()
        {
            base.RaiseClicked();
            if (!string.IsNullOrWhiteSpace(Url))
            {
                System.Mvc.Engine.Execute(Url);
            }
        }
        public string Url { get; set; }
        public ButtonBase()
        {
            VerticalTextAlignment = VerticalAlignment.Center;
            Padding = new Thickness(5);
            Cursor = Cursors.Hand;
        }
    }
    public class Button : ButtonBase
    {
        public static readonly DependencyProperty BorderRadiusProperty =
            DependencyProperty.RegisterAttached(nameof(BorderRadius),
            typeof(double),
            typeof(Button),
            new PropertyMetadata(default(double)));
        public double BorderRadius
        {
            get => (double)GetValue(BorderRadiusProperty);
            set => SetValue(BorderRadiusProperty, value);
        }

        public Button()
        {
            Text = "Button";
            HorizontalTextAlignment = HorizontalAlignment.Center;
        }
        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
            double r = Math.Min(BorderRadius, ActualHeight / 2);
            if (r > 0)
            {
                return new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight), r, r);
            }
            return base.GetLayoutClip(layoutSlotSize);
        }

    }
    public class MenuButton : ButtonBase
    {
        public MenuButton()
        {
            Text = "Menu Item";
        }
    }
    public class HorizontalMenu : Element<StackPanel>
    {
        public HorizontalMenu()
        {
            Layout.Orientation = Orientation.Horizontal;
        }
    }

    public class SideMenuCaption : TextBase
    {

    }
    public class SideMenuButton : MenuButton
    {
    }
    public class SideMenu : Element<StackPanel>
    {
        SideMenuCaption _cap = new SideMenuCaption();
        public string Text
        {
            get => _cap.Text;
            set => _cap.Text = value;
        }
        public SideMenu()
        {
            var p = new StackPanel();

            Children.Add(_cap);
            Children.Add(p);

            Children = p.Children;
            for (int i = 0; i < 4; i++)
            {
                Children.Add(new SideMenuButton { Text = "side menu " + (i + 1) });
            }
        }
    }
}
