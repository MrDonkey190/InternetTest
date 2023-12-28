﻿/*
MIT License

Copyright (c) Léo Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/
using InternetTest.Classes;
using InternetTest.UserControls;
using Synethia;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InternetTest.Pages
{
	/// <summary>
	/// Interaction logic for DownDetectorPage.xaml
	/// </summary>
	public partial class DownDetectorPage : Page
	{
		bool codeInjected = !Global.Settings.UseSynethia;

		public DownDetectorPage()
		{
			InitializeComponent();
			InitUI(); // Load the UI
			Loaded += (o, e) => SynethiaManager.InjectSynethiaCode(this, Global.SynethiaConfig.PagesInfo, 0, ref codeInjected);
		}

		private void InitUI()
		{
			TitleTxt.Text = $"{Properties.Resources.WebUtilities} > {Properties.Resources.DownDetector}"; // Set the title of the page
		}
		private void ClearBtn_Click(object sender, RoutedEventArgs e)
		{
			WebsiteTxt.Text = string.Empty;
		}

		internal List<string> Websites = new();
		private void AddBtn_Click(object sender, RoutedEventArgs e)
		{
			if (!Global.IsUrlValid(WebsiteTxt.Text) || Websites.Contains(WebsiteTxt.Text)) return;
			if (!WebsiteTxt.Text.StartsWith("http"))
			{
				WebsiteTxt.Text = (Global.Settings.UseHttps ? "https://" : "http://") + WebsiteTxt.Text;
			}

			WebsiteDisplayer.Children.Add(new WebsiteItem(WebsiteTxt.Text));
			Websites.Add(WebsiteTxt.Text);
			WebsiteTxt.Text = string.Empty;
		}

		private void TestBtn_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 0; i < WebsiteDisplayer.Children.Count; i++)
			{
				if (WebsiteDisplayer.Children[i] is WebsiteItem websiteItem)
				{
					websiteItem.LaunchTestAsync();
				}
			}
		}

		private void IntervalTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void WebsiteTxt_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				AddBtn_Click(sender, e);
			}
		}
	}
}
