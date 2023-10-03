using Luminous.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Luminous._Windows
{
    public static class Extion
    {
        public static void EnableBlur(this Window window, Color BlurBackground)
        {
            AcrylicBlur windowAccentCompositor = new AcrylicBlur(window);
            windowAccentCompositor.Color = BlurBackground;
            windowAccentCompositor.IsEnabled = true;
        }
        public static void EnableBlur(this Window window)
        {
            AcrylicBlur windowAccentCompositor = new AcrylicBlur(window);
            windowAccentCompositor.Color = Color.FromArgb(1, 255, 255, 255);
            windowAccentCompositor.IsEnabled = true;
        }
        /// <summary>
        /// 隐藏控件获得焦点时的虚线框
        /// </summary>
        /// <param name="root"></param>
        public static void HideBoundingBox(object root)
        {
            Control control = root as Control;
            if (control != null)
            {
                control.FocusVisualStyle = null;
            }

            if (root is DependencyObject)
            {
                foreach (object child in LogicalTreeHelper.GetChildren((DependencyObject)root))
                {
                    HideBoundingBox(child);
                }
            }
        }
    }
    /// <summary>
    /// LWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LWindow : Window
    {

        WindowInteropHelper windowInteropHelper;
        public AcrylicBlur visual_MainWindows;
        public static bool IsWin7 => Environment.OSVersion.Version.Major == 6
                    && Environment.OSVersion.Version.Minor == 1;
        public static bool IsWin10 => Environment.OSVersion.Version.Major == 10;
        public LWindow()
        {
            if (!IsWin10)
            { //检测是否Win10及以上系统
                Environment.Exit(0);//非Win10及以上 退出程序
            }
            InitializeComponent();
            visual_MainWindows = new AcrylicBlur(this);
            this.EnableBlur(Color.FromArgb(5, 42, 42, 42));
            this.SetRoundedCorners();
            windowInteropHelper = new WindowInteropHelper(this);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void _Titleborder_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) DragMove();
        }

    }
}
