namespace ParkingSpotFinderApp
{
	public interface ICar
	{
		public string CarNumber { get; set; }
		public CarType Type { get; }
		public CarSpotType SpotType { get; }

	}
}
