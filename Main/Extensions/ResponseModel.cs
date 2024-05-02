namespace Main.Extensions
{
    public class ResponseModel
	{
		public ResponseModel()
		{
			PresenterInfo = new PresenterInfo();
        }

		public int VideoId { get; set; }
		public string VideoTitle { get; set; }
		public PresenterInfo PresenterInfo { get; set; }
	}

	public class PresenterInfo
	{
		public int PresenterId { get; set; }
		public string Name { get; set; }
		public string Lastname { get; set; }
	}
}

