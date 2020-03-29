using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    class voc 
    {
        public voc(string w ,short l) 
        {
            this.l = l;
            this.wo = w;
        }
        public string wo { private set; get; }
        public short l { private set; get; }
    }
    public partial class secpage : ContentPage
    {
        public static Stack<int> stacklist = new Stack<int>();
        public static short pretestcount = 0;
        public static string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        public static string path1;
        public static string path2;

        struct RnI
        {
            public RnI(Queue<string> q, Queue<short> i) 
            {
                this.q = q;
                this.i = i;
            }
           public Queue<string> q;
           public Queue<short> i;
        }
        RnI findprefix(string prefix) 
        { 
            Queue<string> result = new Queue<string>();
            Queue<short> index = new Queue<short>();
            int i = 0;
            for (int j = 0; j < VocTypes.Voc.size[VocTypes.Voc.typesclick]; j++) 
            {
                if (i > 30) { break;}
                if (VocTypes.Voc.word[j * 2].StartsWith(prefix)) { result.Enqueue(VocTypes.Voc.word[j * 2]); index.Enqueue(Convert.ToInt16(j)); i++; }
            }            
            return new RnI(result,index);
        }
        public secpage()
        {
            InitializeComponent();
            ToolbarItems.Add(new ToolbarItem("test", null, () => { Navigation.PushAsync(new test()); })) ;
            ToolbarItems.Add(new ToolbarItem("prepare test content", null, () => { Navigation.PushAsync(new pretest()); }));
            //////////////////////////////////////////////////////
            
            path1 = System.IO.Path.Combine(folder, "testlist");
            path2 = System.IO.Path.Combine(folder, "pre_count");

            if (!System.IO.File.Exists(path1))
            {
                System.IO.File.Create(path1).Dispose();                
            }
            if (!System.IO.File.Exists(path2))
            {
                System.IO.File.Create(path2).Dispose();
            }           
            string testlist = File.ReadAllText(path1);
            string path2read = File.ReadAllText(path2);
            if (path2read.Length > 0)
            {
                pretestcount = Convert.ToInt16(path2read);
            }
            string[] buf = testlist.Split(' ');
            stacklist.Clear();
            for (int i = 0; i < buf.Length - 1; i++)
            {
                if(buf[i]!=""){ stacklist.Push(Convert.ToInt16(buf[i])); }
            }
            ////////////////////////////////////////


            ObservableCollection<voc> vocs = new ObservableCollection<voc>() ;
            short n = 20;
            for (short i =0;i<n;i++) 
            {                
                vocs.Add(new voc(VocTypes.Voc.word[i*2],Convert.ToInt16(i*2)));
            } ;
            ListView list = new ListView
            {
                SelectionMode = ListViewSelectionMode.Single,                
                ItemsSource = vocs,
                ItemTemplate = new DataTemplate(() =>
                {
                    Label name1 = new Label();
                    name1.SetBinding(Label.TextProperty , "wo");
                    name1.FontSize = 20;
                    name1.FontAttributes = FontAttributes.Bold; 
                    ViewCell vc = new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(10, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children = { name1 }
                        }
                    };
                    return vc;
                })
            };
            var b=vocs;
            list.ItemSelected += (o, e) => { var item = e.SelectedItem as voc;VocTypes.Voc.vocclick = item.l;var a=vocs;
                Navigation.PushAsync(new thirdpage()); };
            Entry entry = new Entry { Placeholder = " Type what you want to search for " };
            entry.TextChanged += (o,e) => { vocs.Clear(); RnI vs = findprefix(entry.Text) ; while(vs.q.Count!=0) { vocs.Add(new voc (vs.q.Dequeue(),Convert.ToInt16(2*vs.i.Dequeue())));} };
            this.Content = new StackLayout
            {
                Children = { entry,list }
            };
        }      
    }
}

