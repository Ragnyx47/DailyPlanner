using DailyPlanner.Database;
using DailyPlanner.Models;
using DailyPlanner.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DailyPlanner
{
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : Page
    {
        private Task _currentTask;
        private MainWindow _mainWindow;

        public TaskView(Task task,MainWindow mainWindow)
        {
            InitializeComponent();


            _mainWindow = mainWindow;
            _currentTask = task;

            lbltitle.Content = task.Title;
            lbltimeFrom.Content = task.HourFrom + ":" + task.MinuteFrom;
            lbltimeTo.Content = task.HourTo + ":" + task.MinuteTo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IList<DateTime> selectedDates = new List<DateTime>()
            {
                _currentTask.TaskCollection.DateOfTasks
            };


            TaskInfoView taskInfoView = new TaskInfoView(true, selectedDates, _currentTask );
            taskInfoView.ShowDialog();
            _mainWindow.UpdateCalendar();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_currentTask.Description);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TaskContextSingleton.GetInstance().Context.Tasks.Remove(_currentTask);
            TaskContextSingleton.GetInstance().Context.SaveChanges();
            _mainWindow.UpdateCalendar();
        }
    }
}
