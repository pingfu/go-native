using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using native.Interop;

namespace native
{
    /// <summary>
    /// https://stackoverflow.com/questions/28040646/transparent-blurred-background-to-canvas
    /// https://www.codeproject.com/Articles/881315/Display-HTML-in-WPF-and-CefSharp-Tutorial-Part
    /// https://www.codeproject.com/Articles/1058700/Embedding-Chrome-in-your-Csharp-App-using-CefSharp
    /// https://github.com/cefsharp/CefSharp.MinimalExample
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.StateChanged += OnStateChanged;
        }

        //
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
        }

        private void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);

            var accent = new User32.AccentPolicy
            {
                AccentState = User32.AccentState.ACCENT_ENABLE_BLURBEHIND
            };

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);

            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new User32.WindowCompositionAttributeData
            {
                Attribute = User32.WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            User32.SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                {
                    ViewChangeButton.Text = "1"; // maximise
                    break;
                }
                case WindowState.Maximized:
                {
                    ViewChangeButton.Text = "2"; // restore
                    break;
                }
            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TitleBarBorder_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ViewChangeButton_MouseDown(this, null);
            }
        }

        private void TitleBarBorder_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
            }
        }

        private void MinimizeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void MinimizeButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            MinimizeButton.Background = (Brush)new BrushConverter().ConvertFrom("#FCFCFD"); // active gray
            MinimizeButton.Foreground = (Brush)new BrushConverter().ConvertFrom("#007ACC"); // active blue
        }

        private void MinimizeButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            MinimizeButton.Background = Brushes.Transparent;
            MinimizeButton.Foreground = (Brush)new BrushConverter().ConvertFrom("#282828"); // inactive gray
        }

        private void ViewChangeButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                {
                    SystemCommands.MaximizeWindow(this);
                    break;
                }
                case WindowState.Maximized:
                {
                    SystemCommands.RestoreWindow(this);
                    break;
                }
            }
        }

        private void ViewChangeButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ViewChangeButton.Background = (Brush)new BrushConverter().ConvertFrom("#FCFCFD"); // active gray
            ViewChangeButton.Foreground = (Brush)new BrushConverter().ConvertFrom("#007ACC"); // active blue
        }

        private void ViewChangeButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ViewChangeButton.Background = Brushes.Transparent;
            ViewChangeButton.Foreground = (Brush)new BrushConverter().ConvertFrom("#282828"); // inactive gray
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void CloseButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            CloseButton.Background = (Brush)new BrushConverter().ConvertFrom("#E04343"); // active red
        }

        private void CloseButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            CloseButton.Background = (Brush)new BrushConverter().ConvertFrom("#C75050"); // inactive red
        }
    }
}
