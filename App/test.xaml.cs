using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Medallion;
namespace App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]    
    public partial class test : ContentPage
    {
        public test()
        {
            InitializeComponent();
            Random random = new Random(Rand.NextInt32(Rand.Current));            
            List<int> ex= new List<int>(secpage.stacklist.ToArray());
            if (ex.Count < 2)
            {
                Label label = new Label 
                {
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    Text ="\"Prepare test content\" needs at least 2 words.",                    
                };


                Button exit = new Button 
                {
                    Text = "OK!",
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    Margin = new Thickness(Application.Current.MainPage.Width / 3, Application.Current.MainPage.Height * 3 / 10, Application.Current.MainPage.Width *2/ 3, 0),
                    HeightRequest = 60,
                    WidthRequest = 100
                };
                exit.Clicked += (o,e) => { Navigation.PopAsync();};
                AbsoluteLayout.SetLayoutBounds(label, new Rectangle(0, Application.Current.MainPage.Height/6, 300, 300));
                this.Content = new AbsoluteLayout { Children = {label, exit } };
            }
            else
            {
                Queue<int> mis = new Queue<int>();
                Rand.Shuffle(ex, random);
                short testcount = 0;
                int whichans = random.Next(0, 3);
                int correct = 0;
                int wrong = 0;
                Label wronglist = new Label
                {
                    Text = "incorrect answer :\n",
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    Margin = new Thickness(Application.Current.MainPage.Width / 3, Application.Current.MainPage.Height / 4, Application.Current.MainPage.Width / 3, 0),
                    IsVisible = false
                };
                Button okbutton = new Button()
                {
                    Text = "ok",                   
                    IsVisible=false
                };
                okbutton.Clicked += (o, e) => { Navigation.PopAsync(); };
               
                Label topic = new Label
                {
                    Text = VocTypes.Voc.word[ex[testcount]],
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 1.5
                };
                Label correctl = new Label
                {
                    Text = "Correct :" + correct.ToString(),
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                };
                Label wrongl = new Label
                {
                    Text = "Wrong :" + wrong.ToString(),
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                };
              

                Button[] ans = new Button[4];
                for (int i = 0; i < 4; i++)
                {
                    ans[i] = new Button()
                    {
                        Margin = new Thickness(0, Application.Current.MainPage.Height / 7.5 * (i + 2), Application.Current.MainPage.Width, 0),
                        HorizontalOptions = LayoutOptions.Center,
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        HeightRequest = Application.Current.MainPage.Height / 10,
                        WidthRequest = 400
                    };
                }

                int current = ex[testcount];
                for (int i = 0; i < 4; i++)
                {
                    if (i == whichans)
                    { ans[i].Text = VocTypes.Voc.word[current + 1]; }
                    else
                    {
                        int ran = random.Next(0, VocTypes.Voc.size[VocTypes.Voc.typesclick]);
                        if (ran * 2 == ex[testcount]) { ran += 2; };
                        ans[i].Text = VocTypes.Voc.word[ran * 2 + 1];
                    }
                }
                Label ml = new Label()
                {
                    Text = VocTypes.Voc.word[ex[testcount]] + ":" + VocTypes.Voc.word[current + 1],
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    IsVisible = false
                };
                Button mb = new Button()
                {
                    Text = "OK!",
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    HeightRequest = 60,
                    WidthRequest = 100,
                    IsVisible =false
                };
                AbsoluteLayout.SetLayoutBounds(ml, new Rectangle(Application.Current.MainPage.Width / 5, Application.Current.MainPage.Height / 3.5, 300, 300));
                AbsoluteLayout.SetLayoutBounds(mb, new Rectangle(Application.Current.MainPage.Width / 2.5, Application.Current.MainPage.Height *2/3, 100, 60));
              

                Label sum_l = new Label { Text = "",
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
               };
                
                mb.Clicked += (o, e) => { for (int i = 0; i < 4; i++) { ans[i].IsEnabled = true; }; ml.IsVisible = false; mb.IsVisible = false; };

                ans[0].Clicked += (o, e) =>
                {
                    if (whichans == 0) { correct += 1; } else { ml.Text = VocTypes.Voc.word[ex[testcount]] + ":" + VocTypes.Voc.word[ex[testcount] + 1]; for (int k = 0; k < 4; k++) { ans[k].IsEnabled = false; }; ml.IsVisible = true;mb.IsVisible = true; wrong += 1; mis.Enqueue(testcount); };
                    whichans = random.Next(0, 3);
                    testcount += 1;
                    current = ex[testcount];
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == whichans)
                        { ans[i].Text = VocTypes.Voc.word[current + 1]; }
                        else
                        {
                            int ran = random.Next(0, VocTypes.Voc.size[VocTypes.Voc.typesclick]);
                            if (ran * 2 == ex[testcount]) { ran += 2; };
                            ans[i].Text = VocTypes.Voc.word[ran * 2 + 1];
                        }
                    }
                    topic.Text = VocTypes.Voc.word[ex[testcount]];
                    correctl.Text = "Correct :" + correct.ToString();
                    wrongl.Text = "Wrong :" + wrong.ToString();
                    if (testcount + 1 == ex.Count) {
                        sum_l.Text += "Score : " + (correct * 100 / (correct + wrong)).ToString() + "\nincorrect:\n";
                        for (int i = 0; i < mis.Count; i++)
                        {
                            sum_l.Text += (VocTypes.Voc.word[ex[mis.Dequeue()]] + " : " + VocTypes.Voc.word[ex[mis.Dequeue()] + 1]) + '\n';
                        }; ex.Clear(); for (int i = 0; i < 4; i++) { ans[i].IsVisible = false; };topic.IsVisible = false;correctl.IsVisible = false;wrongl.IsVisible = false;wronglist.IsVisible = false; ml.IsVisible = false; mb.IsVisible = false; okbutton.IsVisible = true;
                    }
                };

                ans[1].Clicked += (o, e) =>
                {
                    if (whichans == 1) { correct += 1; } else { ml.Text = VocTypes.Voc.word[ex[testcount]] + ":" + VocTypes.Voc.word[ex[testcount] + 1]; for (int k = 0; k < 4; k++) { ans[k].IsEnabled = false; }; ml.IsVisible = true;mb.IsVisible = true; wrong += 1; mis.Enqueue(testcount); };
                    whichans = random.Next(0, 3);
                    testcount += 1;
                    current = ex[testcount];
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == whichans)
                        { ans[i].Text = VocTypes.Voc.word[current + 1]; }
                        else
                        {

                            int ran = random.Next(0, VocTypes.Voc.size[VocTypes.Voc.typesclick]);
                            if (ran * 2 == ex[testcount]) { ran += 1; };
                            ans[i].Text = VocTypes.Voc.word[ran * 2 + 1];
                        }
                    }
                    topic.Text = VocTypes.Voc.word[ex[testcount]];
                    correctl.Text = "Correct :" + correct.ToString();
                    wrongl.Text = "Wrong :" + wrong.ToString();
                    if (testcount + 1 == ex.Count)
                    {
                        sum_l.Text += "Score : " + (correct * 100 / (correct + wrong)).ToString() + "\nincorrect:\n";
                        for (int i = 0; i < mis.Count; i++)
                        {
                            sum_l.Text += (VocTypes.Voc.word[ex[mis.Dequeue()]] + " : " + VocTypes.Voc.word[ex[mis.Dequeue()] + 1]) + '\n';
                        }; ex.Clear();  for (int i = 0; i < 4; i++) { ans[i].IsVisible = false; }; topic.IsVisible = false; correctl.IsVisible = false; wrongl.IsVisible = false; wronglist.IsVisible = false; ml.IsVisible = false; mb.IsVisible = false;okbutton.IsVisible = true;
                    }
                };

                ans[2].Clicked += (o, e) =>
                {
                    if (whichans == 2) { correct += 1; } else { ml.Text = VocTypes.Voc.word[ex[testcount]] + ":" + VocTypes.Voc.word[ex[testcount] + 1]; for (int k = 0; k < 4; k++) { ans[k].IsEnabled = false; }; ml.IsVisible = true;mb.IsVisible = true; wrong += 1; mis.Enqueue(testcount); };
                    whichans = random.Next(0, 3);
                    testcount += 1;
                    current = ex[testcount];
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == whichans)
                        { ans[i].Text = VocTypes.Voc.word[current + 1]; }
                        else
                        {
                            int ran = random.Next(0, VocTypes.Voc.size[VocTypes.Voc.typesclick]);
                            if (ran * 2 == ex[testcount]) { ran += 1; };
                            ans[i].Text = VocTypes.Voc.word[ran * 2 + 1];
                        }
                    }
                    topic.Text = VocTypes.Voc.word[ex[testcount]];
                    correctl.Text = "Correct :" + correct.ToString();
                    wrongl.Text = "Wrong :" + wrong.ToString();
                    if (testcount + 1 == ex.Count) {
                        sum_l.Text += "Score : " + (correct * 100 / (correct + wrong)).ToString() + "\nincorrect:\n";
                        for (int i = 0; i < mis.Count; i++)
                        {
                            sum_l.Text += (VocTypes.Voc.word[ex[mis.Dequeue()]] + " : " + VocTypes.Voc.word[ex[mis.Dequeue()] + 1]) + '\n';
                        }; ex.Clear();  for (int i = 0; i < 4; i++) { ans[i].IsVisible = false; }; topic.IsVisible = false; correctl.IsVisible = false; wrongl.IsVisible = false; wronglist.IsVisible = false; ml.IsVisible = false; mb.IsVisible = false; okbutton.IsVisible = true;
                    }
                };

                ans[3].Clicked += (o, e) =>
                {
                    if (whichans == 3) { correct += 1; } else { ml.Text = VocTypes.Voc.word[ex[testcount]] + ":" + VocTypes.Voc.word[ex[testcount] + 1]; for (int k = 0; k < 4; k++) { ans[k].IsEnabled = false; }; ml.IsVisible = true;mb.IsVisible = true; wrong += 1; mis.Enqueue(testcount); };
                    whichans = random.Next(0, 3);
                    testcount += 1;
                    current = ex[testcount];
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == whichans)
                        { ans[i].Text = VocTypes.Voc.word[current + 1]; }
                        else
                        {

                            int ran = random.Next(0, VocTypes.Voc.size[VocTypes.Voc.typesclick]);
                            if (ran * 2 == ex[testcount]) { ran += 1; };
                            ans[i].Text = VocTypes.Voc.word[ran * 2 + 1];
                        }
                    }
                    topic.Text = VocTypes.Voc.word[ex[testcount]];
                    correctl.Text = "Correct :" + correct.ToString();
                    wrongl.Text = "Wrong :" + wrong.ToString();
                    if (testcount + 1 == ex.Count) {
                        sum_l.Text += "Score : " + (correct*100 / (correct + wrong)).ToString() + "\nincorrect:\n";
                        for (int i = 0; i < mis.Count; i++)
                        {
                            sum_l.Text += (VocTypes.Voc.word[ex[mis.Dequeue()]] + " : " + VocTypes.Voc.word[ex[mis.Dequeue()] + 1]) + '\n';
                        }; ex.Clear();  for (int i = 0; i < 4; i++) { ans[i].IsVisible = false; }; topic.IsVisible = false; correctl.IsVisible = false; wrongl.IsVisible = false; wronglist.IsVisible = false; ml.IsVisible = false; mb.IsVisible = false; okbutton.IsVisible = true;
                    }
                };







                AbsoluteLayout.SetLayoutBounds(topic, new Rectangle(Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 4, Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 3, 300, 300));
                AbsoluteLayout.SetLayoutBounds(correctl, new Rectangle(0, Device.GetNamedSize(NamedSize.Large, typeof(Label)) * 2, 300, 300));
                AbsoluteLayout.SetLayoutBounds(wrongl, new Rectangle(0, Device.GetNamedSize(NamedSize.Large, typeof(Label)), 300, 300));


               
                ScrollView sum = new ScrollView
                {
                    Orientation = ScrollOrientation.Vertical,
                    Content = new AbsoluteLayout { Children = { sum_l,topic, correctl, wrongl, ans[0], ans[1], ans[2], ans[3], wronglist,ml,mb} }
                };

                this.Content = new StackLayout { Children = { sum , okbutton} };
            }
        }
        
    }
}