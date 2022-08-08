namespace ImageStuff
{
    public class ImageHandler
    {
        public async Task<string> WriteImagetoStorage(IConfiguration config, HttpRequest request)
        {
            int dog = 0;

            dog = TestMeth();        

            if (!request.HasFormContentType)
                return "No Form content Type";

            var form = await request.ReadFormAsync();
            var formFile = form.Files["file"];

            string uploadsFolder = Path.Combine(config["Dog"], "images");
            
            if (formFile is null || formFile.Length == 0)
                return "File size invalid";
            
            await using var stream = formFile.OpenReadStream();

            // var reader = new StreamReader(stream);
            // var text = await reader.ReadToEndAsync();

            string filePath = Path.Combine(uploadsFolder, formFile.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);

            }
            
            return "API Process was good";
        
        }

        private int TestMeth()
        {
            return 1;
        }
    } //End Class ImageHandler

} // end of namespace