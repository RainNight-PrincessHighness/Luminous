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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Luminous._Controls
{
    /// <summary>
    /// DButton.xaml 的交互逻辑
    /// </summary>
    public partial class DButton : Button
    {
        public DButton()
        {
            InitializeComponent();
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
        public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(DButton));

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = this.Template.FindName("Bor", this) as Border;
            border.CornerRadius = CornerRadius;
        }
    }
}
