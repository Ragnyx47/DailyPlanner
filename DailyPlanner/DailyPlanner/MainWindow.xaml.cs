using DailyPlanner.Database;
using DailyPlanner.Models;
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
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Collections.IList addedItems = e.AddedItems;
            System.Collections.IList removedItems = e.RemovedItems;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TaskContextSingleton.GetInstance().initalizeContext();

            TaskCollection taskCollection = TaskContextSingleton.GetInstance().Context.TaskCollections.FirstOrDefault(a => a.DateOfTasks.Date == DateTime.Now.Date);

            taskCollection.Tasks.ToList().ForEach(a =>
            {
                TaskView g = new TaskView();
                Frame fr = new Frame();
                fr.Content = g;
                panelForTasks.Children.Add(fr);
            });
            

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // clean up database connections
            TaskContextSingleton.GetInstance().Context.Dispose();
            base.OnClosing(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task t = new Task();
            t.Title = "Zadanie1";
            t.Description = "Desc1";
            t.Hour = 10;
            t.Minute = 20;
            t.KaizenMode = false;
            t.TaskPriority = TaskPriority.Normal;

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
        }

        private void calendar_Loaded(object sender, RoutedEventArgs e)
        {
          
        }

    }
}
