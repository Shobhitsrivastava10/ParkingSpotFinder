namespace ParkingSpotFinderApp
{
	public class MiniTruck : ICar
	{
		public string CarNumber { get; set; }
		public CarType Type
		{
			get { return CarType.MiniTruck; }
		}

		public CarSpotType SpotType
		{
			get { return CarSpotType.MiniTruck; }
		}

	}
}
