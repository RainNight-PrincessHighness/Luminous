using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Luminous._Controls
{
    public class ScrollViewerAnimation : ScrollViewer
    {       
        private double s_LastLocation = 0;
        private double s_HorizontalLasttLocation = 0;
        private bool s_IsScrolling = false;
        private float s_Speed = 0.5f;
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                s_IsScrolling = true;
            }
        }
        protected override void OnPreviewKeyUp(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl)
            {
                s_IsScrolling = false;
            }
        }
        //重写鼠标滚动事件
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (ScrollableHeight != 0 && ScrollableWidth == 0)
            {
                double WheelChange = e.Delta;
                //可以更改一次滚动的距离倍数 (WheelChange可能为正负数!)
                double newOffset = s_LastLocation - (WheelChange * s_Speed);
                //Animation并不会改变真正的VerticalOffset(只是它的依赖属性) 所以将VOffset设置到上一次的滚动位置 (相当于衔接上一个动画)
                ScrollToVerticalOffset(s_LastLocation);
                //碰到底部和顶部时的处理
                if (newOffset < 0)
                    newOffset = 0;
                if (newOffset > ScrollableHeight)
                    newOffset = ScrollableHeight;

                AnimateScroll(newOffset);
                s_LastLocation = newOffset;
                //告诉ScrollViewer我们已经完成了滚动
                e.Handled = true;
            } else if (ScrollableHeight == 0 && ScrollableWidth != 0)
            {
                double WheelChange = e.Delta;
                //可以更改一次滚动的距离倍数 (WheelChange可能为正负数!)
                double newOffset = s_HorizontalLasttLocation - (WheelChange * s_Speed);
                //Animation并不会改变真正的VerticalOffset(只是它的依赖属性) 所以将VOffset设置到上一次的滚动位置 (相当于衔接上一个动画)
                ScrollToHorizontalOffset(s_HorizontalLasttLocation);
                //碰到底部和顶部时的处理
                if (newOffset < 0)
                    newOffset = 0;
                if (newOffset > ScrollableWidth)
                    newOffset = ScrollableWidth;

                AnimateScrollHorizontal(newOffset);
                s_HorizontalLasttLocation = newOffset;
                //告诉ScrollViewer我们已经完成了滚动
                e.Handled = true;
            } else if (ScrollableHeight != 0 && ScrollableWidth != 0)
            {
                if (s_IsScrolling)
                {
                    double WheelChange = e.Delta;
                    //可以更改一次滚动的距离倍数 (WheelChange可能为正负数!)
                    double newOffset = s_HorizontalLasttLocation - (WheelChange * s_Speed);
                    //Animation并不会改变真正的VerticalOffset(只是它的依赖属性) 所以将VOffset设置到上一次的滚动位置 (相当于衔接上一个动画)
                    ScrollToHorizontalOffset(s_HorizontalLasttLocation);
                    //碰到底部和顶部时的处理
                    if (newOffset < 0)
                        newOffset = 0;
                    if (newOffset > ScrollableWidth)
                        newOffset = ScrollableWidth;

                    AnimateScrollHorizontal(newOffset);
                    s_HorizontalLasttLocation = newOffset;
                    //告诉ScrollViewer我们已经完成了滚动
                    e.Handled = true;
                } else
                {
                    double WheelChange = e.Delta;
                    //可以更改一次滚动的距离倍数 (WheelChange可能为正负数!)
                    double newOffset = s_LastLocation - (WheelChange * s_Speed);
                    //Animation并不会改变真正的VerticalOffset(只是它的依赖属性) 所以将VOffset设置到上一次的滚动位置 (相当于衔接上一个动画)
                    ScrollToVerticalOffset(s_LastLocation);
                    //碰到底部和顶部时的处理
                    if (newOffset < 0)
                        newOffset = 0;
                    if (newOffset > ScrollableHeight)
                        newOffset = ScrollableHeight;

                    AnimateScroll(newOffset);
                    s_LastLocation = newOffset;
                    //告诉ScrollViewer我们已经完成了滚动
                    e.Handled = true;
                }
            }
        }
        private void AnimateScroll(double ToValue)
        {
            //为了避免重复，先结束掉上一个动画
            BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, null);
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Animation.From = VerticalOffset;
            Animation.To = ToValue;
            //动画速度
            Animation.Duration = TimeSpan.FromMilliseconds(800);
            //考虑到性能，可以降低动画帧数
            //Timeline.SetDesiredFrameRate(Animation, 40);
            BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, Animation);
        }
        private void AnimateScrollHorizontal(double ToValue)
        {
            //为了避免重复，先结束掉上一个动画
            BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, null);
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Animation.From = HorizontalOffset;
            Animation.To = ToValue;
            //动画速度
            Animation.Duration = TimeSpan.FromMilliseconds(800);
            //考虑到性能，可以降低动画帧数
            //Timeline.SetDesiredFrameRate(Animation, 40);
            BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, Animation);
        }
    }
}
