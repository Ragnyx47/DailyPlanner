using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyPlanner.Database
{
    public class TaskContextSingleton
    {
        private TaskContextSingleton() { }

        private static TaskContextSingleton _instance;

        public TaskContext Context { get; set; }

        public static TaskContextSingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TaskContextSingleton();
            }
            return _instance;
        }

        private bool initalized = false;

        public void initalizeContext()
        {
            if(!initalized)
            {
                Context = new TaskContext();

                Context.Database.EnsureCreated();

                Context.TaskCollections.Load();
                initalized = true;
            }
        }

    }
}
