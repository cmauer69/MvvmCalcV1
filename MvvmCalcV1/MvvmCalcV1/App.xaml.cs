using MvvmCalcV1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MvvmCalcV1
{
    public partial class App : Application
    {
        MvvmCalcV1ViewModel mvvmCalcV1viewModel;

        public App()
        {
            //Instantiate and Initialize the vewModel for the page
            mvvmCalcV1viewModel = new MvvmCalcV1ViewModel();
            mvvmCalcV1viewModel.RestoreState(Current.Properties);
            MainPage = new MvvmCalcV1.MvvmCalcV1Page(mvvmCalcV1viewModel);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
