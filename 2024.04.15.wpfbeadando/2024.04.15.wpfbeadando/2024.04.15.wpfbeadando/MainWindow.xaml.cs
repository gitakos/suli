using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using MessageBox = System.Windows.MessageBox;
using System.Threading;

namespace _2024._04._15.wpfbeadando
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        kep kepek;
        string ut = "../../../images";
        ICollectionView myView;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            nezet_label.Content = nezetLista[0];
            FileSystemWatcher fsw = new FileSystemWatcher(ut);
            fsw.Created += FswCreated;
            fsw.EnableRaisingEvents = true;
        }

        private void FswCreated(object sender, FileSystemEventArgs e)
        {
            //Ha új képet töltünk be
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                //Diszépcserrel ellenőrzött módon tud a két szál között kommunikálni.
                Dispatcher.Invoke(new ThreadStart(() =>
                {
                    FileInfo fs = new FileInfo(e.FullPath);
                    // Ez külön szálon fut.Ezért ezt nem támogatja. Dispécsert kell használni a két szál között.
                    kepek.Insert(0, fs);
                }));
            }
        }

        public List<string> nezetLista = new List<string>() {"Gyönyörködni akarok","Listát akarok","Táblázatot ide"};
        public int kivalasztottNezet = 0;
        public void nezet_gomb_Click(object sender, RoutedEventArgs e)
        {
            kivalasztottNezet++;
            if (kivalasztottNezet == nezetLista.Count)
            {
                kivalasztottNezet = 0;
            }
            nezet_label.Content = nezetLista[kivalasztottNezet];
            if (nezetLista[kivalasztottNezet]== "Listát akarok")
            {
                //myView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
                hehe.Visibility = Visibility.Hidden;
                lista.Visibility = Visibility.Visible;
                haha.Visibility = Visibility.Hidden;
            }
            if (nezetLista[kivalasztottNezet] == "Gyönyörködni akarok")
            {
                myView.MoveCurrentToPosition(0);
                //szar.View = 
                hehe.Visibility = Visibility.Visible;
                lista.Visibility = Visibility.Hidden;
                haha.Visibility = Visibility.Hidden;
            }
            if (nezetLista[kivalasztottNezet] == "Táblázatot ide")
            {
                hehe.Visibility = Visibility.Hidden;
                lista.Visibility = Visibility.Hidden;
                haha.Visibility = Visibility.Visible;
            }
        }
        int kepekHossza = 0;
        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var files = new DirectoryInfo(ut).GetFiles().Where(c=>c.Extension == ".jpg"|| c.Extension == ".jfif"|| c.Extension == ".png").ToList();
            //MessageBox.Show(files[0].Length+" ");
            kepek = new kep(files);
            this.DataContext = kepek;
            myView = CollectionViewSource.GetDefaultView(kepek);
            kepekHossza = kepek.Count;
            this.DataContext = myView;
        }
        int index = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Elore
            /*index++;
            if(index== kepekHossza)
            {
                index = 0;
            }*/
            if (myView.CurrentPosition != kepekHossza-1)
            {
                myView.MoveCurrentToNext();
                balra.Visibility = Visibility.Visible;
                if (myView.CurrentPosition == kepekHossza-1)
                {
                    jobbra.Visibility = Visibility.Hidden;
                }
            }
            //System.Windows.Forms.MessageBox.Show(myView.CurrentPosition + " "+ kepekHossza);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (myView.CurrentPosition != 0)
            {
                myView.MoveCurrentToPrevious();
                jobbra.Visibility = Visibility.Visible;
                if(myView.CurrentPosition == 0)
                {
                    balra.Visibility = Visibility.Hidden;
                }
            }
            //System.Windows.Forms.MessageBox.Show(myView.CurrentPosition + " ");
            //Hatra
            /*index--;
            if (index == -1)
            {
                index = kepekHossza-1;
            }*/
        }
        int rendInd = 0;
        private void rendezes_Click(object sender, RoutedEventArgs e)
        {
            rendInd++;
            myView.SortDescriptions.Clear();
            if (rendInd == 0)
            {
                myView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                rendezes.Content = "Rend: Név";
            }
            else if (rendInd == 1) {
                myView.SortDescriptions.Add(new SortDescription("LastAccessTime", ListSortDirection.Ascending));
                rendezes.Content = "Rend: Elér";
            }
            else
            {
                myView.SortDescriptions.Add(new SortDescription("CreationTime", ListSortDirection.Ascending));
                rendezes.Content = "Rend: Készült";
                rendInd = -1;
            }
        }
    }
}
