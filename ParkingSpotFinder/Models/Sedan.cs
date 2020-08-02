namespace ParkingSpotFinderApp
{
	public class Sedan : ICar
	{
		public string CarNumber { get; set; }
		public CarType Type
		{
			get { return CarType.Sedan; }
		}

		public CarSpotType SpotType
		{
			get { return CarSpotType.Sedan; }
		}


	}
}
