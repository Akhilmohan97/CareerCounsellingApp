using CareerCounsellingApp.Data;
using CareerCounsellingApp.Helpers;
using CareerCounsellingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CareerCounsellingApp.ViewModels
{
    public class StudentManagementViewModel : INotifyPropertyChanged
    {
        private string _admissionNo = "";
        private string _fullName = "";
        private string _gender = "";
        private string _email = "";
        private string _course = "";
        private string _institution = "";
        private string _username = "";
        private string _password = "";
        private string _photoPath = "";

        private Student? _selectedStudent;

        public string AdmissionNo
        {
            get => _admissionNo;
            set { _admissionNo = value; OnPropertyChanged(nameof(AdmissionNo)); }
        }

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }

        public string Gender
        {
            get => _gender;
            set { _gender = value; OnPropertyChanged(nameof(Gender)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string Course
        {
            get => _course;
            set { _course = value; OnPropertyChanged(nameof(Course)); }
        }

        public string Institution
        {
            get => _institution;
            set { _institution = value; OnPropertyChanged(nameof(Institution)); }
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string PhotoPath
        {
            get => _photoPath;
            set { _photoPath = value; OnPropertyChanged(nameof(PhotoPath)); }
        }

        public Student? SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;

                if (value != null)
                {
                    AdmissionNo = value.AdmissionNo;
                    FullName = value.FullName;
                    Gender = value.Gender;
                    Email = value.Email;
                    Course = value.Course;
                    Institution = value.Institution;
                    PhotoPath = value.PhotoPath;
                }

                OnPropertyChanged(nameof(SelectedStudent));
            }
        }

        public ObservableCollection<Student> Students { get; }
            = new();

        public ICommand AddStudentCommand { get; }
        public ICommand UpdateStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }

        public StudentManagementViewModel()
        {
            AddStudentCommand = new RelayCommand(AddStudent);
            UpdateStudentCommand = new RelayCommand(UpdateStudent);
            DeleteStudentCommand = new RelayCommand(DeleteStudent);

            LoadStudents();
        }

        private void LoadStudents()
        {
            using var db = new AppDbContext();

            Students.Clear();

            foreach (var student in db.Students)
            {
                Students.Add(student);
            }
        }

        private void AddStudent()
        {
            if (string.IsNullOrWhiteSpace(FullName))
                return;

            using var db = new AppDbContext();

            var user = new User
            {
                Username = Username,
                Password = Password,
                Role = "Student"
            };

            db.Users.Add(user);
            db.SaveChanges();

            var student = new Student
            {
                UserId = user.Id,
                AdmissionNo = AdmissionNo,
                FullName = FullName,
                Gender = Gender,
                Email = Email,
                Course = Course,
                Institution = Institution,
                PhotoPath = PhotoPath
            };

            db.Students.Add(student);
            db.SaveChanges();

            ClearForm();
            LoadStudents();
        }

        private void UpdateStudent()
        {
            if (SelectedStudent == null)
                return;

            using var db = new AppDbContext();

            var student = db.Students
                .FirstOrDefault(x => x.Id == SelectedStudent.Id);

            if (student == null)
                return;

            student.AdmissionNo = AdmissionNo;
            student.FullName = FullName;
            student.Gender = Gender;
            student.Email = Email;
            student.Course = Course;
            student.Institution = Institution;
            student.PhotoPath = PhotoPath;

            db.SaveChanges();

            LoadStudents();
        }

        private void DeleteStudent()
        {
            if (SelectedStudent == null)
                return;

            using var db = new AppDbContext();

            var student = db.Students
                .FirstOrDefault(x => x.Id == SelectedStudent.Id);

            if (student == null)
                return;

            db.Students.Remove(student);
            db.SaveChanges();

            ClearForm();
            LoadStudents();
        }

        private void ClearForm()
        {
            AdmissionNo = "";
            FullName = "";
            Gender = "";
            Email = "";
            Course = "";
            Institution = "";
            Username = "";
            Password = "";
            PhotoPath = "";

            SelectedStudent = null;

            OnPropertyChanged(nameof(AdmissionNo));
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Gender));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Course));
            OnPropertyChanged(nameof(Institution));
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(PhotoPath));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
