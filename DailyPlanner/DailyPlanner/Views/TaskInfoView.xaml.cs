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
        private bool _editMode;
        private IList<DateTime> _selectedDates;
        private Task _currentTask;

        public TaskInfoView(bool isEdit, IList<DateTime> currentSelectedDates,Task currentTask) 
        {
            _editMode = isEdit;
            _selectedDates = currentSelectedDates;
            _currentTask = currentTask;

            InitializeComponent();
            initalizeValuesInControls();
        }

        private void initalizeValuesInControls()
        {
            txtTitle.Text = _currentTask.Title;
            txtHourFrom.Text = _currentTask.HourFrom.ToString();
            txtMinuteFrom.Text  = _currentTask.MinuteFrom.ToString();
            txtHourTo.Text = _currentTask.HourTo.ToString(); ;
            txtMinuteTo.Text = _currentTask.MinuteTo.ToString();
            txtDescription.Text = _currentTask.Description;

            if(_currentTask.TaskPriority == TaskPriority.Low)
            {
                radioLow.IsChecked = true;
                radioNormal.IsChecked = false;
                radioHigh.IsChecked = false;
            }
            else if(_currentTask.TaskPriority == TaskPriority.Normal)
            {
                radioLow.IsChecked = false;
                radioNormal.IsChecked = true;
                radioHigh.IsChecked = false;
            }
            else
            {
                radioLow.IsChecked = false;
                radioNormal.IsChecked = false;
                radioHigh.IsChecked = true;
            }

            chboxKaizedMode.IsEnabled = _currentTask.KaizenMode;
            
        }

        private void btnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            Task t;


            if (_editMode)
            {
                t = _currentTask;
            }
            else
            {
                t = new Task();
            }


            t.Title = txtTitle.Text;
            t.HourFrom = Convert.ToInt32(txtHourFrom.Text);
            t.MinuteFrom = Convert.ToInt32(txtMinuteFrom.Text);
            t.HourTo = Convert.ToInt32(txtHourTo.Text);
            t.MinuteTo = Convert.ToInt32(txtMinuteTo.Text);
            t.TaskPriority = mapTaskPriorityFromRadio();
            t.KaizenMode = chboxKaizedMode.IsEnabled;
            t.Description = txtDescription.Text;

            if(_editMode)
            {
                TaskContextSingleton.GetInstance().Context.Tasks.Update(t);
            }
            else
            {
                TaskContextSingleton.GetInstance().Context.Tasks.Add(t);
            }

            TaskContextSingleton.GetInstance().Context.SaveChanges();


            for (int i = 0; i < _selectedDates.Count; i++)
            {
                TaskCollection taskCollection = TaskContextSingleton.GetInstance().Context.TaskCollections.FirstOrDefault(a => a.DateOfTasks.Date == _selectedDates[i].Date);

                if (taskCollection == null)
                {
                    TaskCollection tNew = new TaskCollection();
                    tNew.DateOfTasks = DateTime.Now;
                    tNew.Tasks.Add(t);

                    TaskContextSingleton.GetInstance().Context.TaskCollections.Add(tNew);
                }
                else
                {
                    if(!_editMode)
                    {
                        taskCollection.Tasks.Add(t);
                    }
                }

            }

            TaskContextSingleton.GetInstance().Context.SaveChanges();

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
