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

namespace SchoolDB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    class Types {

        public static Lesson lesson;
        public static Mark mark;
        public static Palor palor;
        public static Pupil pupil;
        public static ReportCard reportCard;
        public static Schedule schedule;
        public static Teacher teacher;
    }
    public partial class MainWindow : Window
    {
        
        SchoolDBEntities a = null;
        public MainWindow()
        {
            InitializeComponent();
            a = new SchoolDBEntities();
            grid.Loaded += InitList;
            Tables.SelectionChanged += OnTableSelected;
            
        }
        private async void InitList(object sender,EventArgs e) {
            IEnumerable<Table> teachers = await Task.Run(() => { return a.Database.SqlQuery<Table>("SELECT name FROM sys.tables"); });
            Tables.ItemsSource = teachers.Select(it=>it.Name).ToList();

        }
        private async void OnTableSelected(object sender, SelectionChangedEventArgs e) {

            
            IEnumerable<object> tables = null;
            var b = Dispatcher.Invoke(() => { return Tables.SelectedItem; });
            
            switch (debugText.Text = Dispatcher.Invoke(() => { return Tables.SelectedItem; }).ToString())
            {
                
                case "Schedule": tables = await (Task.Run(() => { return a.Database.SqlQuery<Schedule>($"select * from {b}"); })); break;
                case "Lesson": tables = await (Task.Run(() => { return a.Database.SqlQuery<Lesson>($"select * from {b}"); })); break;
                case "Mark": tables = await (Task.Run(() => { return a.Database.SqlQuery<Mark>($"select * from {b}"); })); break;
                case "Palor": tables = await (Task.Run(() => { return a.Database.SqlQuery<Palor>($"select * from {b}"); })); break;
                case "Pupil": tables = await (Task.Run(() => { return a.Database.SqlQuery<Pupil>($"select * from {b}"); })); break;
                case "ReportCard": tables = await (Task.Run(() => { return a.Database.SqlQuery<ReportCard>($"select * from {b}"); })); break;
                case "Teacher": tables = await (Task.Run(() => { return a.Database.SqlQuery<Teacher>($"select * from {b}"); })); break;
            }
            
            SelectedData.ItemsSource = tables.ToList();
            
        }
    }
    public class TypeContainer { 
         
    }
}
/*
            List<Table> selected = (await Task.Run(() => { return a.Database.SqlQuery<Table>($"select * from {e.AddedItems[0].ToString()}"); })).ToList();
            SelectedData.ItemsSource = selected.Select(it=>it).ToList();
 */