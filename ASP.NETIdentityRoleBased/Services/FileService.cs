namespace ASP.NETIdentityRoleBased.Services
{
    public class FileService : IFileService
    {

        IWebHostEnvironment environment;

        public FileService(IWebHostEnvironment env)
        {
            environment = env;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "uploads");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extensions

                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

                if (!allowedExtensions.Contains(ext))
                {
                    string msg = $"Error. Only {string.Join(",", allowedExtensions)} are allowed";

                    return new Tuple<int, string>(0, msg);
                }

                string uniqueString = Guid.NewGuid().ToString();

                var newFileName = uniqueString + ext;
                
                var fileWithPath = Path.Combine(path, newFileName);
                
                var stream = new FileStream(fileWithPath, FileMode.Create);

                imageFile.CopyTo(stream);

                stream.Close();

                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, $"Error, the image cannot be saved. Error code: { ex.ToString() }");
            }
        }

        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "uploads\\", imageFileName);

                if (File.Exists(path))
                {
                    File.Delete(path);

                    return true;
                }

                return false;
            }
            catch (Exception /* ex */) 
            {
                return false;
            }
        }

    }
}
