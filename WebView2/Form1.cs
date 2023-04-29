using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;

namespace WebView2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            webView21.NavigationStarting += EnsureHttps;

            InitializeAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Form Load");

        }

        private async Task WelcomeWebView(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
   
           await  webView21.CoreWebView2.ExecuteScriptAsync($"Welcome to web view");
   
        }
        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            String uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                webView21.CoreWebView2.ExecuteScriptAsync($"alert('{uri} is not safe, try an https link')");
                args.Cancel = true;
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView21.Size = this.ClientSize - new System.Drawing.Size(webView21.Location);
            goButton.Left = this.ClientSize.Width - goButton.Width;
            addressBar.Width = goButton.Left - addressBar.Left;
        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }

        private async void InitializeAsync()
        {
            await webView21.EnsureCoreWebView2Async(null);
            //Set Default Web Page
            webView21.CoreWebView2.Navigate("https://www.oremanga.net");
            webView21.CoreWebView2.WebMessageReceived += UpdateAddressBar;
            await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
        }

        public void InitBrowser()
        {
            webView21.CoreWebView2.Navigate("https://www.google.com");
        }

        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String uri = args.TryGetWebMessageAsString();
            addressBar.Text = uri;
            Console.WriteLine("URI: " +  uri );
            webView21.CoreWebView2.PostWebMessageAsString(uri);
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("GO BUTTON");
            //webView21.CoreWebView2.ExecuteScriptAsync($"alert('GO!')");

            if (webView21 != null && webView21.CoreWebView2 != null)
            {
                Console.WriteLine("Process 2");
                webView21.CoreWebView2.Navigate(addressBar.Text);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var script = "alert(document.querySelector(\".header-logo\").innerText);";
        //await webView.CoreWebView2.ExecuteScriptAsync("(function() { document.getElementById('user').value = 'blahblahusername'; document.getElementById('password').value = 'blahpassword'; })()");
        //await webView.CoreWebView2.ExecuteScriptAsync("(function() { document.getElementsByName('wp-submit')[0].click(); })()");
            webView21.CoreWebView2
                .ExecuteScriptAsync(script);
        }
    }
}
//https://weblog.west-wind.com/posts/2021/Jan/26/Chromium-WebView2-Control-and-NET-to-JavaScript-Interop-Part-2#summary
