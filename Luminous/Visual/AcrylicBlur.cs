using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;

namespace Luminous.Visual
{
    public class AcrylicBlur
    {
        private readonly Window _window;
        private bool _isEnabled;
        private int _blurColor;

        public AcrylicBlur(Window window) => _window = window ?? throw new ArgumentNullException(nameof(window));

        [DefaultValue(false)]
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnIsEnabledChanged(value);
            }
        }
  
        public Color Color
        {
            get => Color.FromArgb(                
                (byte)((_blurColor & 0x000000ff) >> 0),               
                (byte)((_blurColor & 0x0000ff00) >> 8),             
                (byte)((_blurColor & 0x00ff0000) >> 16),           
                (byte)((_blurColor & 0xff000000) >> 24));
            set => _blurColor =              
                value.R << 0 |         
                value.G << 8 |         
                value.B << 16 |         
                value.A << 24;
        }

        private void OnIsEnabledChanged(bool isEnabled)
        {
            Window window = _window;
            var handle = new WindowInteropHelper(window).EnsureHandle();
            Composite(handle, isEnabled);
        }

        private void Composite(IntPtr handle, bool isEnabled)
        {          
            var osVersion = Environment.OSVersion.Version;
            var windows10_1809 = new Version(10, 0, 17763);
            var windows10 = new Version(10, 0);
          
            var accent = new AccentPolicy();
        
            if (!isEnabled)
            {
                accent.AccentState = AccentState.ACCENT_DISABLED;
            } else if (osVersion > windows10_1809)
            {              
                accent.AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND;
                accent.GradientColor = _blurColor;
            } else if (osVersion > windows10)
            {       
                accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
            } else
            {       
                return;
            }
     
            var accentPolicySize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentPolicySize);
            Marshal.StructureToPtr(accent, accentPtr, false);
         
            try
            {
                var data = new WindowCompositionAttributeData
                {
                    Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                    SizeOfData = accentPolicySize,
                    Data = accentPtr,
                };
                SetWindowCompositionAttribute(handle, ref data);
            } finally
            {
                Marshal.FreeHGlobal(accentPtr);
            }
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
            ACCENT_INVALID_STATE = 5,
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        private enum WindowCompositionAttribute
        {
            // 省略其他未使用的字段
            WCA_ACCENT_POLICY = 19,
            // 省略其他未使用的字段
        }

        public enum BlurSupportedLevel
        {
            NotSupported,
            Aero,
            Blur,
            Acrylic,
        }
    }

}