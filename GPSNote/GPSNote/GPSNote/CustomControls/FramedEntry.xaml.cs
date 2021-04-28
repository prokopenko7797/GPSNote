using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNote.CustomControls
{
    public partial class FramedEntry : ContentView
    {
        public FramedEntry()
        {
            InitializeComponent();

        }

        #region -- Public properties --

        public static readonly BindableProperty TextEntryProperty =
            BindableProperty.Create(nameof(TextEntry),
                                    typeof(string),
                                    typeof(FramedEntry),
                                    defaultValue: string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: TextEntryPropertyChanged);


        public string TextEntry
        {
            get => (string)GetValue(TextEntryProperty);
            set => SetValue(TextEntryProperty, value);
        }


        public static readonly BindableProperty EntryBorderColorProperty =
                                    BindableProperty.Create(nameof(EntryBorderColor),
                                    typeof(Color),
                                    typeof(FramedEntry),
                                    defaultValue: Color.FromHex("#858E9E"),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: EntryBorderColorChanged);


        public string EntryBorderColor
        {
            get => (string)GetValue(EntryBorderColorProperty);
            set => SetValue(EntryBorderColorProperty, value);
        }

        public static readonly BindableProperty PlaceholderTextProperty =
            BindableProperty.Create(nameof(PlaceholderText),
                                    typeof(string),
                                    typeof(FramedEntry),
                                    defaultValue: default,
                                    propertyChanged: PlaceholderTextPropertyChanged);


        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }


        public static readonly BindableProperty EntryBackGroundColorProperty =
                            BindableProperty.Create(nameof(EntryBackGroundBorderColor),
                            typeof(Color),
                            typeof(FramedEntry),
                            defaultValue: Color.FromHex("#00ffffff"),
                            defaultBindingMode: BindingMode.TwoWay,
                            propertyChanged: EntryBackGroundColorChanged);


        public string EntryBackGroundBorderColor
        {
            get => (string)GetValue(EntryBorderColorProperty);
            set => SetValue(EntryBorderColorProperty, value);
        }

        #endregion


        #region -- Private helpers --


        private static void EntryBackGroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            FramedEntry customEntry = bindable as FramedEntry;
            if (customEntry != null)
            {
                customEntry.frame.BackgroundColor = (Color)newValue;
            }
        }


        private static void EntryBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            FramedEntry customEntry = bindable as FramedEntry;
            if (customEntry != null)
            {
                customEntry.frame.BorderColor = (Color)newValue;
            }
        }

        private static void TextEntryPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            FramedEntry customEntry = bindable as FramedEntry;
            if (customEntry != null)
            {
                customEntry.entry.Text = (string)newValue;
            }
        }


        private static void PlaceholderTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            FramedEntry customEntry = bindable as FramedEntry;

            if (customEntry != null)
            {
                customEntry.entry.Placeholder = (string)newValue;
            }
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            clearButton.IsVisible = false;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextEntry = e.NewTextValue;

            if (entry.Text != string.Empty)
            {
                clearButton.IsVisible = true;
            }
            else
            {
                frame.BorderColor = ((Color)App.Current.Resources["System/LightGray"]);
                clearButton.IsVisible = false;
            }
        }


        private void ClearButtonClicked(object sender, EventArgs e)
        {
            entry.Text = string.Empty;
        }

        #endregion
    }
}