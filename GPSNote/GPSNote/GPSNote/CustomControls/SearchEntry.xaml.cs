using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNote.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchEntry : ContentView
    {
        public SearchEntry()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty TextEntryProperty =
            BindableProperty.Create(nameof(TextEntry),
                                    typeof(string),
                                    typeof(SearchEntry),
                                    defaultValue: string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: TextEntryPropertyChanged);


        public string TextEntry
        {
            get => (string)GetValue(TextEntryProperty);
            set => SetValue(TextEntryProperty, value);
        }


        public static readonly BindableProperty LeftIconTapCommandProperty =
            BindableProperty.Create(nameof(LeftIconTapCommand),
                                    typeof(ICommand),
                                    typeof(SearchEntry),
                                    defaultValue: default(ICommand),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: LeftIconTapCommandPropertyChanged);


        public ICommand LeftIconTapCommand
        {
            get => (ICommand)GetValue(LeftIconTapCommandProperty);
            set => SetValue(LeftIconTapCommandProperty, value);
        }


        public static readonly BindableProperty RightIconTapCommandProperty =
           BindableProperty.Create(nameof(RightIconTapCommand),
                                   typeof(ICommand),
                                   typeof(SearchEntry),
                                   defaultValue: default(ICommand),
                                   defaultBindingMode: BindingMode.TwoWay,
                                   propertyChanged: RightIconTapCommandPropertyChanged);

        public ICommand RightIconTapCommand
        {
            get => (ICommand)GetValue(RightIconTapCommandProperty);
            set => SetValue(RightIconTapCommandProperty, value);
        }


        public static readonly BindableProperty IsEntryFocusedProperty =
           BindableProperty.Create(nameof(IsEntryFocused),
                                   typeof(bool),
                                   typeof(SearchEntry),
                                   defaultValue: false,
                                   defaultBindingMode: BindingMode.TwoWay,
                                   propertyChanged: IsEntryFocusedPropertyChanged);


        public bool IsEntryFocused
        {
            get => (bool)GetValue(IsEntryFocusedProperty);
            set => SetValue(IsEntryFocusedProperty, value);
        }



        public static readonly BindableProperty EntryBorderColorProperty =
                                    BindableProperty.Create(nameof(EntryBorderColor),
                                    typeof(Color),
                                    typeof(SearchEntry),
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
                                    typeof(SearchEntry),
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
                            typeof(SearchEntry),
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

        private static void LeftIconTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SearchEntry navBar = bindable as SearchEntry;

            if (navBar != null)
            {
                navBar.leftButton.Command = (ICommand)newValue;
            }
        }


        private static void RightIconTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SearchEntry navBar = bindable as SearchEntry;

            if (navBar != null)
            {
                navBar.rightButton.Command = (ICommand)newValue;
            }
        }


        private static void EntryBackGroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SearchEntry customEntry = bindable as SearchEntry;
            if (customEntry != null)
            {
                customEntry.frame.BackgroundColor = (Color)newValue;
            }
        }


        private static void EntryBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SearchEntry customEntry = bindable as SearchEntry;
            if (customEntry != null)
            {
                customEntry.frame.BorderColor = (Color)newValue;
            }
        }

        private static void TextEntryPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SearchEntry customEntry = bindable as SearchEntry;
            if (customEntry != null)
            {
                customEntry.entry.Text = (string)newValue;
            }
        }



        private static void IsEntryFocusedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SearchEntry customEntry = bindable as SearchEntry;
            if (customEntry != null)
            {
                
            }
        }

        private static void PlaceholderTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SearchEntry customEntry = bindable as SearchEntry;

            if (customEntry != null)
            {
                customEntry.entry.Placeholder = (string)newValue;
            }
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            clearButton.IsVisible = false;
            backButton.IsVisible = false;
            leftButton.IsVisible = true;
            IsEntryFocused = false;
            rightButton.IsVisible = true;
            Grid.SetColumnSpan(frame, 1);
        }



        private void OnFocused(object sender, FocusEventArgs e)
        {
            IsEntryFocused = true;
            backButton.IsVisible = true;
            leftButton.IsVisible = false;
            clearButton.IsVisible = true;
            rightButton.IsVisible = false;
            Grid.SetColumnSpan(frame, 2);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextEntry = e.NewTextValue;
        }


        private void ClearButtonClicked(object sender, EventArgs e)
        {
            entry.Text = string.Empty;
        }


        private void BackButtonClicked(object sender, EventArgs e)
        {
            IsEntryFocused = false;
        }

        
        
        #endregion
    }
}
