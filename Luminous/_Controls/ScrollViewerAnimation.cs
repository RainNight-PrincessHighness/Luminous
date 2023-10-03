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
    /// <summary>
    /// ScrollViewer scroll animation
    /// </summary>
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
                double newOffset = s_LastLocation - (WheelChange * s_Speed);                
                ScrollToVerticalOffset(s_LastLocation);                
                if (newOffset < 0)
                    newOffset = 0;
                if (newOffset > ScrollableHeight)
                    newOffset = ScrollableHeight;

                AnimateScroll(newOffset);
                s_LastLocation = newOffset;                
                e.Handled = true;
            } else if (ScrollableHeight == 0 && ScrollableWidth != 0)
            {
                double WheelChange = e.Delta;                
                double newOffset = s_HorizontalLasttLocation - (WheelChange * s_Speed);             
                ScrollToHorizontalOffset(s_HorizontalLasttLocation);
                //碰到底部和顶部时的处理
                if (newOffset < 0)
                    newOffset = 0;
                if (newOffset > ScrollableWidth)
                    newOffset = ScrollableWidth;

                AnimateScrollHorizontal(newOffset);
                s_HorizontalLasttLocation = newOffset;

                e.Handled = true;
            } else if (ScrollableHeight != 0 && ScrollableWidth != 0)
            {
                if (s_IsScrolling)
                {
                    double WheelChange = e.Delta;        
                    double newOffset = s_HorizontalLasttLocation - (WheelChange * s_Speed);                  
                    ScrollToHorizontalOffset(s_HorizontalLasttLocation);
            
                    if (newOffset < 0)
                        newOffset = 0;
                    if (newOffset > ScrollableWidth)
                        newOffset = ScrollableWidth;

                    AnimateScrollHorizontal(newOffset);
                    s_HorizontalLasttLocation = newOffset;                   
                    e.Handled = true;
                } else
                {
                    double WheelChange = e.Delta;                   
                    double newOffset = s_LastLocation - (WheelChange * s_Speed);                    
                    ScrollToVerticalOffset(s_LastLocation);
       
                    if (newOffset < 0)
                        newOffset = 0;
                    if (newOffset > ScrollableHeight)
                        newOffset = ScrollableHeight;

                    AnimateScroll(newOffset);
                    s_LastLocation = newOffset;               
                    e.Handled = true;
                }
            }
        }
        private void AnimateScroll(double ToValue)
        {         
            BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, null);
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Animation.From = VerticalOffset;
            Animation.To = ToValue;       
            Animation.Duration = TimeSpan.FromMilliseconds(800);
            //Animation FPS
            //Timeline.SetDesiredFrameRate(Animation, 40);
            BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, Animation);
        }
        private void AnimateScrollHorizontal(double ToValue)
        {           
            BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, null);
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Animation.From = HorizontalOffset;
            Animation.To = ToValue;          
            Animation.Duration = TimeSpan.FromMilliseconds(800);           
            //Timeline.SetDesiredFrameRate(Animation, 40);
            BeginAnimation(ScrollViewerBehavior.HorizontalOffsetProperty, Animation);
        }
    }
}
