using System;
using System.Collections.Generic;
using System.Text;
using CefSharp;

namespace CefTools
{
    public class CefDisplayHandler : CefSharp.IDisplayHandler
    {
        public Action<int> CursorChangeEvnet;
        public void OnAddressChanged(IWebBrowser chromiumWebBrowser, AddressChangedEventArgs addressChangedArgs)
        {
            return;
        }

        public bool OnAutoResize(IWebBrowser chromiumWebBrowser, IBrowser browser, CefSharp.Structs.Size newSize)
        {
            return false;
        }

        public bool OnConsoleMessage(IWebBrowser chromiumWebBrowser, ConsoleMessageEventArgs consoleMessageArgs)
        {
            return false;
        }

        public bool OnCursorChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr cursor, CefSharp.Enums.CursorType type, CefSharp.Structs.CursorInfo customCursorInfo)
        {
            CursorChangeEvnet?.Invoke((int)type);
            return false;
        }

        public void OnFaviconUrlChange(IWebBrowser chromiumWebBrowser, IBrowser browser, IList<string> urls)
        {
            return;
        }

        public void OnFullscreenModeChange(IWebBrowser chromiumWebBrowser, IBrowser browser, bool fullscreen)
        {
            return;
        }

        public void OnLoadingProgressChange(IWebBrowser chromiumWebBrowser, IBrowser browser, double progress)
        {
            return;
        }

        public void OnStatusMessage(IWebBrowser chromiumWebBrowser, StatusMessageEventArgs statusMessageArgs)
        {
            return;
        }

        public void OnTitleChanged(IWebBrowser chromiumWebBrowser, TitleChangedEventArgs titleChangedArgs)
        {
            return;
        }

        public bool OnTooltipChanged(IWebBrowser chromiumWebBrowser, ref string text)
        {
            return false;
        }
    }
}
