using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.ViewModels.ExtendedViewModels
{
    public class PinViewModel : BindableBase
    {

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }

        private int _UserId;
        public int UserId
        {
            get { return _UserId; }
            set { SetProperty(ref _UserId, value); }
        }

        private double _Latitude;
        public double Latitude
        {
            get { return _Latitude; }
            set { SetProperty(ref _Latitude, value); }
        }

        private double _Longitude;
        public double Longitude
        {
            get { return _Longitude; }
            set { SetProperty(ref _Longitude, value); }
        }

        private string _Label;
        public string Label
        {
            get { return _Label; }
            set { SetProperty(ref _Label, value); }
        }


        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value); }
        }

        private bool _IsEnabled;
        public bool IsEnabled
        {
            get 
            {
                //if(IsEnabled)
                //{
                //    ImagePath = Constant.ImageEnabled; 
                //}
                //else
                //{
                //    ImagePath = Constant.ImageDisable;
                //}
                return _IsEnabled;
            }
            set { SetProperty(ref _IsEnabled, value); }
        }

        private string _ImagePath;
        public string ImagePath
        {

            get { return _ImagePath; }
            set { SetProperty(ref _ImagePath, value); }
        }


    }
}
