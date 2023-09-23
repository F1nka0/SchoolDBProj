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
            BaseGrid.Loaded += (o,e)=> { BaseGrid.ItemsSource = Container.context.Database.SqlQuery<Mark>("select * from Mark").ToList(); };
            
        }

        private void BaseGrid_SelectedCellsChanged(object sender, DataGridCellEditEndingEventArgs e)
        {
            Mark mark = Container.context.Mark.Where(it => it.ID == 1).First();
            System.Diagnostics.Debug.WriteLine(mark.DateOfMark);
            var b=(e.EditingElement.ToString().Substring(e.EditingElement.ToString().IndexOf(' ')));//e.Row);//SelectedIndex
            log.WriteLine(b);
            int[] dateParts = b.Split(new char[] { ' ', ':', '/' },StringSplitOptions.RemoveEmptyEntries).Take(6).Select(it => int.Parse(it)).ToArray();
            mark.DateOfMark = new DateTime(dateParts[2], dateParts[0], dateParts[1], dateParts[3], dateParts[4], dateParts[5]);// = b.DateOfMark;
            Container.context.SaveChanges();
        }

        private void BaseGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) {
                Container.context.Mark.Remove(Container.context.Mark.First(it=>it.ID== BaseGrid.SelectedIndex));
            }
            Container.context.SaveChanges();
        }
    }
}
