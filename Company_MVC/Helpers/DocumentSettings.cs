namespace Company_MVC.Helpers
{
    public static class DocumentSettings
    {
        //1. Upload 
        public static string UploadFile(IFormFile file , string folderName)
        {
            //1. Get Folder Loction
            //string folderPath = "D:\\Back end Route\\Eng Ahmed Amin\\07 ASP.NET Core MVC\\Company_MVC\\Company_MVC\\wwwroot\\files\\" + folderName;
            //var foldePath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;

            var foldePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            //2. Get File Path and Make it Unique

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            //File Path

            var filePath = Path.Combine(foldePath, fileName);

         using var fileStream = new FileStream(filePath,FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;


        }

        //2. Delete
        public static void DeleteFile(string fileName , string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

        }

    }
}
