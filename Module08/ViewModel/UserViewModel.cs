using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module08.Model;
using Module08.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.VisualBasic.FileIO;

namespace Module08.ViewModel
{
    public class UserViewModel : BindableObject
    {
        private readonly UserService _userService;
        public ObservableCollection<User> Users { get; private set; }

      //Select Item on the list view
        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                UpdateEntryField();
            }
        }

        //Name
        private string _nameInput;
        public string NameInput
        {
            get => _nameInput;
            set
            {
                _nameInput = value;
                OnPropertyChanged();
            }
        }

        //gender
        private string _genderInput;
        public string GenderInput
        {
            get => _genderInput;
            set
            {
                _genderInput = value;
                OnPropertyChanged();
            }
        }

        //ContactNo
        private string _contactNoInput;
        public string ContactNoInput
        {
            get => _contactNoInput;
            set
            {
                _contactNoInput = value;
                OnPropertyChanged();
            }
        }

        private void ClearInput()
        {
            NameInput = string.Empty;
            GenderInput = string.Empty;
            ContactNoInput = string.Empty;
        }

        private void UpdateEntryField()
        {
            if (SelectedUser != null)
            {
                NameInput = SelectedUser.Name;
                GenderInput = SelectedUser.Gender;
                ContactNoInput = SelectedUser.ContactNo;
            }
            else
            {
                ClearInput();
            }
        }

        public UserViewModel() 
        { 
            _userService = new UserService();
            Users = new ObservableCollection<User>();
            LoadUserCommand = new Command(async () => await LoadUsers());
            AddUserCommand = new Command(async () => await AddUser());
            DeleteUserCommand = new Command(async () => await DeleteUser());
            UpdateUserCommand = new Command(async () => await UpdateUser());
        }

        public ICommand LoadUserCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand UpdateUserCommand { get; }

        private async Task UpdateUser()
        {
            if (SelectedUser != null)
            {
                // Update the selected user with the current input values
                SelectedUser.Name = NameInput;
                SelectedUser.Gender = GenderInput;
                SelectedUser.ContactNo = ContactNoInput;

                // Call the service to update the user
                var result = await _userService.UpdateUserAsync(SelectedUser);

                if (result.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    await LoadUsers();  // Reload the users after update
                }
                else
                {
                    // Handle any error messages if needed
                    Console.WriteLine("Update failed: " + result);
                }
            }
        }

        private async Task DeleteUser()
        {
            if (SelectedUser != null)
            {
                var result = await _userService.DeleteUserAsync(SelectedUser.ID);
                await LoadUsers();
                SelectedUser = null;  // Clear selection after deletion
            }
        }


        private async Task LoadUsers()
        {
            var users = await _userService.GetUserAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private async Task AddUser()
        {
            if (!string.IsNullOrWhiteSpace(NameInput) &&
                !string.IsNullOrWhiteSpace(GenderInput) &&
                !string.IsNullOrWhiteSpace(ContactNoInput))
            {
                var newUser = new User
                {
                    Name = NameInput,
                    Gender = GenderInput,
                    ContactNo = ContactNoInput,
                };
                var result = await _userService.AddUserAsync(newUser);
                await LoadUsers();

                if (result.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    await LoadUsers();
                }
            }
        }
    }
}
