using System;
using System.Collections.Generic;
using System.IO;

namespace StudentDraw.Models
{
    public class DataService
    {
        private static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public List<SchoolClass> GetAllClasses()
        {
            var classes = new List<SchoolClass>();

            if (!Directory.Exists(AppDataPath))
                Directory.CreateDirectory(AppDataPath);

            var files = Directory.GetFiles(AppDataPath, "*.txt");
            foreach (var file in files)
            {
                var className = Path.GetFileNameWithoutExtension(file);
                var lines = File.ReadAllLines(file);
                var schoolClass = new SchoolClass { ClassName = className };

                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        schoolClass.Students.Add(new Student { Name = line.Trim() });
                    }
                }

                classes.Add(schoolClass);
            }

            return classes;
        }

        public void SaveClass(SchoolClass schoolClass)
        {
            var path = Path.Combine(AppDataPath, $"{schoolClass.ClassName}.txt");
            var lines = new List<string>();

            foreach (var student in schoolClass.Students)
            {
                lines.Add(student.Name);
            }

            File.WriteAllLines(path, lines);
        }

        public void DeleteClass(string className)
        {
            var path = Path.Combine(AppDataPath, $"{className}.txt");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}