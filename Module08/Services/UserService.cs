using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module08.Model;
using System.Net.Http.Json;


namespace Module08.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost/api/";
        public UserService() 
        {
            _httpClient = new HttpClient();
        }

        //GetFromJsonAsync - method to call HTTP GET
        //PostAsJsonAsync - method to call HTTP POST
        //ReadAsStringAsync - method to read the content of  HTTPContent

        //Get User
        public async Task<List<User>> GetUserAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<User>>($"{BaseUrl}get_user.php");
            return response ?? new List<User>();
        }

        //add user
        public async Task<string> AddUserAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}add_user.php", user);
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Add user result: {result}");  // Log the result for debugging
            return result;
        }

        //update user
        public async Task<string> UpdateUserAsync(User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}update_user.php", user);
                if (response.IsSuccessStatusCode)
                {
                    return "Success";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return errorMessage;  // Handle errors from the server
                }
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions that occur during the HTTP request
                Console.WriteLine($"Error updating user: {ex.Message}");
                return "Error";
            }
        }


        //delete user
        public async Task<string> DeleteUserAsync(int userID)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}delete_user.php", new { id = userID });
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Delete result: {result}");  // Log to debug the response
            return result;
        }



    }
}
