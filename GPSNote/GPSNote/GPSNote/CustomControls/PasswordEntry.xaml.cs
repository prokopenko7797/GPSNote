using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNote.CustomControls
{
    public partial class PasswordEntry : ContentView
    {
        public PasswordEntry()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty TextEntryProperty =
            BindableProperty.Create(nameof(TextEntry),
                                    typeof(string),
                                    typeof(PasswordEntry),
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
                                    typeof(PasswordEntry),
                                    defaultValue: (Color)App.Current.Resources["System/LightGray"],
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
                                    typeof(PasswordEntry),
                                    defaultValue: default,
                                    propertyChanged: PlaceholderTextPropertyChanged);


        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        #endregion


        #region -- Private helpers --

        
        private static void EntryBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PasswordEntry customEntry = bindable as PasswordEntry;
            if (customEntry != null)
            {
                customEntry.frame.BorderColor = (Color)newValue;
            }
        }

        private static void TextEntryPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PasswordEntry customEntry = bindable as PasswordEntry;
            if (customEntry != null)
            {
                customEntry.entry.Text = (string)newValue;
            }
        }


        private static void PlaceholderTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PasswordEntry customEntry = bindable as PasswordEntry;

            if (customEntry != null)
            {
                customEntry.entry.Placeholder = (string)newValue;
            }
        }



        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextEntry = e.NewTextValue;

            if (entry.Text != string.Empty)
            {
                eyeButton.IsVisible = true;
            }
            else
            {
                eyeButton.IsVisible = false;
                frame.BorderColor = ((Color)App.Current.Resources["System/LightGray"]);
            }
        }


        private void EyeButtonClicked(object sender, EventArgs e)
        {
            entry.IsPassword = !entry.IsPassword;
            if (entry.IsPassword)
            {
                eyeButton.Source = "ic_eye.png";
            }
            else
            {
                eyeButton.Source = "ic_eye_off.png";
            }
        }

        #endregion
    }
}