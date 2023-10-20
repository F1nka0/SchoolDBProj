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
        
        public void UpdateGrid() {
            BaseGrid.ItemsSource = Container.context.Database.SqlQuery<Mark>("select * from Mark").ToList();
            BaseGrid.UpdateLayout();
        }
        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            int mark;
            DateTime dateTime = DateTime.Now;
            int index;
            ReportCard rc=null;
            int.TryParse(RCFK2TB.Text, out index);
            if (int.TryParse(MarkTB.Text, out mark) && DateTime.TryParse(DateOfMarkTB.Text, out dateTime)) {
                try {
                    if(RCFK2TB.Text != "Report card (optional)")
                    rc = Container.context.ReportCard.FirstOrDefault(it=>it.ID == index);
                } catch (ArgumentNullException) {
                    return;
                }
                int[] dateParts = DateOfMarkTB.Text.Split(new char[] { ' ', ':', '/' }, StringSplitOptions.RemoveEmptyEntries).Take(6).Select(it => int.Parse(it)).ToArray();
                Container.context.Mark.Add(new Mark() { Mark1=mark,ReportCard=rc,DateOfMark= new DateTime(dateParts[2], dateParts[1], dateParts[0], dateParts[3], dateParts[4], dateParts[5]) });
                Container.context.SaveChanges();
                UpdateGrid();
            }
        }
        private void Update() {//get done 
            try
            {
                Mark mark;
                int markNum;//
                DateTime dateTime = DateTime.Now;//
                int index;
                ReportCard rc = null;
                if ((bool)UDTCB.IsChecked)
                {
                    mark = Container.context.Mark.First(it => it.ID == int.Parse(ID.Text));
                    mark.ReportCardFK2 = int.Parse(RCFK2TB.Text);
                    mark.Mark1 = int.Parse(MarkTB.Text);
                    int.TryParse(RCFK2TB.Text, out index);
                    if (int.TryParse(MarkTB.Text, out markNum) && DateTime.TryParse(DateOfMarkTB.Text, out dateTime))
                    {
                        try
                        {
                            if (RCFK2TB.Text != "Report card (optional)")
                            rc = Container.context.ReportCard.FirstOrDefault(it => it.ID == index);
                        }
                        catch (ArgumentNullException)
                        {
                            return;
                        }
                        int[] dateParts = DateOfMarkTB.Text.Split(new char[] { ' ', ':', '/' }, StringSplitOptions.RemoveEmptyEntries).Take(6).Select(it => int.Parse(it)).ToArray();
                        dateTime = new DateTime(dateParts[2], dateParts[1], dateParts[0], dateParts[3], dateParts[4], dateParts[5]);
                    }
                }
            }
            finally{}
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

//CellEditEnding="BaseGrid_SelectedCellsChanged"

//{
//    try
//    {

//        //log.WriteLine(b);
//        //System.Diagnostics.Debug.WriteLine(mark.DateOfMark);
//        Mark mark = Container.context.Mark.Where(it => it.ID == ((Mark)(((DataGrid)(sender)).CurrentItem)).ID).First();
//        var b = (e.EditingElement.ToString().Substring(e.EditingElement.ToString().IndexOf(' ')));//e.Row);//SelectedIndex
//        int[] dateParts = b.Split(new char[] { ' ', ':', '/' }, StringSplitOptions.RemoveEmptyEntries).Take(6).Select(it => int.Parse(it)).ToArray();
//        mark.DateOfMark = new DateTime(dateParts[2], dateParts[0], dateParts[1], dateParts[3], dateParts[4], dateParts[5]);// = b.DateOfMark;
//        Container.context.SaveChanges();
//    }
//    catch (InvalidOperationException){ 

//    }