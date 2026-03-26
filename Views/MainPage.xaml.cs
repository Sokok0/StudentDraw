using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using StudentDraw.Models;

namespace StudentDraw.Views
{
    public partial class MainPage : ContentPage
    {
        private DataService _dataService = new DataService();
        private List<SchoolClass> _classes;
        private Random _random = new Random();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadClasses();
        }

        private void LoadClasses()
        {
            _classes = _dataService.GetAllClasses();
            ClassPicker.ItemsSource = _classes.Select(c => c.ClassName).ToList();
        }

        private void OnClassPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            DrawnPersonLabel.Text = "???";
        }

        private async void OnDrawClicked(object sender, EventArgs e)
        {
            if (ClassPicker.SelectedIndex < 0)
            {
                await DisplayAlert("Błąd", "Wybierz klasę z listy.", "OK");
                return;
            }

            var selectedClassName = ClassPicker.SelectedItem.ToString();
            var selectedClass = _classes.FirstOrDefault(c => c.ClassName == selectedClassName);

            if (selectedClass != null && selectedClass.Students.Count > 0)
            {
                var index = _random.Next(selectedClass.Students.Count);
                DrawnPersonLabel.Text = selectedClass.Students[index].Name;
            }
            else
            {
                await DisplayAlert("Błąd", "Wybrana klasa nie ma na liście żadnych uczniów.", "OK");
            }
        }
    }
}
