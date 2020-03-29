using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pretest : ContentPage
    {
        public pretest()
        {
            InitializeComponent();
           
                       
            Label name1 = new Label
            {
                Text = VocTypes.Voc.word[2 * secpage.pretestcount],
                FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,                
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 1000
            };
            Label name2 = new Label
            {
                Text = VocTypes.Voc.word[2 * secpage.pretestcount + 1],
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                WidthRequest = 1000,
                IsVisible = false
            };
            Switch @switch = new Switch
            {
                Margin = new Thickness(0, Device.GetNamedSize(NamedSize.Medium, typeof(Label)), 0, Device.GetNamedSize(NamedSize.Medium, typeof(Label))),
                IsToggled = false
            };
            @switch.Toggled += (o, e) => { if (e.Value) { name2.IsVisible = true; } else { name2.IsVisible = false; } };
            Button button1 = new Button
            {
                Text = "I've already known the word!",
                Margin = new Thickness(0, Application.Current.MainPage.Height/2, Application.Current.MainPage.Width / 2, 0),
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 250,
                WidthRequest = 80
            };

            button1.Clicked += (o, e) =>{ secpage.pretestcount++; name1.Text= VocTypes.Voc.word[2 * secpage.pretestcount]; name2.Text = VocTypes.Voc.word[2 * secpage.pretestcount + 1]; };
            Button button2 = new Button
            {
                Text = "I don't understand the word.",
                Margin = new Thickness(Application.Current.MainPage.Width-80, Application.Current.MainPage.Height/2, 0, 0),
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 250,
                WidthRequest = 80
            };
            button2.Clicked += (o, e) => { secpage.pretestcount++; File.AppendAllText(secpage.path1, (2 * secpage.pretestcount).ToString()+' ') ; secpage.stacklist.Push( Convert.ToInt16(2* secpage.pretestcount));name1.Text = VocTypes.Voc.word[2 * secpage.pretestcount]; name2.Text = VocTypes.Voc.word[2 * secpage.pretestcount + 1]; };
            
            AbsoluteLayout.SetLayoutBounds(name1,new Rectangle(55,50,300,300));
            this.Content = new AbsoluteLayout { Children = {button1,button2,name1,name2 ,@switch} };
            this.Disappearing += (o, e) => { File.WriteAllText(secpage.path2, secpage.pretestcount.ToString());};
        }
    }
}