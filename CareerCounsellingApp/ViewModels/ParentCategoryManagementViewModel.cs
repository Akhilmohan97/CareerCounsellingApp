using CareerCounsellingApp.Data;
using CareerCounsellingApp.Helpers;
using CareerCounsellingApp.Models;
using Microsoft.EntityFrameworkCore;
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
    public class ParentCategoryManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
        private string _parentCategory;
        private ParentCategory? _selectedParentCategory;

        public string ParentCategory
        {
            get { return _parentCategory; }
            set { _parentCategory = value;
            OnPropertyChanged(nameof(ParentCategory));}
        }

        public ParentCategory? SelectedParentCategory
        {
            get => _selectedParentCategory;
            set
            {
                _selectedParentCategory = value;
                if (value != null)
                {
                    ParentCategory = value.Name;
                }
                OnPropertyChanged(nameof(SelectedParentCategory));
            }
        }

        public ObservableCollection<ParentCategory> ParentCategories { get; set; } = new();

        public ICommand AddParentCategoryCommand { get; }
        public ICommand UpdateParentCategoryCommand { get; }
        public ICommand DeleteParentCategoryCommand { get; }

        public ParentCategoryManagementViewModel()
        {
            AddParentCategoryCommand = new RelayCommand(AddParentCategory);
            UpdateParentCategoryCommand = new RelayCommand(UpdateParentCategory);
            DeleteParentCategoryCommand = new RelayCommand(DeleteParentCategory);
            LoadParentCategories();
        }

        private void LoadParentCategories()
        {
            using var db = new AppDbContext();
            ParentCategories.Clear();
            foreach (var category in db.ParentCategories.Include(p => p.Categories))
            {
                ParentCategories.Add(category);
            }
        }

        private void AddParentCategory()
        {
            if(!string.IsNullOrWhiteSpace(ParentCategory))
            {
                using var db = new AppDbContext();
                var newParentCategory = new ParentCategory
                {
                    Name = ParentCategory
                };
                db.ParentCategories.Add(newParentCategory);
                db.SaveChanges();
                ParentCategory = string.Empty;
                SelectedParentCategory = null;
                LoadParentCategories();
            }
        }

        private void UpdateParentCategory()
        {
            if (SelectedParentCategory == null || string.IsNullOrWhiteSpace(ParentCategory))
                return;

            using var db = new AppDbContext();
            var parentCategory = db.ParentCategories
                .FirstOrDefault(x => x.Id == SelectedParentCategory.Id);

            if (parentCategory == null)
                return;

            parentCategory.Name = ParentCategory;
            db.SaveChanges();

            ParentCategory = string.Empty;
            SelectedParentCategory = null;
            LoadParentCategories();
        }

        private async void DeleteParentCategory()
        {
            if (SelectedParentCategory == null)
                return;

            using var db = new AppDbContext();
            var parentCategory = db.ParentCategories
                .Include(p => p.Categories)
                .FirstOrDefault(x => x.Id == SelectedParentCategory.Id);

            if (parentCategory == null)
                return;

            // Check if parent category has categories and questions
            if (parentCategory.Categories.Any())
            {
                var totalQuestions = parentCategory.Categories
                    .SelectMany(c => c.Questions)
                    .Count();

                if (totalQuestions > 0)
                {
                    await DialogHelper.ShowErrorAsync(null, "Cannot Delete", 
                        $"Cannot delete parent category '{parentCategory.Name}' because it has {parentCategory.Categories.Count} categories with {totalQuestions} questions associated with it. Please delete all categories and questions first.");
                    return;
                }

                if (parentCategory.Categories.Count > 0)
                {
                    await DialogHelper.ShowErrorAsync(null, "Cannot Delete",
                        $"Cannot delete parent category '{parentCategory.Name}' because it has {parentCategory.Categories.Count} categories associated with it. Please delete all categories first.");
                    return;
                }
            }

            // If validation passes, confirm deletion
            var confirmed = await DialogHelper.ShowConfirmationAsync(null, "Confirm Deletion",
                $"Are you sure you want to delete the parent category '{parentCategory.Name}'?");

            if (confirmed)
            {
                db.ParentCategories.Remove(parentCategory);
                db.SaveChanges();

                ParentCategory = string.Empty;
                SelectedParentCategory = null;
                LoadParentCategories();
            }
        }
    }
}
