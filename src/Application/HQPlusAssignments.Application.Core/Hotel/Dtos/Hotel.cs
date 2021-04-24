namespace HQPlusAssignments.Application.Core.Hotel.Dtos
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Hotel
    {
        public int HotelID { get; set; }
        public int Classification { get; set; }
        public string Name { get; set; }
        public double Reviewscore { get; set; }
    }


}
