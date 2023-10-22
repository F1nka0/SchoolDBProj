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
using System.Windows.Shapes;

namespace SchoolDB
{
    /// <summary>
    /// Логика взаимодействия для TeacherWin.xaml
    /// </summary>
    public partial class TeacherWin : Window
    {
        public TeacherWin()
        {
            InitializeComponent();
            BaseGrid.Loaded += (o, e) => { BaseGrid.ItemsSource = Container.context.Teacher.SqlQuery("select * from Teacher").ToList(); };
        }
        private List<Schedule> FindSchedule() {
            List<Schedule> referencingSchedule = null;
            int ScheduleIDContainer;
            bool hasSchedule = int.TryParse(ScheduleTB.Text, out ScheduleIDContainer);
            referencingSchedule = Container.context.Schedule.Where(it => it.ID == ScheduleIDContainer).ToList();
            return referencingSchedule.Count()==0?null: referencingSchedule;
        }
        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            List<Schedule> referencingSchedule=null;
            if (ScheduleTB.Text!=""&& ScheduleTB.Text != "Schedule (Optional)") {
                referencingSchedule = FindSchedule();
                if (referencingSchedule == null) {
                    ScheduleTB.Text = "INCORRECT ID";
                    return;
                } 
            }
            if ((bool)UpdateCB.IsChecked)
            {
                int recordId = int.Parse(ID.Text);
                Teacher t = Container.context.Teacher.First(it => it.ID == recordId);
                t.TeacherName = TeacherName.Text;
                t.SubjectName = SubjectName.Text;
                t.Schedule = referencingSchedule;
            }
            else
            {
                Container.context.Teacher.Add(new Teacher() { SubjectName = SubjectName.Text, TeacherName = TeacherName.Text, Schedule = referencingSchedule});
            }
            Container.context.SaveChanges();
            UpdateGrid();
        }

        private void UpdateCB_Checked(object sender, RoutedEventArgs e)
        {
             ID.IsEnabled = true;
        }

        private void BaseGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Container.context.Teacher.Remove(Container.context.Teacher.First(it => it.ID == ((Teacher)BaseGrid.SelectedItem).ID));
                Container.context.SaveChanges();
                UpdateGrid();
            }
        }

        private void UpdateGrid()
        {
            BaseGrid.ItemsSource = Container.context.Database.SqlQuery<Teacher>("select * from Teacher").ToList();
            BaseGrid.UpdateLayout();
        }

        private void UpdateCB_Unchecked(object sender, RoutedEventArgs e)
        {
            ID.IsEnabled = false;
        }
    }
}
