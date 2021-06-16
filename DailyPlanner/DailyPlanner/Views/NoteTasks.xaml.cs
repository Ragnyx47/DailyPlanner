using DailyPlanner.Database;
using DailyPlanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for NoteTasks.xaml
    /// </summary>
    public partial class NoteTasks : Window
    {
        private int currentIndexOfTextBoxEdit = 0;
        private ListBoxItem lastListBoxItem = null;
        private TextBox lastTextBox = null;
        private bool _isEditing;
        
        public NoteTasks()
        {
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var noteTasks = TaskContextSingleton.GetInstance().Context.NoteTasks.ToList();

            foreach (var noteTask in noteTasks)
            {
                ListBoxItem listBoxItem = new ListBoxItem() { Content = noteTask.Title };
                listBoxItem.MouseDoubleClick += ItemTempleateMouseDoubleClick;

                lstNoteTasks.Items.Add(listBoxItem);
            }
            chbRepeatName.IsChecked = true;
        }

        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
           if(txtAddNote.Text == string.Empty )
            {
                MessageBox.Show("Wpisz tekst");
                return;
            }

            ListBoxItem listBoxItem = new ListBoxItem() { Content = txtAddNote.Text };
            listBoxItem.MouseDoubleClick += ItemTempleateMouseDoubleClick;

            lstNoteTasks.Items.Add(listBoxItem);
            TaskContextSingleton.GetInstance().Context.NoteTasks.Add(new NoteTask() { Title = txtAddNote.Text});
            TaskContextSingleton.GetInstance().Context.SaveChanges();

            if(chbRepeatName.IsChecked.Value)
            {
                txtAddNote.Text = string.Empty;
            }
           
        }

        private void btnDeleteNode_Click(object sender, RoutedEventArgs e)
        {
            var selected = lstNoteTasks.SelectedItems;
            bool wasChangesMade = false;

            var ls = new List<ListBoxItem>();

            foreach (ListBoxItem item in selected)
            {
                ls.Add(item);
                string deletedNoteTask = item.Content.ToString();

                //Because there will not be thounsands or even hundrets of those note tasks we can safely use this method
                NoteTask noteTask = TaskContextSingleton.GetInstance().Context.NoteTasks.FirstOrDefault(a => a.Title == deletedNoteTask);
                if (noteTask != null)
                {
                    TaskContextSingleton.GetInstance().Context.NoteTasks.Remove(noteTask);
                    wasChangesMade = true;
                }
            }

            if(wasChangesMade)
            {
                TaskContextSingleton.GetInstance().Context.SaveChanges();
            }

            ls.ForEach(a => lstNoteTasks.Items.Remove(a));

            if(lstNoteTasks.HasItems)
            {
                lstNoteTasks.SelectedItem = lstNoteTasks.Items[0];
            }

            lstNoteTasks.Focus();
        }

        private void btnSelectAllItems_Click(object sender, RoutedEventArgs e)
        {
            TaskContextSingleton.GetInstance().Context.NoteTasks.RemoveRange(TaskContextSingleton.GetInstance().Context.NoteTasks);
            TaskContextSingleton.GetInstance().Context.SaveChanges();
            lstNoteTasks.Items.Clear();

        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int v = lstNoteTasks.Items.IndexOf(firstListItem);
            currentIndexOfTextBoxEdit = v;

            ListBoxItem firstListItemTemp = new ListBoxItem();
            firstListItemTemp.Content = firstListItem.Content;
            lastListBoxItem = firstListItemTemp;


            lstNoteTasks.Items.Remove(firstListItem);

            TextBox textBox = new TextBox() { Text = firstListItemTemp.Content.ToString(),Name="tempTextBox" };
            
            lastTextBox = textBox;

            lstNoteTasks.Items.Insert(v, textBox);
            _isEditing = true;

            string kfdspo = "fds";
        }

        private void ItemTempleateMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int v = lstNoteTasks.Items.IndexOf(firstListItem);
            currentIndexOfTextBoxEdit = v;

            ListBoxItem firstListItemTemp = new ListBoxItem();
            firstListItemTemp.Content = firstListItem.Content;
            lastListBoxItem = firstListItemTemp;


            lstNoteTasks.Items.Remove(firstListItem);

            TextBox textBox = new TextBox() { Text = firstListItemTemp.Content.ToString(), Name = "tempTextBox" };

            lastTextBox = textBox;

            lstNoteTasks.Items.Insert(v, textBox);
            _isEditing = true;

            string kfdspo = "fds";
        }

        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mouseWasDownOn = e.Source as FrameworkElement;
            if (mouseWasDownOn != null)
            {
                string elementName = mouseWasDownOn.Name;
                if(elementName != "tempTextBox" && _isEditing)
                {
                    lstNoteTasks.Items.RemoveAt(currentIndexOfTextBoxEdit);
                    lstNoteTasks.Items.Insert(currentIndexOfTextBoxEdit, lastListBoxItem);
                    _isEditing = false;
                }
            }
        }
    }
}
