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
    public partial class thirdpage : ContentPage
    {
        
        public thirdpage()
        {
            InitializeComponent();
            StackLayout stack = new StackLayout();
            ScrollView scrollView = new ScrollView();
            Label label1 = new Label {
                Text = VocTypes.Voc.word[VocTypes.Voc.vocclick],
                FontSize = 35,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Start,
                FontAttributes = FontAttributes.Bold
            };
               Label label2 = new Label
            {
                   Text = VocTypes.Voc.word[VocTypes.Voc.vocclick+1],
                   FontSize = 23,
                   HorizontalOptions = LayoutOptions.StartAndExpand,
                   VerticalOptions = LayoutOptions.Start,
                   FontAttributes = FontAttributes.Bold
            };
            Label label3 = new Label
            {
                Text ="\n\n" + eg.sen[VocTypes.Voc.vocclick] +"\n"+ eg.sen[VocTypes.Voc.vocclick+1],
                FontSize = 20,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                FontAttributes = FontAttributes.Bold
            };
            Switch Switcher = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            string str0 = File.ReadAllText(secpage.path1);
            if (str0.Contains((2 * (VocTypes.Voc.vocclick+1)).ToString() + ' '))
            { Switcher.IsToggled = true; }            
            else { Switcher.IsToggled = false; Console.WriteLine((2 * VocTypes.Voc.vocclick).ToString()+'\n'+'\n'); };
            Switcher.Toggled += (s, e) => {
                if (e.Value) { File.AppendAllText(secpage.path1, (2 * (VocTypes.Voc.vocclick + 1)).ToString() + ' '); secpage.stacklist.Push(2 * (VocTypes.Voc.vocclick + 1)); }
                else {string str = File.ReadAllText(secpage.path1); File.WriteAllText(secpage.path1,str.Replace((2 * (VocTypes.Voc.vocclick + 1)).ToString(),string.Empty));};  
            };
            stack.Children.Add(label1);
            stack.Children.Add(label2);
            stack.Children.Add(label3);
            if (secpage.pretestcount > VocTypes.Voc.vocclick) { stack.Children.Add(Switcher); };
            scrollView.Content = stack;
            Content = scrollView;
        }
    }
}