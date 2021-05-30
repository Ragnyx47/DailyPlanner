using DailyPlanner.Database;
using DailyPlanner.Models;
using DailyPlanner.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IList<DateTime> currentSelectedDates;

        public MainWindow()
        {
            InitializeComponent();
            currentSelectedDates = new List<DateTime>()
            {
               DateTime.Now
            };

        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Collections.IList addedItems = e.AddedItems;
            IList<DateTime> selectedDates = new List<DateTime>();

            for (int i = 0; i < addedItems.Count; i++)
            {
                DateTime dt = (DateTime)addedItems[i];
                selectedDates.Add(dt);
            }
            currentSelectedDates = selectedDates;
            UpdateCalendar();

            // System.Collections.IList removedItems = e.RemovedItems;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            TaskContextSingleton.GetInstance().initalizeContext();

            UpdateCalendar();



        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // clean up database connections
            TaskContextSingleton.GetInstance().Context.Dispose();
            base.OnClosing(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TaskInfoView taskInfoView = new TaskInfoView(false, currentSelectedDates, prepareEmptyTask());
            taskInfoView.ShowDialog();
            UpdateCalendar();
        }

        private void calendar_Loaded(object sender, RoutedEventArgs e)
        {
          
        }

        private Task prepareEmptyTask()
        {
            Task tK = new Task()
            {
                Title = string.Empty,
                Description = string.Empty,
                HourFrom = 0,
                HourTo = 0,
                KaizenMode = false,
                MinuteFrom = 0,
                MinuteTo = 0,
                TaskPriority = TaskPriority.Normal
            };
            return tK;
        }

        public void UpdateCalendar()
        {
            panelForTasks.Children.Clear();
            bool wasAnyItemsAdded = false;

            for (int i = 0; i < currentSelectedDates.Count; i++)
            {
                TaskCollection taskCollection = TaskContextSingleton.GetInstance().Context.TaskCollections.FirstOrDefault(a => a.DateOfTasks.Date == currentSelectedDates[i].Date);


                if (taskCollection != null)
                {
                    taskCollection.Tasks.ToList().ForEach(a =>
                    {
                        wasAnyItemsAdded = true;
                        TaskView g = new TaskView(a,this);
                        Frame fr = new Frame();
                        fr.Content = g;
                        panelForTasks.Children.Add(fr);
                    });

                }
            }

            if(!wasAnyItemsAdded)
            {
                lblNoTasksInfo.Visibility = Visibility.Visible;
                copyTasksGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                lblNoTasksInfo.Visibility = Visibility.Hidden;
                copyTasksGrid.Visibility = Visibility.Visible;
                currentDatePicker.SelectedDate = currentSelectedDates.ElementAt(0).Date.AddDays(1);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = currentDatePicker.SelectedDate;
            IList<Task> listOfTasks = new List<Task>();


            for (int i = 0; i < currentSelectedDates.Count; i++)
            {
                TaskCollection taskCollection = TaskContextSingleton.GetInstance().Context.TaskCollections.FirstOrDefault(a => a.DateOfTasks.Date == currentSelectedDates[i].Date);


                if (taskCollection != null)
                {
                    taskCollection.Tasks.ToList().ForEach(a =>
                    {
                        Task task = prepareNewTask(a);

                        listOfTasks.Add(task);
                    });

                }
            }


            TaskCollection taskCollectionToAddTasks = TaskContextSingleton.GetInstance().Context.TaskCollections.FirstOrDefault(a => a.DateOfTasks.Date == selectedDate);
            if (taskCollectionToAddTasks == null)
            {
                taskCollectionToAddTasks = new TaskCollection();
                taskCollectionToAddTasks.DateOfTasks = (DateTime)selectedDate;

                for (int i = 0; i < listOfTasks.Count; i++)
                {
                    taskCollectionToAddTasks.Tasks.Add(listOfTasks[i]);

                }
                TaskContextSingleton.GetInstance().Context.TaskCollections.Add(taskCollectionToAddTasks);
            }
            else
            {
                for (int i = 0; i < listOfTasks.Count; i++)
                {
                    taskCollectionToAddTasks.Tasks.Add(listOfTasks[i]);

                }
                TaskContextSingleton.GetInstance().Context.TaskCollections.Update(taskCollectionToAddTasks);
            }

            TaskContextSingleton.GetInstance().Context.SaveChanges();

            MessageBox.Show("Poprawnie przekopiowano");
        }

        private Task prepareNewTask(Task taskFromDb)
        {
            return new Task()
            {
                Title = taskFromDb.Title,
                Description = taskFromDb.Description,
                HourFrom = taskFromDb.HourFrom,
                HourTo = taskFromDb.HourTo,
                KaizenMode = taskFromDb.KaizenMode,
                MinuteFrom = taskFromDb.MinuteFrom,
                MinuteTo = taskFromDb.MinuteTo,
                TaskPriority = taskFromDb.TaskPriority
            };
        }
    }
}
