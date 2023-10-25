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
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using log = System.Diagnostics.Debug;
namespace SchoolDB
{
    /// <summary>
    /// Логика взаимодействия для MarkWin.xaml
    /// </summary>
    public partial class MarkWin : Window
    {
        public MarkWin()
        {
            InitializeComponent();
            BaseGrid.Loaded += (o, e) => { BaseGrid.ItemsSource = Container.context.Database.SqlQuery<Mark>("select * from Mark").ToList(); };

        }
        private void BaseGrid_SelectedCellsChanged(object sender, DataGridCellEditEndingEventArgs e)
        {

        }
        private void BaseGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) {
                Container.context.Mark.Remove(Container.context.Mark.First(it => it.ID == ((Mark)BaseGrid.SelectedItem).ID));
                Container.context.SaveChanges();
            UpdateGrid();
            }

        }
        private void ResetTBColors()
        {
            UDTCB.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            RCFK2TB.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            MarkTB.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        public void UpdateGrid() {
            BaseGrid.ItemsSource = Container.context.Database.SqlQuery<Mark>("select * from Mark").ToList();
            BaseGrid.UpdateLayout();
        }
        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            ResetTBColors();
            int mark;
            DateTime dateTime = DateTime.Now;
            int index;
            int idHolder;
            ReportCard rc=null;
            if (!int.TryParse(RCFK2TB.Text, out index)&& RCFK2TB.Text!="") { RCFK2TB.Background= new SolidColorBrush(Color.FromRgb(255, 0, 0)); return; }
            if (!int.TryParse(MarkTB.Text, out mark)||mark<1) { 
            MarkTB.Background= new SolidColorBrush(Color.FromRgb(255, 0, 0));
                return;
            }
            if (!DateTime.TryParse(DateOfMarkTB.Text, out dateTime)) { 
                DateOfMarkTB.Background= new SolidColorBrush(Color.FromRgb(255, 0, 0));
                return;
            }
            int[] dateParts = DateOfMarkTB.Text.Split(new char[]{ ' ',':','/' },StringSplitOptions.RemoveEmptyEntries).Take(6).Select(it => int.Parse(it)).ToArray();


            if (!(bool)UDTCB.IsChecked)
            {
                Container.context.Mark.Add(new Mark() { Mark1 = mark, ReportCard = rc, DateOfMark = new DateTime(dateParts[2], dateParts[1], dateParts[0], dateParts[3], dateParts[4], dateParts[5]) });
                
            }
            else {
                int.TryParse(ID.Text, out idHolder);
                Mark mark1 = Container.context.Mark.FirstOrDefault(it => it.ID == idHolder);
                if (mark1 == null) { 
                    ID.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    return;
                }
                mark1.Mark1 = mark;
                mark1.ReportCard= rc;
                mark1.DateOfMark = new DateTime(dateParts[2], dateParts[1], dateParts[0], dateParts[3], dateParts[4], dateParts[5]);
            }
            Container.context.SaveChanges();
            UpdateGrid();
            ResetTBColors();
        }
        private void Update()
        {

        
        }
                
        
        private void UDTCB_Checked(object sender, RoutedEventArgs e)
        {
            ID.IsEnabled= true;
        }

        private void UDTCB_Unchecked(object sender, RoutedEventArgs e)
        {
            ID.IsEnabled = false;
        }
    }
}