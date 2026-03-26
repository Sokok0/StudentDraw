using System;
using System.Linq;
using Microsoft.Maui.Controls;
using StudentDraw.Models;

namespace StudentDraw.Views
{
    public partial class ManageClassesPage : ContentPage
    {
        private DataService _dataService = new DataService();
        private SchoolClass _currentClass;

        public ManageClassesPage()
        {
            InitializeComponent();
        }

        private void OnSelectClassClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ClassNameEntry.Text))
                return;

            string className = ClassNameEntry.Text.Trim();
            var classes = _dataService.GetAllClasses();

            _currentClass = classes.FirstOrDefault(c => c.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase));

            if (_currentClass == null)
            {
                _currentClass = new SchoolClass { ClassName = className };
            }

            CurrentClassLabel.Text = $"Edycja klasy: {_currentClass.ClassName}";
            StudentsEditor.Text = string.Join(Environment.NewLine, _currentClass.Students.Select(s => s.Name));
            StudentsEditor.IsVisible = true;
            SaveButton.IsVisible = true;
        }

        private void OnDeleteClassClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ClassNameEntry.Text))
                return;

            string className = ClassNameEntry.Text.Trim();
            _dataService.DeleteClass(className);
            ClassNameEntry.Text = string.Empty;
            CurrentClassLabel.Text = "Brak wybranej klasy";
            StudentsEditor.Text = string.Empty;
            StudentsEditor.IsVisible = false;
            SaveButton.IsVisible = false;
            _currentClass = null;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (_currentClass == null)
                return;

            _currentClass.Students.Clear();
            var lines = StudentsEditor.Text?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    _currentClass.Students.Add(new Student { Name = line.Trim() });
                }
            }

            _dataService.SaveClass(_currentClass);
            await DisplayAlert("Sukces", "Lista uczniów została zapisana", "OK");
        }
    }
}