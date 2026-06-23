using CareerCounsellingApp.Helpers;
using System;
using System.Collections.Generic;
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
        private string parentCategory;

        public string ParentCategory
        {
            get { return parentCategory; }
            set { parentCategory = value;
            OnPropertyChanged(nameof(ParentCategory));}
        }
        public ICommand AddParentCategoryCommand { get; }
        public ParentCategoryManagementViewModel()
        {
            AddParentCategoryCommand = new RelayCommand(AddParentCategory);
        }

        private void AddParentCategory()
        {
            if(!string.IsNullOrWhiteSpace(ParentCategory))
            {
                var db= new Data.AppDbContext();
                var newParentCategory = new Models.ParentCategory
                {
                    Name = ParentCategory
                };
                db.ParentCategories.Add(newParentCategory);
                db.SaveChanges();
                ParentCategory = string.Empty;
            }
        }
    }
}
