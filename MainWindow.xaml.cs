using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SchoolDB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public class Container {
        private static MarkWin MarkWin = new MarkWin();
        public static MainWindow mainWindow = new MainWindow();
        public static SchoolDBEntities context = new SchoolDBEntities();
        public static Window markWindow { get { return new MarkWin(); } set { markWindow = value; } }
        public static WindowCollection winCollection = Application.Current.Windows;
    }
    public class TableInfo {
        public TableInfo(string name)
        {

            Name = name;

        }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty TextProperty;
        
        static MainWindow() {
            TextProperty = DependencyProperty.Register(
                    "Text",
                    typeof(Table),
                    typeof(MainWindow)
                    );
        }
        public IEnumerable<Table> Text { get; set; } 
        public MainWindow()
        {
            InitializeComponent();
            grid.Loaded += InitList;
            Tables.SelectionChanged += OnTableSelected;
        }
        private async void InitList(object sender,EventArgs e) {
            IEnumerable<Table> teachers = await Task.Run(() => { return Dispatcher.Invoke<IEnumerable<Table>>(()=> { return Container.context.Database.SqlQuery<Table>("SELECT name FROM sys.tables"); }); });
            Text = teachers.ToList();
            Tables.ItemsSource = Text;
        }
        private async void OnTableSelected(object sender, SelectionChangedEventArgs e) {
            IEnumerable<object> tables = null;
            var b = Dispatcher.Invoke(() => { return ((Table)Tables.SelectedItem).Name; });
            switch (debugText.Text = Dispatcher.Invoke(() => { return Tables.SelectedItem as Table; }).Name.ToString())
            {
                #region DO NOT OPEN UNDER ANY CIRCUMSTANCES
                case "Schedule": tables = await (Task.Run(() => { return Container.context.Database.SqlQuery<Schedule>($"select * from {b}"); })); break;
                case "Lesson": tables = await (Task.Run(() => { return Container.context.Database.SqlQuery<Lesson>($"select * from {b}"); })); break;
                case "Mark": tables = await (Task.Run(() => { return Container.context.Database.SqlQuery<Mark>($"select * from {b}"); })); break;
                case "Palor": tables = await (Task.Run(() => { return Container.context.Database.SqlQuery<Palor>($"select * from {b}"); })); break;
                case "Pupil": tables = await (Task.Run(() => { return Container.context.Database.SqlQuery<Pupil>($"select * from {b}"); })); break;
                case "ReportCard": tables = await (Task.Run(() => { return Container.context.Database.SqlQuery<ReportCard>($"select * from {b}"); })); break;
                case "Teacher": tables = await (Task.Run(() => { return Container.context.Database.SqlQuery<Teacher>($"select * from {b}"); })); break;
                    #endregion
            }

            SelectedData.ItemsSource = tables.ToList();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private async void AlterButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var win in Container.winCollection) {//судя по всему придётся переделать это на switch (sry)
                if (((Window)win).ToString().Contains((sender as Button).Content.ToString())) {
                    //Container.markWindow = (win as Window);
                    Container.markWindow.Show();
                    break;
                }
            }
        }
    }
}