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
using SchoolDB;
namespace SchoolDB
{
    /// <summary>
    /// Логика взаимодействия для PupilWin.xaml
    /// </summary>
    public partial class PupilWin : Window
    {
        public PupilWin()
        {
            InitializeComponent();
            BaseGrid.Loaded += (o, e) => { BaseGrid.ItemsSource = Container.context.Pupil.SqlQuery("select * from Pupil").ToList(); };
        }

        private List<Lesson> FindLesson()
        {
            List<Lesson> referencingLesson = null;
            int LessonIDContainer;
            bool hasLesson = int.TryParse(Lesson.Text, out LessonIDContainer);
            referencingLesson = Container.context.Lesson.Where(it => it.ID == LessonIDContainer).ToList();
            return referencingLesson.Count() == 0 ? null : referencingLesson.ToList();
        }
        private List<ReportCard> FindReportCard()
        {
            List<ReportCard> referencingReportCard = null;
            int ReportCardIDContainer;
            bool hasReportCard = int.TryParse(ReportCard.Text, out ReportCardIDContainer);
            referencingReportCard = Container.context.ReportCard.Where(it => it.ID == ReportCardIDContainer).ToList();
            return referencingReportCard.Count() == 0 ? null : referencingReportCard.ToList();
        }
        private void ResetTBColors() {
            Lesson.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            ReportCard.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Grade.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        private void Commit_Click(object sender, RoutedEventArgs e)
        {

            ResetTBColors();
            List<Lesson> referencingLesson = null;
            List<ReportCard> referencingReportCard = null;
            int holder = 0;
            if (int.TryParse(Lesson.Text, out holder))
            {
                referencingLesson = FindLesson();
                if (referencingLesson == null)
                {

                    Lesson.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    return;
                }
            }
            if (int.TryParse(ReportCard.Text, out holder))
            {
                referencingReportCard = FindReportCard();
                if (referencingReportCard == null)
                {

                    ReportCard.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    return;
                }
            }
            if (!int.TryParse(Grade.Text,out holder)||holder<1) { 
                Grade.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                return;
            }
            if ((bool)UpdateCB.IsChecked)
            {
                bool isRecordIdValid = int.TryParse(ID.Text, out holder);
                if (!isRecordIdValid) { ID.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0)); return; }
                Pupil t = Container.context.Pupil.First(it => it.ID == holder);
                t.ReportCard = referencingReportCard;
                t.Lesson = referencingLesson.First();
                t.Grade = int.Parse(Grade.Text);
                t.PupilName = PipilName.Text;
            }
            else if (Lesson.Text == "")
            {
                Container.context.Pupil.Add(new Pupil() { Grade = int.Parse(Grade.Text), PupilName = PipilName.Text, ReportCard = referencingReportCard });
            }
            else if (!int.TryParse(Lesson.Text, out holder) || holder < 1|| Container.context.Lesson.First(it => it.ID == int.Parse(Lesson.Text))==null)
            {
                Lesson.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                return;
            }
            else {
                Container.context.Pupil.Add(new Pupil() { Grade = int.Parse(Grade.Text), PupilName = PipilName.Text, ReportCard = referencingReportCard,Lesson = Container.context.Lesson.First(it=>it.ID==int.Parse(Lesson.Text)) });
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
                Container.context.Pupil.Remove(Container.context.Pupil.First(it => it.ID == ((Pupil)BaseGrid.SelectedItem).ID));
                Container.context.SaveChanges();
                UpdateGrid();
            }
        }

        private void UpdateGrid()
        {
            BaseGrid.ItemsSource = Container.context.Database.SqlQuery<Pupil>("select * from Pupil").ToList();
            BaseGrid.UpdateLayout();
        }

        private void UpdateCB_Unchecked(object sender, RoutedEventArgs e)
        {
            ID.IsEnabled = false;
        }

    }
}
