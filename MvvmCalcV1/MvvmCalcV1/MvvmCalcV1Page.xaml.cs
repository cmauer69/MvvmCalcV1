using MvvmCalcV1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmCalcV1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MvvmCalcV1Page : ContentPage
    {
        public MvvmCalcV1Page(MvvmCalcV1ViewModel viewModel)
        {
            InitializeComponent();
            //Set ViewModel as BindingContent
            BindingContext = viewModel;
        }

        void OnPageSizeChanged(object sender, EventArgs args)
        { // Portrait mode. 
            if (Width < Height)
            { mainGrid.RowDefinitions[1].Height = GridLength.Auto;
              mainGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Absolute);
              Grid.SetRow(keypadGrid, 1);
              Grid.SetColumn(keypadGrid, 0);
            }
            // Landscape mode. 
            else
            { mainGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Absolute);
              mainGrid.ColumnDefinitions[1].Width = GridLength.Auto;
              Grid.SetRow(keypadGrid, 0);
              Grid.SetColumn(keypadGrid, 1);
            }
        }
    }
}
