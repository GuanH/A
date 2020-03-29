using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            StackLayout stackLayout = new StackLayout();
            ScrollView scroll = new ScrollView{Content = stackLayout};
            short n = VocTypes.Voc.types;
            Button[] button = new Button[n];
            for (short i = 0; i < n; i++)
            {
                button[i] = new Button
                {
                    Text = VocTypes.Voc.name[i].ToLower()
                };                
                short c = i;
                button[i].Clicked += (o, e) => { VocTypes.Voc.typesclick = c; };
                stackLayout.Children.Add(button[i]);
            }
            button[0].Clicked += (o, e) => { Navigation.PushAsync(new secpage()); };
            scroll.Content = stackLayout;

            Content = scroll;
        }
        
    }
}
