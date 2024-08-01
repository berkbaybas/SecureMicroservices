using Movies.Client.Models;

namespace Movies.Client.ApiServices
{
    public class MockMovieApiService : IMovieApiService
    {
        public async Task<IEnumerable<Movie>> GetMovies()
        {
            var movieList = new List<Movie>();
            movieList.Add(
                new Movie
                {
                    Id = 1,
                    Genre = "Fantastic",
                    Title = "Harry Potter and Half Blood Prince",
                    ImageUrl = "images/",
                    ReleaseDate = DateTime.Now,
                    Owner = "Berk"
                });

            return await Task.FromResult(movieList);
        }

        public Task<Movie> CreateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovie(string id)
        {
            throw new NotImplementedException();
        }
     

        public Task<Movie> UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task<UserInfoViewModel> GetUserInfo()
        {
            throw new NotImplementedException();
        }
    }
}
