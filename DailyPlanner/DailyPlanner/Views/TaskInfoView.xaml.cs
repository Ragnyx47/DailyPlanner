using DailyPlanner.Database;
using DailyPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DailyPlanner.Views
{
    /// <summary>
    /// Interaction logic for TaskInfoView.xaml
    /// </summary>
    public partial class TaskInfoView : Window
    {
        private bool editMode;
        public TaskInfoView()
        {
            InitializeComponent();
        }

        public TaskInfoView(bool isEdit) 
        {
            editMode = isEdit;
            InitializeComponent();
        }

        private void btnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Task t = new Task();
            t.Title = txtTitle.Text;
            t.HourFrom = Convert.ToInt32(txtHourFrom.Text);
            t.MinuteFrom = Convert.ToInt32(txtMinuteFrom.Text);
            t.HourTo = Convert.ToInt32(txtHourTo.Text);
            t.MinuteTo = Convert.ToInt32(txtMinuteTo.Text);
            t.TaskPriority = mapTaskPriorityFromRadio();
            t.KaizenMode = chboxKaizedMode.IsEnabled;
            t.Description = txtDescription.Text;


            TaskContextSingleton.GetInstance().Context.Tasks.Add(t);
            TaskContextSingleton.GetInstance().Context.SaveChanges();


            TaskCollection taskCollection = TaskContextSingleton.GetInstance().Context.TaskCollections.FirstOrDefault(a => a.DateOfTasks == DateTime.Now);
            
            if(taskCollection == null)
            {
                TaskCollection tNew = new TaskCollection();
                tNew.DateOfTasks = DateTime.Now;
                tNew.Tasks.Add(t);

                TaskContextSingleton.GetInstance().Context.TaskCollections.Add(tNew);
                TaskContextSingleton.GetInstance().Context.SaveChanges();
            }
            else
            {
                taskCollection.Tasks.Add(t);
                TaskContextSingleton.GetInstance().Context.SaveChanges();
            }

            MessageBox.Show("Poprawnie zapisano");

            this.Close();
            
        }

        private TaskPriority mapTaskPriorityFromRadio()
        {
            if(radioHigh.IsChecked.Value)
            {
                return TaskPriority.High;
            }
            else if (radioNormal.IsChecked.Value)
            {
                return TaskPriority.Normal;
            }
            else
            {
                return TaskPriority.Low;
            }
        }
    }
}
