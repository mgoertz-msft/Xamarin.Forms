﻿using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Platform.Handlers
{
	public partial class SearchBarHandler : AbstractViewHandler<ISearch, UISearchBar>
	{
		static UIColor? CancelButtonTextColorDefaultDisabled;
		static UIColor? CancelButtonTextColorDefaultHighlighted;
		static UIColor? CancelButtonTextColorDefaultNormal;

		static UIColor? DefaultTextColor;
		static UIColor? DefaultTintColor;

		static UITextField? TextField;
		static string? TypedText;

		protected override UISearchBar CreateNativeView()
		{
			var searchBar = new UISearchBar(CGRect.Empty) { ShowsCancelButton = true, BarStyle = UIBarStyle.Default };

			TextField = searchBar.FindDescendantView<UITextField>();

			return searchBar;
		}

		protected override void ConnectHandler(UISearchBar nativeView)
		{
			nativeView.CancelButtonClicked += OnCancelClicked;
			nativeView.SearchButtonClicked += OnSearchButtonClicked;
			nativeView.TextChanged += OnTextChanged;
			nativeView.ShouldChangeTextInRange += ShouldChangeText;

			nativeView.OnEditingStarted += OnEditingStarted;
			nativeView.OnEditingStopped += OnEditingEnded;
		}

		protected override void DisconnectHandler(UISearchBar nativeView)
		{
			nativeView.CancelButtonClicked -= OnCancelClicked;
			nativeView.SearchButtonClicked -= OnSearchButtonClicked;
			nativeView.TextChanged -= OnTextChanged;
			nativeView.ShouldChangeTextInRange -= ShouldChangeText;

			nativeView.OnEditingStarted -= OnEditingStarted;
			nativeView.OnEditingStopped -= OnEditingEnded;
		}

		protected override void SetupDefaults(UISearchBar nativeView)
		{
			base.SetupDefaults(nativeView);

			DefaultTintColor = nativeView.BarTintColor;

			var cancelButton = nativeView.FindDescendantView<UIButton>();
			CancelButtonTextColorDefaultNormal = cancelButton?.TitleColor(UIControlState.Normal);
			CancelButtonTextColorDefaultHighlighted = cancelButton?.TitleColor(UIControlState.Highlighted);
			CancelButtonTextColorDefaultDisabled = cancelButton?.TitleColor(UIControlState.Disabled);

			TextField ??= nativeView.FindDescendantView<UITextField>();
			DefaultTextColor = TextField?.TextColor;
		}

		public static void MapCancelButtonColor(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateCancelButtonColor(search, CancelButtonTextColorDefaultNormal, CancelButtonTextColorDefaultHighlighted, CancelButtonTextColorDefaultDisabled);
		}

		public static void MapText(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateText(search);
			handler.TypedNativeView?.UpdateCancelButtonColor(search);
		}

		public static void MapTextColor(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateTextColor(TextField, search, DefaultTextColor);
		}

		public static void MapTextTransform(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateTextTransform(search);
		}

		public static void MapCharacterSpacing(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateCharacterSpacing(TextField, search);
		}

		public static void MapPlaceholder(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdatePlaceholder(search);
		}

		public static void MapPlaceholderColor(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdatePlaceholderColor(search);
		}

		public static void MapFontAttributes(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateFontAttributes(TextField, search);
		}

		public static void MapFontFamily(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateFontFamily(TextField, search);
		}

		public static void MapFontSize(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateFontSize(TextField, search);
		}

		public static void MapMaxLength(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateMaxLength(search);
		}

		public static void MapKeyboard(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateKeyboard(search);
		}

		public static void MapIsSpellCheckEnabled(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateIsSpellCheckEnabled(search);
		}

		public static void MapHorizontalTextAlignment(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateHorizontalTextAlignment(TextField, search);
		}

		public static void MapVerticalTextAlignment(SearchBarHandler handler, ISearch search)
		{
			ViewHandler.CheckParameters(handler, search);

			handler.TypedNativeView?.UpdateVerticalTextAlignment(TextField, search);
		}

		void OnCancelClicked(object sender, EventArgs args)
		{
			if (VirtualView != null)
				VirtualView.Text = string.Empty;

			TypedNativeView?.ResignFirstResponder();
		}

		void OnSearchButtonClicked(object sender, EventArgs e)
		{
			VirtualView?.SearchButtonPressed();
			TypedNativeView?.ResignFirstResponder();
		}

		void OnTextChanged(object sender, UISearchBarTextChangedEventArgs a)
		{
			// This only fires when text has been typed into the SearchBar; see UpdateText()
			// for why this is handled in this manner.
			TypedText = a.SearchText;
			UpdateOnTextChanged();
		}

		bool ShouldChangeText(UISearchBar searchBar, NSRange range, string text)
		{
			var newLength = searchBar?.Text?.Length + text.Length - range.Length;
			return newLength <= VirtualView?.MaxLength;
		}

		void OnEditingStarted(object sender, EventArgs e)
		{

		}

		void OnEditingEnded(object sender, EventArgs e)
		{
		
		}

		void UpdateOnTextChanged()
		{
			if (VirtualView != null)
				VirtualView.Text = TypedText ?? string.Empty;
		}
	}
}