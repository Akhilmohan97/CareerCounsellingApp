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
    public class CategoryManagementViewModel : INotifyPropertyChanged
    {
        private string _categoryName = "";
        private string _categoryDescription = "";
        private Category? _selectedCategory;
        public ObservableCollection<ParentCategory> ParentCat { get; set; }
        = new();
        private ParentCategory? _selectedParentCategory;
        public ParentCategory? SelectedParentCat
        {
            get => _selectedParentCategory;
            set
            {
                _selectedParentCategory = value;
                OnPropertyChanged(nameof(SelectedParentCat));
            }
        }
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;

                if (value != null)
                {
                    CategoryName = value.Name;
                    CategoryDescription = value.Description;

                    OnPropertyChanged(nameof(CategoryName));
                    OnPropertyChanged(nameof(CategoryDescription));
                }

                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }

        public string CategoryDescription
        {
            get => _categoryDescription;
            set
            {
                _categoryDescription = value;
                OnPropertyChanged(nameof(CategoryDescription));
            }
        }

        public ObservableCollection<Category> Categories { get; set; }
            = new();

        public ICommand AddCategoryCommand { get; }
        public ICommand UpdateCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public CategoryManagementViewModel()
        {

            LoadParentCategories();
            LoadCategories();

            AddCategoryCommand = new RelayCommand(AddCategory);
            UpdateCategoryCommand = new RelayCommand(UpdateCategory);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory);
        }
        private void DeleteCategory()
        {
            if (SelectedCategory == null)
                return;

            using var db = new AppDbContext();

            var category = db.Categories
                .FirstOrDefault(x => x.Id == SelectedCategory.Id);

            if (category == null)
                return;

            db.Categories.Remove(category);

            db.SaveChanges();

            CategoryName = "";
            CategoryDescription = "";

            LoadCategories();
        }
        private void UpdateCategory()
        {
            if (SelectedCategory == null)
                return;

            using var db = new AppDbContext();

            var category = db.Categories
                .FirstOrDefault(x => x.Id == SelectedCategory.Id);

            if (category == null)
                return;

            category.Name = CategoryName;
            category.Description = CategoryDescription;

            db.SaveChanges();

            LoadCategories();
        }
        private void LoadCategories()
        {
            using var db = new AppDbContext();

            Categories.Clear();

            foreach (var category in db.Categories)
            {
                Categories.Add(category);
            }
        }
        private void LoadParentCategories()
        {
            using var db = new AppDbContext();

            ParentCat.Clear();
            foreach (var category in db.ParentCategories)
            {
                ParentCat.Add(category);
            }
        }

        private void AddCategory()
        {
            if (string.IsNullOrWhiteSpace(CategoryName))
                return;

            using var db = new AppDbContext();

            var category = new Category
            {
                Name = CategoryName,
                Description = CategoryDescription
            };

            db.Categories.Add(category);
            db.SaveChanges();

            CategoryName = "";
            CategoryDescription = "";

            LoadCategories();
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
