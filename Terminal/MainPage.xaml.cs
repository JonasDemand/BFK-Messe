namespace Terminal;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
		InitializeComponent();
	}

	private async void OnWebcamClicked(object sender, EventArgs e)
	{
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                byte[] bytes;
                using Stream sourceStream = await photo.OpenReadAsync();
                using (var memoryStream = new MemoryStream())
                {
                    sourceStream.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                }
                Base64Label.Text = Convert.ToBase64String(bytes);
            }
        }
    }
}
